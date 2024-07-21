import { AbstractPageInitializer } from "./../../../interfaces/irootbundle"
import { BrokerListTableInitializer } from "./../../services/BrokerListTableInitializer"

class BrokerListPageInitializer extends AbstractPageInitializer {

  private _urlToApiService: string = "/api/Brokers";
  private _tableId: string = "brokers-list";
  private _addBtnId: string = "add-new-broker";

  async onStart() {
    let self = this;
    let listData = [];

    let loadingHtmlPromise = self._loadingHtmlPromiseUpdateContainer(self._urlToApiService + "/GetBrokerListTemplate");

    let loadingTableData = $.post(self._urlToApiService + "/GetBrokerListData")
      .done(self._disposableListenerWrapper.bind(self, (data) => {
        if (!data) {
          listData = [];
        } else {
          listData = JSON.parse(data);
        }
      })).fail((e) => {
        console.error(self._urlToApiService + "/GetBrokerListData");
      });

    await loadingHtmlPromise;
    await loadingTableData;

    self._disposableListenerWrapper.call(self, () => {
      self._initializeBrokerTable(listData);
      self._enableJQueryListeners();
    });
  }

  private _initializeBrokerTable(listData) {
    let tableInitializer = new BrokerListTableInitializer(this._tableId);
    tableInitializer.initializeTable(listData);

    this._enableJQueryListenersForTable();
  }

  private _removeCounter = 0;
  private _enableJQueryListenersForTable() {
    let self = this;
    $(`#${self._containerId} #${self._tableId} button`).on("click.brokerListPageInitializer", function () {
      let data = $(this).data();
      self._redirectTo(`/admin/editBrokerLocalization/${encodeURIComponent(data.idItem)}/${encodeURIComponent(data.culture)}`);
    });

    $(`#${self._containerId} #${self._tableId} .remove-broker`).on("click.brokerListPageInitializer", function () {
      let data = $(this).data();
      if (!confirm("This broker will be permanently removed")) return;
      var countId = ++self._removeCounter;

      let loadLocalizationData = $.ajax(`${self._urlToApiService}/RemoveBroker`, {
        method: "POST",
        data: {
          id: +data.idItem
        }
      }).done(self._disposableListenerWrapper.bind(self, (data) => {
        if (self._removeCounter != countId) return;
        self._initializeBrokerTable(JSON.parse(data));
      })).fail(() => {
        console.error(`${self._urlToApiService}/GetBrokerLocalizationData`);
      });
    });
  }

  private _enableJQueryListeners() {
    let self = this;

    $(`#${self._containerId} #${self._addBtnId}`).on("click.brokerListPageInitializer", function () {
      self._redirectTo("/admin/addBroker");
    });
  }
}

export { BrokerListPageInitializer };