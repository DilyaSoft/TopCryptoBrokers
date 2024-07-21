interface IEntryRenderService<T, K> {
  drawSelectedItems(): void;
  updateFoundedEntry(text: string): void;
  unselectEntry(id: K): void;
  selectEntry(id: K): void;
  drawBothContainers(): void;
  getSelectedEntries(): T[];
  showFocusMessage(containerId: string, classAdd: string, classToRemove: string, text: string): void;
  preClickOnSaveEntryButton(): void;
  clickOnSaveEntryButton(saveEntriesAjaxRequest: JQuery.jqXHR<any>): void;
  unDisableSaveButton(): void;
}

export { IEntryRenderService }