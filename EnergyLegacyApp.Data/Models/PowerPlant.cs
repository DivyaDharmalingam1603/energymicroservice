namespace EnergyLegacyApp.Data.Models
{
    public class PowerPlant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Capacity { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal CurrentOutput { get; set; }
        public decimal EfficiencyRating { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
    }
}