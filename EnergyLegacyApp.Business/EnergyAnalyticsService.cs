using EnergyLegacyApp.Data;
using EnergyLegacyApp.Data.Models;

namespace EnergyLegacyApp.Business
{
    public class EnergyAnalyticsService
    {
        private readonly EnergyConsumptionRepository _consumptionRepository;
        private readonly PowerPlantRepository _powerPlantRepository;

        public EnergyAnalyticsService(EnergyConsumptionRepository consumptionRepository, PowerPlantRepository powerPlantRepository)
        {
            _consumptionRepository = consumptionRepository;
            _powerPlantRepository = powerPlantRepository;
        }

        public Dictionary<string, object> GetDashboardData()
        {
            var allPlants = _powerPlantRepository.GetAllPowerPlants();
            var lastMonth = DateTime.Now.AddDays(-30);
            var consumptionData = _consumptionRepository.GetConsumptionByDateRange(lastMonth, DateTime.Now);

            return new Dictionary<string, object>
            {
                ["TotalCapacity"] = allPlants.Sum(p => p.Capacity),
                ["ActivePlants"] = allPlants.Count(p => p.Status == "Active"),
                ["TotalPlants"] = allPlants.Count,
                ["PlantsByType"] = allPlants.GroupBy(p => p.Type).ToDictionary(g => g.Key, g => g.Count()),
                ["MonthlyConsumption"] = consumptionData.Sum(c => c.ConsumptionMWh),
                ["MonthlyEmissions"] = consumptionData.Sum(c => c.CarbonEmissions)
            };
        }

        public List<object> GetEfficiencyAnalysis()
        {
            return _powerPlantRepository.GetAllPowerPlants().Select(plant => new
            {
                PlantName = plant.Name,
                Type = plant.Type,
                Efficiency = plant.EfficiencyRating,
                Status = GetEfficiencyStatus(plant.EfficiencyRating),
                Recommendation = GetEfficiencyRecommendation(plant)
            }).Cast<object>().ToList();
        }

        private string GetEfficiencyStatus(decimal efficiency)
        {
            return efficiency switch
            {
                < 0.6m => "Poor",
                < 0.8m => "Average",
                > 0.95m => "Excellent",
                _ => "Good"
            };
        }

        private string GetEfficiencyRecommendation(PowerPlant plant)
        {
            if (plant.EfficiencyRating < 0.6m)
                return "Consider major overhaul or replacement";
            else if (plant.EfficiencyRating < 0.8m)
                return "Schedule maintenance and efficiency upgrades";
            else if (plant.Type == "Coal" && plant.EfficiencyRating < 0.85m)
                return "Consider conversion to cleaner fuel source";
            return "Operating at optimal efficiency";
        }

        public Dictionary<string, decimal> GetRegionalAnalysis()
        {
            var consumptionSummary = _consumptionRepository.GetConsumptionByRegionSummary();
            var regions = new[] { "North", "South", "East", "West", "Central" };
            
            return regions.ToDictionary(region => region, region => consumptionSummary.GetValueOrDefault(region, 0));
        }

        public List<MaintenanceAlert> GetMaintenanceAlerts()
        {
            var alerts = new List<MaintenanceAlert>();
            var plants = _powerPlantRepository.GetAllPowerPlants();

            foreach (var plant in plants)
            {
                var daysSinceLastMaintenance = (DateTime.Now - plant.LastMaintenanceDate).Days;
                var maintenanceInterval = plant.Type switch
                {
                    "Nuclear" => 90,
                    "Coal" => 120,
                    "Gas" => 180,
                    "Solar" => 365,
                    "Wind" => 180,
                    _ => 365
                };

                if (daysSinceLastMaintenance > maintenanceInterval)
                {
                    alerts.Add(new MaintenanceAlert
                    {
                        PlantId = plant.Id,
                        PlantName = plant.Name,
                        Type = plant.Type,
                        DaysOverdue = daysSinceLastMaintenance - maintenanceInterval,
                        Priority = daysSinceLastMaintenance > maintenanceInterval * 1.5m ? "High" : "Medium"
                    });
                }
            }

            return alerts;
        }

        public string ExportDataToCsv(string dataType)
        {
            return dataType switch
            {
                "plants" => ExportPlantsToCsv(),
                "consumption" => ExportConsumptionToCsv(),
                _ => string.Empty
            };
        }

        private string ExportPlantsToCsv()
        {
            var plants = _powerPlantRepository.GetAllPowerPlants();
            var header = "Id,Name,Type,Capacity,Location,Status,CurrentOutput,Efficiency\n";
            var rows = string.Join("\n", plants.Select(p => 
                $"{p.Id},{p.Name},{p.Type},{p.Capacity},{p.Location},{p.Status},{p.CurrentOutput},{p.EfficiencyRating}"));
            return header + rows;
        }

        private string ExportConsumptionToCsv()
        {
            var lastMonth = DateTime.Now.AddDays(-30);
            var consumption = _consumptionRepository.GetConsumptionByDateRange(lastMonth, DateTime.Now);
            var header = "Id,PowerPlantId,Date,ConsumptionMWh,PeakDemand,Region,CostPerMWh\n";
            var rows = string.Join("\n", consumption.Select(c => 
                $"{c.Id},{c.PowerPlantId},{c.RecordDate:yyyy-MM-dd},{c.ConsumptionMWh},{c.PeakDemand},{c.Region},{c.CostPerMWh}"));
            return header + rows;
        }
    }
}
