using EnergyLegacyApp.Data.Models;

namespace EnergyLegacyApp.Data
{
    public class DatabaseHelper
    {
        public List<PowerPlant> GetSamplePowerPlants()
        {
            return new List<PowerPlant>
            {
                new PowerPlant { Id = 1, Name = "Plant A", Type = "Coal", Capacity = 500, Location = "North", Status = "Active", CurrentOutput = 450, EfficiencyRating = 0.85m, LastMaintenanceDate = DateTime.Now.AddDays(-60) },
                new PowerPlant { Id = 2, Name = "Plant B", Type = "Gas", Capacity = 300, Location = "South", Status = "Active", CurrentOutput = 280, EfficiencyRating = 0.92m, LastMaintenanceDate = DateTime.Now.AddDays(-30) }
            };
        }

        public List<EnergyConsumption> GetSampleConsumption()
        {
            return new List<EnergyConsumption>
            {
                new EnergyConsumption { Id = 1, PowerPlantId = 1, RecordDate = DateTime.Now.AddDays(-1), ConsumptionMWh = 450, PeakDemand = 500, Region = "North", CostPerMWh = 50, CarbonEmissions = 200 },
                new EnergyConsumption { Id = 2, PowerPlantId = 2, RecordDate = DateTime.Now.AddDays(-1), ConsumptionMWh = 280, PeakDemand = 300, Region = "South", CostPerMWh = 45, CarbonEmissions = 120 }
            };
        }
    }
}