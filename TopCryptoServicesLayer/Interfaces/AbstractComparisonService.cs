using System;
using System.Collections.Generic;
using TopCrypto.ServicesLayer.SocketServices.WebSockets;
using TopCrypto.ServicesLayer.WebSockets.Models;

namespace TopCrypto.ServicesLayer.Interfaces
{
    public abstract class AbstractComparisonService<T, K> where T : K, IComparisonDTO
    {
        protected IList<K> lastResult = new List<K>();
        protected object lockDown = new Object();

        public IList<T> GetLastResult()
        {
            lock (lockDown)
            {
                IList<T> listT = new List<T>();
                var enumerator = lastResult.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    listT.Add(ConvertToComparisonDTO(enumerator.Current, SocketComparisonEnum.Added));
                }

                return listT;
            }
        }

        public IList<T> GetDiffrencess(IList<K> newResult)
        {
            lock (lockDown)
            {
                var comparions = new List<T>();
                var copyedNewResult = new List<K>();

                var orderedNewResult = OrderList(newResult);
                copyedNewResult.AddRange(orderedNewResult);
                while (copyedNewResult.Count > 0)
                {
                    var founded = false;
                    for (var i = 0; i < lastResult.Count; i++)
                    {
                        var itemL = lastResult[i];
                        var itemN = copyedNewResult[0];

                        if (!EqualityById(itemL, itemN)) continue;

                        if (!(EqualityByData(itemL, itemN)))
                        {
                            comparions.Add(ConvertToComparisonDTO(itemN, SocketComparisonEnum.Updated));
                        }
                        else
                        {
                            comparions.Add(ConvertToComparisonDTO(itemN, SocketComparisonEnum.NotModified));
                        }
                        founded = true;
                        lastResult.RemoveAt(i);
                        break;
                    }
                    if (!founded)
                    {
                        comparions.Add(ConvertToComparisonDTO(copyedNewResult[0], SocketComparisonEnum.Added));
                    }

                    copyedNewResult.RemoveAt(0);
                }

                foreach (var item in lastResult)
                {
                    comparions.Add(ConvertToComparisonDTO(item, SocketComparisonEnum.Deleted));
                }
                lastResult = orderedNewResult;

                return comparions;
            }
        }

        public abstract T ConvertToComparisonDTO(K marketPriceDTO, SocketComparisonEnum comparison);
        public abstract bool EqualityByData(K item1, K item2);
        public abstract bool EqualityById(K item1, K item2);
        public abstract IList<K> OrderList(IList<K> lastResult);
    }
}
