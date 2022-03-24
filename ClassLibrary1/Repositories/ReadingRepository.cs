using EnergyConsumption.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using EnergyConsumption.Repository;
using EnergyConsumption.Repository.Interfaces;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Data;
using EnergyConsumption.Repository.Entities;
using System.Text.RegularExpressions;

namespace DataAccess.EFCore.Repositories
{
    public class ReadingRepository : GenericRepository<MeterReading>, IReadingRepository
    {
        public ReadingRepository(EnergyConsumptionContext context) : base(context)
        {
        }
        public ReadingResult ProcessMeterReadings(StreamReader stream,IEnumerable<Account> accounts)
        {
            ReadingResult readingResult=new ReadingResult();
            var csv = new CsvReader(stream, CultureInfo.InvariantCulture);
            var dr = new CsvDataReader(csv);
            var dt = new DataTable();
            DataSet formattedData=new DataSet();
            ReadingResult validInvalid = new ReadingResult();
            dt.Load(dr);

            //Validate reading formats
            formattedData = CheckReadingFormat(dt);
            //Get valid readings based on removeduplicates and avoid data not available in accounts table
            readingResult.ValidReading= filterReadings(formattedData.Tables[0], accounts);
            readingResult.InValidReading=formattedData.Tables[1];

            return readingResult;
        }

        private List<MeterReading> filterReadings(DataTable dataReadings,IEnumerable<Account> accounts)
        {
            MeterReading reading = null;
            List<MeterReading> readings = new List<MeterReading>();
            DataTable dtFilteredReading = dataReadings.Clone();

            foreach (DataRow dr in dataReadings.AsEnumerable())
            {
                reading = new MeterReading() { AccountId = Convert.ToInt32(dr.ItemArray[0]), MeterReadingDateTime = Convert.ToDateTime(dr.ItemArray[1]), MeterReadValue = Convert.ToInt32(dr.ItemArray[2]) };
                readings.Add(reading);
            }

            var finalReadings = from meterReading in readings
                                group meterReading by new { meterReading.AccountId }
                                     into mIdGroup
                                select mIdGroup.OrderBy(groupedReading => groupedReading.MeterReadingDateTime)
                                                .Last();
            return finalReadings.Where(f => accounts.Any(id => id.AccountId == f.AccountId)).ToList();
            
            


        }

        public DataSet CheckReadingFormat(DataTable dt)
        {
            DataSet formatted = new DataSet();
            ReadingResult validInvalidReadings = new ReadingResult();
            DataTable dtError = null;
            DataTable dtData = null;
            List<DataTable> validInvalid = new List<DataTable>();

            if (dtData == null) dtData = dt.Clone();
            foreach (DataRow row in dt.AsEnumerable())
            {

                Boolean error = false;
                foreach (DataColumn col in dt.Columns.OfType<DataColumn>().Take(3))

                {

                    RegexCompare regexCompare = RegexCompare.dict[col.ColumnName];
                    object colValue = row.Field<object>(col.ColumnName);
                    if (regexCompare.Required)
                    {
                        if (colValue == null)
                        {
                            error = true;
                            break;
                        }
                    }
                    else
                    {
                        if (colValue == null)
                            continue;
                    }
                    string colValueStr = colValue.ToString();
                    Match match = Regex.Match(colValueStr, regexCompare.Pattern);

                    if (!match.Success)
                    {
                        error = true;
                        break;
                    }
                    if (colValueStr.Length != match.Value.Length)
                    {
                        error = true;
                        break;
                    }

                    else
                    {
                        if (match.Success)
                        {
                            error = false;

                        }
                    }

                }

                if (error)
                {
                    if (dtError == null) dtError = dt.Clone();
                    dtError.Rows.Add(row.ItemArray);

                }
                else
                {

                    dtData.Rows.Add(row.ItemArray);
                }


            }
            while (dtData.Columns.Count > 3)
            {
                dtData.Columns.RemoveAt(3);
                formatted.Tables.Add(dtData);

            }
            if(dtError != null)
            { 
            while (dtError.Columns.Count > 3)
            {
                dtError.Columns.RemoveAt(3);
            }
                dtError.Columns.Add("Failure_Reason", typeof(System.String));
                foreach (DataRow dr in dtError.Rows)
                {
                    
                    dr["Failure_Reason"] = "Meter reading value is not numeric";  
                }
            formatted.Tables.Add(dtError);
                }

            return formatted;
        }
    }
}
