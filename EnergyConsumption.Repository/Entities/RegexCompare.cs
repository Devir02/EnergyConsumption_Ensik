using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyConsumption.Repository.Entities
{
    public class RegexCompare
    {
        public string ColumnName { get; set; }
        public string Pattern { get; set; }
     
        public Boolean Required { get; set; }

        public static Dictionary<string, RegexCompare> dict = new Dictionary<string, RegexCompare>() {
               { "AccountId", new RegexCompare() { ColumnName = "AccountId", Pattern = @"[\d]+", Required = true}},
               { "MeterReadingDateTime", new RegexCompare() { ColumnName = "MeterReadingDateTime",Pattern=@"^([1-9]|([012][0-9])|(3[01]))-([0]{0,1}[1-9]|1[012])-\d\d\d\d [012]{0,1}[0-9]:[0-6][0-9]$", Required = true}},
               { "MeterReadValue", new RegexCompare() { ColumnName = "MeterReadValue", Pattern = @"[\d]+",Required = true}},
            };

    }
}
