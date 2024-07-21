using System.Collections.Generic;

namespace TopCrypto.ServicesLayer.Interfaces
{
    public abstract class AbstractFilterService<T, Z>
    {
        protected IList<T> GetFilteredDataSorting(IList<T> marketPriceValues, Z[] coinIds)
        {
            if (coinIds.Length == 0 || marketPriceValues.Count == 0) return new List<T>();

            var copyCoinList = new List<Z>();
            copyCoinList.AddRange(coinIds);

            var filteredList = new List<T>();
            for (var i = 0; i < marketPriceValues.Count; i++)
            {
                for (var j = 0; j < copyCoinList.Count; j++)
                {
                    if (EqualsById(marketPriceValues[i], copyCoinList[j]))
                    {
                        filteredList.Add(marketPriceValues[i]);
                        copyCoinList.RemoveAt(j);
                        break;
                    }
                }
                if (copyCoinList.Count == 0) break;
            }
            return filteredList;
        }

        protected abstract bool EqualsById(T t, Z z);
    }
}
