using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Broker.Interfaces;
using TopCrypto.DataLayer.Services.Broker.Models;
using TopCrypto.DataLayer.Services.CustomCache;
using TopCrypto.ServicesLayer.Interfaces;

namespace TopCrypto.ServicesLayer
{
    public class BrokersService : IBrokersService
    {
        private int CACHE_SECONDS = 60 * 5;
        private string _getDetailCacheQueryPattern = "cultureName:{0};id:{1}";
        private static object _lockObject = new object();
        private static object _lockObjectDetail = new object();

        private InternalCache _internalCache;
        private IBrokerDataService _brokerDataService;

        public BrokersService(IBrokerDataService brokerDataService, InternalCache internalCache)
        {
            this._brokerDataService = brokerDataService;
            this._internalCache = internalCache;
        }

        public async Task RemoveBoker(int id)
        {
            await _brokerDataService.RemoveBroker(id);
            _internalCache.CleanStorageHard(InternalCacheKeys.Brokers);
        }

        private BrokerListingDTO GetTop(string cultureName)
        {
            lock (_lockObject)
            {
                var brokersListing = _internalCache.Get(InternalCacheKeys.Brokers, cultureName);
                if (brokersListing != null)
                {
                    return (BrokerListingDTO)brokersListing;
                }

                var newsTask = _brokerDataService.GetTopBrokers(cultureName);
                newsTask.Wait();
                brokersListing = newsTask.Result;
                _internalCache.Add(InternalCacheKeys.Brokers, brokersListing, CACHE_SECONDS, cultureName);

                return (BrokerListingDTO)brokersListing;
            }
        }

        public IEnumerable<BrokerTopDTO> GetTopIndex(string cultureName)
        {
            return GetTop(cultureName)?.BrokerTopDTOList?.Take(9);
        }

        public BrokerListingDTO GetTop(string cultureName, int countItems)
        {
            var cachingObject = GetTop(cultureName);

            if (cachingObject == null || cachingObject.BrokerTopDTOList == null) return null;

            return new BrokerListingDTO()
            {
                CountVisibleTopBrokers = cachingObject.BrokerTopDTOList.Count,
                BrokerTopDTOList = cachingObject.BrokerTopDTOList.Take(countItems).ToList()
            };
        }

        public Task<BrokerDTO> GetBrokerById(string name, string cultureName)
        {
            lock (_lockObjectDetail)
            {
                var cacheQuery = string.Format(_getDetailCacheQueryPattern, cultureName, name);
                var brokerDTO = _internalCache.Get(InternalCacheKeys.Brokers, cacheQuery);
                if (brokerDTO != null)
                {
                    return Task.FromResult<BrokerDTO>((BrokerDTO)brokerDTO);
                }

                var newsTask = _brokerDataService.GetBrokerById(name, cultureName);
                newsTask.Wait();
                _internalCache.Add(InternalCacheKeys.Brokers, newsTask.Result, CACHE_SECONDS, cacheQuery);

                return newsTask;
            }
        }

        #region Admin
        public async Task<string> GetJsonBrokerListForAdmin()
        {
            return await _brokerDataService.GetJsonBrokerListForAdmin();
        }

        public async Task AddBrokerForAdmin(BrokerAdminDTO model)
        {
            await _brokerDataService.AddBrokerForAdmin(model);
        }

        public async Task<BrokerLocalizationDTO> GetBrokerLocalization(int id, string cultureName)
        {
            return await _brokerDataService.GetBrokerLocalization(id, cultureName);
        }

        public async Task UpdateBrokerLocalization(int id, string cultureName, BrokerLocalizationDTO dto)
        {
            await _brokerDataService.UpdateBrokerLocalization(id, cultureName, dto);

            var cacheQuery = string.Format(_getDetailCacheQueryPattern, cultureName, id);
            _internalCache.CleanStorageHard(InternalCacheKeys.Brokers);
        }

        public async Task<BrokerAdminDTO> GetBrokerAdminById(int id)
        {
            return await _brokerDataService.GetBrokerAdminById(id);
        }

        public async Task UpdateBroker(int id, BrokerAdminDTO dto)
        {
            await _brokerDataService.UpdateBroker(id, dto);
            _internalCache.CleanStorageHard(InternalCacheKeys.Brokers);
        }
        #endregion
    }
}
