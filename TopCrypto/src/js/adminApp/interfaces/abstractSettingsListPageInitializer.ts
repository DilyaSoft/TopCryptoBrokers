import { AbstractPageInitializer } from "./../../interfaces/irootbundle"
import { SettingsListTableInitializer } from "./../services/settingsListTableInitializer"

abstract class AbstractSettingsListPageInitializer extends AbstractPageInitializer {
  protected _urlToApiService = "/api/Settings";
  protected _tableId = "settings-list";
  protected abstract _urlToData;

  async onStart() {
    let self = this;
    let listData = [];

    let loadingHtmlPromise = self._loadingHtmlPromiseUpdateContainer(self._urlToApiService + "/GetAllSettingsTemplate");

    let loadingTableData = $.post(self._urlToApiService + self._urlToData)
      .done(self._disposableListenerWrapper.bind(self, (data) => {
        listData = data;
      })).fail((e) => {
        console.error(self._urlToApiService + self._urlToData);
      });

    await loadingHtmlPromise;
    await loadingTableData;

    self._disposableListenerWrapper.call(self, () => {
      let tableInitializer = new SettingsListTableInitializer(self._tableId);
      tableInitializer.initializeTable(listData);

      self._enableJQueryListeners();
    });
  }

  protected _enableJQueryListeners() {
    let self = this;
    $(`#${self._containerId} #${self._tableId} tbody tr`).on("click", function () {
      let data = $(this).data();
      if (data.query) {
        self._redirectTo(`/admin/editSetting/${encodeURIComponent(data.id)}/${encodeURIComponent(data.query)}`);
      } else {
        self._redirectTo(`/admin/editSetting/${encodeURIComponent(data.id)}`);
      }
    });
  }
}

export { AbstractSettingsListPageInitializer }