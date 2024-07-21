import { IEntryDataService } from "./../../interfaces/iEntryDataService";
import { AbstractEntryDataServiceFiat } from "./../../interfaces/abstractEntryDataService";

class EntryDataServiceFiat extends AbstractEntryDataServiceFiat<string, string> {
  constructor(selectedEntry: string[], allEntry: string[]) {
    super(selectedEntry, allEntry);
  }

  selectEntry(id: string) {
    if (!id) return;
    let self = this;
    let item = this._allEntry.find(function (item) {
      return self._equalIds(item, id);
    });

    if (!item) return;

    this._selectedEntry.push(item);
    this._sortArrayOfEntries(this._selectedEntry);
  };

  unselectEntry(id: string) {
    for (let i = 0; i < this._selectedEntry.length; i++) {
      if (this._equalIds(this._selectedEntry[i], id)) {
        this._selectedEntry.splice(i, 1);
        break;
      }
    }
  };

  findFromAllItemsWithoutCrossing(partOfText: string): string[] {
    if (!partOfText) {
      return this._removeCrossingEntry(
        this._allEntry, this._selectedEntry);
    }

    let founded = [];
    for (let i = 0; i < this._allEntry.length; i++) {
      if (~this._allEntry[i].toLowerCase().trim()
        .indexOf(partOfText.toLowerCase().trim())) {
        founded.push(this._allEntry[i]);
      }
    }

    let arrWithoutCrossing = this._removeCrossingEntry(founded, this._selectedEntry);
    return this._getDeepCopyJSON(arrWithoutCrossing);
  }

  protected _sortArrayOfEntries(arr: string[]) {
    if (!arr || !arr.length) return;

    arr.sort(function (item1, item2) {
      if (item1 < item2) return -1;
      if (item1 > item2) return 1;
      return 0;
    });
  }

  protected _removeCrossingEntry(array: string[], array2: string[]): string[] {
    let self = this;
    return array.filter(function (item) {
      return !array2.find(
        function (item2) { return self._equalIds(item, item2); })
    });
  }

  protected _getDeepCopyJSON(obj) {
    return JSON.parse(JSON.stringify(obj));
  }
}

export { EntryDataServiceFiat }