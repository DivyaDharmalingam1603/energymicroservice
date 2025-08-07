using EnergyLegacyApp.Data.Models;

namespace EnergyLegacyApp.Data
{
    public class PowerPlantRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public PowerPlantRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<PowerPlant> GetAllPowerPlants()
        {
            return _dbHelper.GetSamplePowerPlants();
        }
    }
}