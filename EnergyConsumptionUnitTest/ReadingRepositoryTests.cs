using EnergyConsumption.Repository;
using EnergyConsumption.Repository.Interfaces;
using EnergyConsumption.Data;
using NSubstitute;
using System.Collections.Generic;
using System.Data;
using Xunit;
using ExpectedObjects;

namespace EnergyConsumptionUnitTest
{
    public class ReadingRepositoryTests
    {
        [Fact]
        public void CheckReadingFormat_ReturnDataSet()

        {
            
            IReadingRepository ireadingRepositoryMock = Substitute.For<IReadingRepository>();
            //var datatable = EmbeddedJsonFileHelper.GetContent<DataTable>(@"Files\datatable.transactions");
            using (var test_Stream = this.GetType().Assembly.GetManifestResourceStream(@"Mocks\Meter_Value")) 
            {
                var expectedResult = EmbeddedJsonFileHelper.GetContent<List<MeterReading>>(@"Mocks\FormatOutput").ToExpectedObject();
                var accounts= EmbeddedJsonFileHelper.GetContent<List<Account>>(@"Mocks\TestAccounts").ToExpectedObject();
                //ireadingRepositoryMock.ProcessMeterReadings().Returns(x => expectedResult);
            }

        }
    }
}
