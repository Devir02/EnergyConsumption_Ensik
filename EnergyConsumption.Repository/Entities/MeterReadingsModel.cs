

namespace EnergyConsumption.Repository.Entities
{
    public class MeterReadingsModel
    {
        public  int Successes { get; set; }
        public int Failures { get; set; }
        public string FailedRecords { get; set; }
    }
}
