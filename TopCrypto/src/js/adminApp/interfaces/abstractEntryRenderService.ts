import { IEntryDataService } from "./iEntryDataService";
import { IEntryRenderService } from "./iEntryRenderService";

abstract class AbstractEntryRenderService<T, K> implements IEntryRenderService<T, K> {
  protected _dataService: IEntryDataService<T, K>;
  protected _selectedContainerId: string;
  protected _foundedEntriesContainer: string;
  protected _inputfilterId: string;
  protected _emptyMessageId: string;
  protected _saveEntriesMessageId: string;
  protected _saveButtonId: string;
  protected _allEntrySelectedMessageId: string;
  protected _messagePostSave: string;

  constructor(dataService: IEntryDataService<T, K>,
    selectedContainerId: string,
    foundedEntriesContainer: string,
    inputfilterId: string,
    emptyMessageId: string,
    saveEntriesMessageId: string,
    saveButtonId: string,
    allEntrySelectedMessageId: string,
    messagePostSave: string) {
    this._dataService = dataService;
    this._selectedContainerId = selectedContainerId;
    this._foundedEntriesContainer = foundedEntriesContainer;
    this._inputfilterId = inputfilterId;
    this._emptyMessageId = emptyMessageId;
    this._saveEntriesMessageId = saveEntriesMessageId;
    this._saveButtonId = saveButtonId;
    this._allEntrySelectedMessageId = allEntrySelectedMessageId;
    this._messagePostSave = messagePostSave;
  }

  showFocusMessage(containerId: string, classAdd: string, classToRemove: string, text: string): void {
    $("#" + containerId)
      .addClass(classAdd)
      .removeClass(classToRemove)
      .text(text).show()
      .attr("tabindex", -1).focus();
  }

  preClickOnSaveEntryButton() {
    let self = this;
    $("#" + self._saveButtonId).attr("disabled", 1);
    $("#" + self._messagePostSave).hide();
  }

  clickOnSaveEntryButton(saveEntriesAjaxRequest: JQuery.jqXHR<any>) {
    let self = this;
    saveEntriesAjaxRequest.done((res) => {
      self.showFocusMessage(self._messagePostSave,
        "bg-green-message-info",
        "bg-red-message-info",
        "success");
    });
    saveEntriesAjaxRequest.fail((res) => {
      self.showFocusMessage(self._messagePostSave,
        "bg-red-message-info",
        "bg-green-message-info",
        "error");
    });
    saveEntriesAjaxRequest.always(() => {
      $("#" + self._saveButtonId).attr("disabled", null)
    });
  }

  getSelectedEntries(): T[] {
    return this._dataService.getSelectedEntries();
  }

  drawSelectedItems(): void {
    let selectedEntriesUl = document.getElementById(this._selectedContainerId);

    let selectEntry = this._dataService.getSelectedEntries();
    let fragment = document.createDocumentFragment();
    for (let i = 0; i < selectEntry.length; i++) {
      let li = this.createLi(selectEntry[i]);
      fragment.insertBefore(li, null);
    }

    selectedEntriesUl.innerHTML = "";
    selectedEntriesUl.insertBefore(fragment, null);
  }

  protected _renderSelectedItems(partOfText: string): DocumentFragment {
    let foundedItems = this._dataService.findFromAllItemsWithoutCrossing(partOfText);

    let fragment = document.createDocumentFragment();
    this._hideShow(!foundedItems.length && !this._dataService.isAllEntrySelected(), this._emptyMessageId);
    this._hideShow(this._dataService.isAllEntrySelected(), this._allEntrySelectedMessageId);
    if (!foundedItems.length) {
      return fragment;
    }

    foundedItems.forEach((item) => {
      let li = this.createLi(item);
      fragment.insertBefore(li, null);
    });
    return fragment;
  }

  protected _hideShow(flag: boolean, id: string): void {
    if (flag) {
      $(`#${id}`).removeClass("hidden");
    } else {
      $(`#${id}`).addClass("hidden");
    }
  }

  updateFoundedEntry(text: string): void {
    let fragment = this._renderSelectedItems(text);

    let element = document.getElementById(
      this._foundedEntriesContainer);
    element.innerHTML = "";
    element.insertBefore(fragment, null);
  }

  unselectEntry(id: K): void {
    this._dataService.unselectEntry(id);
    this.drawBothContainers();
    this._renderSaveFunctional();
  }

  protected _renderSaveFunctional(): void {
    this._hideShow(!this._dataService.isNoSelected(), this._saveEntriesMessageId);

    $(`#${this._saveButtonId}`)
      .prop("disabled", !this._dataService.isNoSelected());
  }

  selectEntry(id: K): void {
    this._dataService.selectEntry(id);
    this.drawBothContainers();
    this._renderSaveFunctional();
  }

  drawBothContainers(): void {
    this.drawSelectedItems();
    let partOfText =
      (<HTMLInputElement>document.getElementById(this._inputfilterId)).value;
    this.updateFoundedEntry(partOfText);
  }

  unDisableSaveButton() {
    $("#" + this._saveButtonId).removeAttr("disabled");
  }

  protected abstract createLi(item: T);
}

export { AbstractEntryRenderService }