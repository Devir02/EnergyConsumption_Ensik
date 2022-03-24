using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EnergyConsumption.Repository.Entities
{
    public  class ReadingResult
    {
        public List<MeterReading> ValidReading { get; set; }  
        public DataTable InValidReading { get; set; }

    }
}
