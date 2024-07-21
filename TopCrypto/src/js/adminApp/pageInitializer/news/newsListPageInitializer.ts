import { AbstractPageInitializer } from "./../../../interfaces/irootbundle"
import { NewsListTableInitializer } from "./../../services/newsListTableInitializer"

class NewsListPageInitializer extends AbstractPageInitializer {
  private _urlToApiService: string = "api/News";
  private _tableId: string = "news-list";
  private _addBtnId: string = "add-new-news";

  async onStart() {
    let self = this;
    let listData = [];

    let loadingHtmlPromise = self._loadingHtmlPromiseUpdateContainer(self._urlToApiService + "/GetNewsListTemplate");

    let loadingTableData = $.post(self._urlToApiService + "/GetNewsListData")
      .done(self._disposableListenerWrapper.bind(self, (data) => {
        if (!data) {
          listData = [];
        } else {
          listData = JSON.parse(data);
        }
      })).fail((e) => {
        console.error(self._urlToApiService + "/GetNewsListData");
      });

    await loadingHtmlPromise;
    await loadingTableData;

    self._disposableListenerWrapper.call(self, () => {
      let tableInitializer = new NewsListTableInitializer(self._tableId);
      tableInitializer.initializeTable(listData);

      self._enableJQueryListeners();
    });
  }

  private _enableJQueryListeners() {
    let self = this;
    $(`#${self._containerId} #${self._tableId}`).on("click.newsList", "button", function () {
      let data = $(this).data();
      self._redirectTo(`/admin/editNewsLocalization/${encodeURIComponent(data.idItem)}/${encodeURIComponent(data.culture)}`);
    });

    $(`#${self._containerId} #${self._addBtnId}`).on("click.newsList", function () {
      self._redirectTo("/admin/addNews");
    });
  }
}

export { NewsListPageInitializer }