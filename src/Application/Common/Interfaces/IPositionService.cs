namespace RocketStore.Application.Common.Interfaces
{
    using System.Threading.Tasks;
    using RocketStore.Domain.Entities;

    public interface IPositionService
    {
        Task<AddressData> GetAddress(string address);
    }
}