using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Broker.Models;

namespace TopCrypto.ServicesLayer.Interfaces
{
    public interface IBrokersService
    {
        Task RemoveBoker(int id);
        IEnumerable<BrokerTopDTO> GetTopIndex(string cultureName);
        BrokerListingDTO GetTop(string cultureName, int countItems);
        Task<BrokerDTO> GetBrokerById(string name, string cultureName);
        Task<string> GetJsonBrokerListForAdmin();
        Task AddBrokerForAdmin(BrokerAdminDTO model);
        Task<BrokerLocalizationDTO> GetBrokerLocalization(int id, string cultureName);
        Task UpdateBrokerLocalization(int id, string cultureName, BrokerLocalizationDTO dto);
        Task<BrokerAdminDTO> GetBrokerAdminById(int id);
        Task UpdateBroker(int id, BrokerAdminDTO dto);
    }
}