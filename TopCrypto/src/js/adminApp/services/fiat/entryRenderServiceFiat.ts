import { IEntryDataService } from "./../../interfaces/iEntryDataService";
import { AbstractEntryRenderService } from "./../../interfaces/abstractEntryRenderService";

class EntryRenderServiceFiat extends AbstractEntryRenderService<string, string> {
  constructor(dataService: IEntryDataService<string, string>,
    selectedContainerId: string,
    foundedEntriesContainer: string,
    inputfilterId: string,
    emptyMessageId: string,
    saveEntriesMessageId: string,
    saveButtonId: string,
    allEntrySelectedMessageId: string,
    messagePostSave: string) {
    super(dataService
      , selectedContainerId
      , foundedEntriesContainer
      , inputfilterId
      , emptyMessageId
      , saveEntriesMessageId
      , saveButtonId
      , allEntrySelectedMessageId
      , messagePostSave);
  }

  protected createLi(item: string) {
    let li = document.createElement("li");
    li.textContent = item;
    li.dataset["entryid"] = item;

    return li;
  }
}

export { EntryRenderServiceFiat }