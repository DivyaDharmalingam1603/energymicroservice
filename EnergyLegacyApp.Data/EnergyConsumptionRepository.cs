using EnergyLegacyApp.Data.Models;

namespace EnergyLegacyApp.Data
{
    public class EnergyConsumptionRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public EnergyConsumptionRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<EnergyConsumption> GetConsumptionByDateRange(DateTime startDate, DateTime endDate)
        {
            return _dbHelper.GetSampleConsumption().Where(c => c.RecordDate >= startDate && c.RecordDate <= endDate).ToList();
        }

        public Dictionary<string, decimal> GetConsumptionByRegionSummary()
        {
            var data = _dbHelper.GetSampleConsumption();
            return data.GroupBy(c => c.Region).ToDictionary(g => g.Key, g => g.Sum(c => c.ConsumptionMWh));
        }
    }
}