interface IEntryDataService<T, K> {
  selectEntry(id: K): void;
  unselectEntry(id: K): void;
  isNoSelected(): boolean;
  isAllEntrySelected(): boolean;
  getSelectedEntries(): T[];
  findFromAllItemsWithoutCrossing(partOfText: string): T[];
}

export { IEntryDataService }