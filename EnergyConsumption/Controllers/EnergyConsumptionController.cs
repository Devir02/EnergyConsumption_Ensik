using EnergyConsumption.Repository;
using EnergyConsumption.Repository.Entities;
using EnergyConsumption.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;


namespace EnergyConsumption.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnergyConsumptionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        public EnergyConsumptionController(IUnitOfWork unitOfWork,ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        [HttpPost("meter-reading-uploads")]
        public IActionResult ImportMeterReadings([FromForm] IFormFile file)
        {
            try
            {


                ReadingResult readings = new ReadingResult();
                var accounts = GetAccounts();
                var existingReadings = GetExistingReadings();

               
                if (file.Length > 0)
                {
                    var stream = file.OpenReadStream();
                    using (var reader = new StreamReader(stream))

                    {
                        readings = _unitOfWork.Readings.ProcessMeterReadings(reader, accounts);
                    }
                }

                IEnumerable<MeterReading> toInsert = null;
                IEnumerable<MeterReading> toUpdate = null;

                if (existingReadings != null)
                {
                    toUpdate = (from c in readings.ValidReading
                                where c != null
                                from p in existingReadings.Where(x => c.AccountId == x.AccountId && c.MeterReadingDateTime < x.MeterReadingDateTime)
                                select new MeterReading() { AccountId = c.AccountId, MeterReadValue = c.MeterReadValue, MeterReadingDateTime = c.MeterReadingDateTime });

                    toInsert = readings.ValidReading.Where(p => existingReadings.All(p2 => p2.AccountId != p.AccountId));

                }
               

                foreach (MeterReading r in toInsert)
                {

                    _unitOfWork.Readings.Add(r);
                    _unitOfWork.Complete();

                }
                foreach (MeterReading r in toUpdate)
                {

                    _unitOfWork.Readings.Update(r);
                    _unitOfWork.Complete();

                }
                var model = CreateResponseModel(readings);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Error creating inserting/updating meter readings");

            }
        }

        public IEnumerable<Account> GetAccounts()
        {

            var account = _unitOfWork.Accounts.GetAll();
            return account;
        }
        public IEnumerable<MeterReading> GetExistingReadings()
        {

            var existingReadings = _unitOfWork.Readings.GetAll();
            return existingReadings;
        }
        private MeterReadingsModel CreateResponseModel(ReadingResult context)
        {
            var model = new MeterReadingsModel
            {
                Successes = context.ValidReading.Count,
                Failures = context.InValidReading.Rows.Count,
            };
            
            model.FailedRecords = JsonConvert.SerializeObject(context.InValidReading);
            return model;
        }


    }

}

