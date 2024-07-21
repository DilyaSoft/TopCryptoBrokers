import { IEntryDataService } from "./iEntryDataService";

abstract class AbstractEntryDataServiceFiat<T, K> implements IEntryDataService<T, K> {
  protected _selectedEntry: T[];
  protected _allEntry: T[];

  constructor(selectedEntry: T[], allEntry: T[]) {
    this._selectedEntry = selectedEntry || [];
    this._allEntry = allEntry || [];

    this._sortArrayOfEntries(this._selectedEntry);
    this._sortArrayOfEntries(this._allEntry);
  }

  abstract selectEntry(id: K): void;
  abstract unselectEntry(id: K): void;
  abstract findFromAllItemsWithoutCrossing(partOfText: string): T[];
  protected abstract _sortArrayOfEntries(arr: T[]);

  isNoSelected(): boolean {
    return !!this._selectedEntry.length;
  }

  isAllEntrySelected(): boolean {
    return this._allEntry.length == this._selectedEntry.length;
  }

  getSelectedEntries(): T[] {
    return this._getDeepCopyJSON(this._selectedEntry);
  }

  protected _getDeepCopyJSON(obj) {
    return JSON.parse(JSON.stringify(obj));
  }

  protected _equalIds(id: string, id2: string): boolean {
    if (!id || !id2) return false;
    return id.toLowerCase().trim() === id2.toLowerCase().trim();
  }
}

export { AbstractEntryDataServiceFiat }