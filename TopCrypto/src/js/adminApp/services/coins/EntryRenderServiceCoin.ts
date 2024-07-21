import { IEntryDataService } from "./../../interfaces/iEntryDataService";
import { AbstractEntryRenderService } from "./../../interfaces/abstractEntryRenderService";
import { CoinIds } from "./EntryDataServiceCoin";

class EntryRenderServiceCoin extends AbstractEntryRenderService<CoinIds, string> {
  constructor(dataService: IEntryDataService<CoinIds, string>,
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

  protected createLi(item: CoinIds) {
    let select = document.createElement("select");
    select.classList.add("form-control");
    for (let i = 0; i < item.markets.length; i++) {
      let option = document.createElement("option");

      option.textContent = item.markets[i];
      option.selected = item.selectedMarket == item.markets[i];
      select.appendChild(option);
    }

    let div = document.createElement("div");
    div.classList.add("col-md-7");
    div.classList.add("col-sm-7");
    div.classList.add("col-xs-12");

    div.appendChild(select);

    let label = document.createElement("label");
    label.classList.add("control-label");
    label.classList.add("col-md-5");
    label.classList.add("col-sm-5");
    label.classList.add("col-xs-12");
    label.textContent = item.name;

    let li = document.createElement("li");
    li.classList.add("form-group");
    li.appendChild(label);
    li.appendChild(div);
    li.dataset["entryid"] = item.id;

    return li;
  }
}

export { EntryRenderServiceCoin }