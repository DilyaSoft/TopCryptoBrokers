import { IEntryDataServiceCoin } from "./IEntryDataServiceCoin";
import { AbstractEntryDataServiceFiat } from "./../../interfaces/abstractEntryDataService";

interface CoinIds {
  id: string;
  markets: string[];
  name: string;
  symbol: string;
  selectedMarket: string;
}

class EntryDataServiceCoin extends AbstractEntryDataServiceFiat<CoinIds, string>
  implements IEntryDataServiceCoin {

  constructor(selectedEntry: CoinIds[], allEntry: CoinIds[]) {
    super(selectedEntry, allEntry);
  }

  selectEntry(id: string) {
    if (!id) return;
    let self = this;
    let item = this._allEntry.find(function (item) {
      return self._equalIds(item.id, id);
    });

    if (!item) return;

    if (!item.selectedMarket && item.markets && item.markets.length) {
      item.selectedMarket = item.markets[0];
    }

    this._selectedEntry.push(item);
    this._sortArrayOfEntries(this._selectedEntry);
  };

  selectEntryMarket(id: string, marketId: string) {
    if (!id || !marketId) return;
    let self = this;
    let item = this._allEntry.find(function (item) {
      return self._equalIds(item.id, id);
    });

    if (!item || !~item.markets.indexOf(marketId)) return;

    item.selectedMarket = marketId;
  }

  unselectEntry(id: string) {
    for (let i = 0; i < this._selectedEntry.length; i++) {
      if (this._equalIds(this._selectedEntry[i].id, id)) {
        this._selectedEntry.splice(i, 1);
        break;
      }
    }
  };

  findFromAllItemsWithoutCrossing(partOfText: string): CoinIds[] {
    if (!partOfText) {
      return this._removeCrossingEntry(
        this._allEntry, this._selectedEntry);
    }

    let founded = [];
    for (let i = 0; i < this._allEntry.length; i++) {
      if (~this._allEntry[i].name.toLowerCase().trim()
        .indexOf(partOfText.toLowerCase().trim())) {
        founded.push(this._allEntry[i]);
      }
    }

    let arrWithoutCrossing = this._removeCrossingEntry(founded, this._selectedEntry);
    return this._getDeepCopyJSON(arrWithoutCrossing);
  }

  protected _sortArrayOfEntries(arr: CoinIds[]) {
    if (!arr || !arr.length) return;

    arr.sort(function (item1, item2) {
      if (item1.name < item2.name) return -1;
      if (item1.name > item2.name) return 1;
      return 0;
    });
  }

  protected _removeCrossingEntry(array: CoinIds[], array2: CoinIds[]): CoinIds[] {
    let self = this;
    return array.filter(function (item) {
      return !array2.find(
        function (item2) { return self._equalIds(item.id, item2.id); })
    });
  }

  protected _equalIds(id: string, id2: string): boolean {
    if (!id || !id2) return false;
    return id.toLowerCase().trim() === id2.toLowerCase().trim();
  }
}

export { EntryDataServiceCoin, CoinIds }