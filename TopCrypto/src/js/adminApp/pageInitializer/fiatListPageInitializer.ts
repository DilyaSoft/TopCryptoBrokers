import { EntryRenderServiceFiat } from "./../services/fiat/entryRenderServiceFiat";
import { EntryDataServiceFiat } from "./../services/fiat/entryDataServiceFiat";

import { IEntryRenderService } from "./../interfaces/iEntryRenderService";
import { IEntryDataService } from "./../interfaces/iEntryDataService";

import { AbstractPageInitializer } from "./../../interfaces/irootbundle"

class FiatListPageInitializer extends AbstractPageInitializer {

  private _urlToApiService: string = "/api/FiatCurrency";

  onStart(): void {
    let initedItems = [];
    let allItems = [];

    let self = this;

    let loadingHtmlPromise =
      self._loadingHtmlPromiseUpdateContainer("/api/CoinInfo/CoinInfoTemplate");

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
          self._onStartAfterDataLoad(initedItems, allItems);
        }));
  }

  _onStartAfterDataLoad(initedItems: string[], allItems: string[]) {
    let entryDataService: IEntryDataService<string, string> = new EntryDataServiceFiat(initedItems, allItems);
    let self = this;

    let entryRenderService: IEntryRenderService<string, string> = new EntryRenderServiceFiat(entryDataService,
      "selectedEntriesTbl",
      "foundedItems",
      "inputFilter",
      "emptyMessage",
      "saveEntriesMessage",
      "saveEntriesBtn",
      "allEntrySelectedMessage",
      "messagePost");
    entryRenderService.drawBothContainers();

    $("#inputFilter").on("input.filter", function () {
      let text = <string>$(this).val();
      entryRenderService.updateFoundedEntry(text);
    });

    $("#clearInputFilter").on("click.clearFilter", function () {
      $("#inputFilter").val("");
      entryRenderService.updateFoundedEntry(null);
    });

    $("#foundedItems").on("click.selectEntry", "li", function () {
      entryRenderService.selectEntry($(this).data("entryid"));
    });

    $("#selectedEntriesTbl").on("click.unselectEntry", "li", function () {
      entryRenderService.unselectEntry($(this).data("entryid"));
    });

    $("#saveEntriesBtn").on("click", function () {
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
    $("#inputFilter").off("input.filter");
    $("#clearInputFilter").off("click.clearFilter");
    $("#foundedItems.selectEntry").off("click.removeitem", "li");
    $("#selectedEntriesTbl").off("click.unselectEntry", "button");
  }
}

export { FiatListPageInitializer };