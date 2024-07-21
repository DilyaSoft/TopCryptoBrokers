import { EntryRenderServiceCoin } from "./../services/coins/entryRenderServiceCoin";
import { EntryDataServiceCoin, CoinIds } from "./../services/coins/entryDataServiceCoin";

import { IEntryRenderService } from "./../interfaces/iEntryRenderService";
import { IEntryDataServiceCoin } from "./../services/coins/iEntryDataServiceCoin";

import { AbstractPageInitializer } from "./../../interfaces/irootbundle"

interface CryptoCurrencyDTO {
  id: number
  , symbol: string
  , selectedMarket: string
}

class CoinListPageInitializer extends AbstractPageInitializer {
  private _urlToApiService: string = "/api/CoinInfo";

  onStart(): void {
    let initedItems: CryptoCurrencyDTO[] = [];
    let allItems: CoinIds[] = [];

    let self = this;

    let loadingHtmlPromise = self._loadingHtmlPromiseUpdateContainer(self._urlToApiService + "/CoinInfoTemplate");

    let allItemsPR = $.post(self._urlToApiService + "/GetApiIds")
      .done(function (data) {
        allItems = data;
      });
    let initedItemsPR = $.post(self._urlToApiService + "/GetDataBaseIds")
      .done(function (data) {
        initedItems = data;
      });


    Promise.all([initedItemsPR, allItemsPR, loadingHtmlPromise]).then(
      self._disposableListenerWrapper.bind(self,
        function (data) {
          let filteredItems = [];

          allItems.filter((item) => {
            let foundedObj = initedItems.find((entry) => entry.id.toString() == item.id.toString());
            if (foundedObj) {
              if (~item.markets.indexOf(foundedObj.selectedMarket)) {
                item.selectedMarket = foundedObj.selectedMarket;
              } else {
                item.selectedMarket = item.markets && item.markets.length ? item.markets[0] : null;
              }
              filteredItems.push(item);
            }
          });

          self._onStartAfterDataLoad(filteredItems, allItems);
        }));
  }

  _onStartAfterDataLoad(initedItems: CoinIds[], allItems: CoinIds[]) {
    let entryDataService: IEntryDataServiceCoin = new EntryDataServiceCoin(initedItems, allItems);
    let self = this;

    let entryRenderService: IEntryRenderService<CoinIds, string> = new EntryRenderServiceCoin(entryDataService,
      "selectedEntriesTbl",
      "foundedItems",
      "inputFilter",
      "emptyMessage",
      "saveEntriesMessage",
      "saveEntriesBtn",
      "allEntrySelectedMessage",
      "messagePost");
    entryRenderService.drawBothContainers();

    $(`#${self._containerId} #inputFilter`).on("input.coinList", function () {
      let text = <string>$(this).val();
      entryRenderService.updateFoundedEntry(text);
    });

    $(`#${self._containerId} #clearInputFilter`).on("click.coinList", function () {
      $("#inputFilter").val("");
      entryRenderService.updateFoundedEntry(null);
    });

    $(`#${self._containerId} #foundedItems`).on("click.coinList", "li label", function () {
      entryRenderService.selectEntry($(this).closest("li").data("entryid").toString());
    });

    $(`#${self._containerId} #selectedEntriesTbl`).on("click.coinList", "li label", function () {
      entryRenderService.unselectEntry($(this).closest("li").data("entryid").toString());
    });

    $(`#${self._containerId} #foundedItems, #${self._containerId} #selectedEntriesTbl`)
      .on("change.coinList", "li select", function () {
        entryDataService.selectEntryMarket(
          $(this).closest("li").data("entryid").toString(),
          $(this).val().toString());
      });

    $(`#${self._containerId} #saveEntriesBtn`).on("click.coinList", function () {
      entryRenderService.preClickOnSaveEntryButton();

      let ajaxRequest = $.ajax(self._urlToApiService + "/SaveDataBaseIds",
        {
          data: JSON.stringify(entryRenderService.getSelectedEntries()),
          method: "POST",
          dataType: "json",
          contentType: "application/json; charset=utf-8"
        });
      entryRenderService.clickOnSaveEntryButton(ajaxRequest);

      return false;
    });

    entryRenderService.unDisableSaveButton();
  }

  dispose(): void {
    super.dispose();
    let self = this;
    $(`#${self._containerId} #inputFilter`).off(".coinList");
    $(`#${self._containerId} #clearInputFilter`).off(".coinList");
    $(`#${self._containerId} #foundedItems`).off(".coinList");
    $(`#${self._containerId} #selectedEntriesTbl`).off(".coinList");
    $(`#${self._containerId} #saveEntriesBtn`).off(".coinList");
  }
}

export { CoinListPageInitializer };