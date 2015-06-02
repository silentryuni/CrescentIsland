using System.Threading.Tasks;

namespace CrescentIsland.Website.Models.Interfaces
{
    public interface IUserRepository
    {
        User CreateNewUser(RegisterViewModel model, string mapPath);

        Task<bool> UpdateHealth(int healthChange, int maxHealthChange = 0);
        Task<bool> UpdateEnergy(int energyChange, int maxEnergyChange = 0);

        void Dispose();
    }
}