import { AbstractPageInitializer } from "./../../interfaces/irootbundle";
import { NavigoRouter } from "../../services/navigoRouter";


class NewsDetailInitializer extends AbstractPageInitializer {
  private _itemId: number;
  constructor(router: NavigoRouter, containerId: string, itemId: number) {
    super(router, containerId);
    this._itemId = itemId;
  }

  async onStart() {
    var self = this;
    var loadingHtmlPromise = self._loadingHtmlPromiseUpdateContainer("api/News/NewsDetailTemplate");

    var newsData = {};

    await $.ajax({
      method: "POST",
      url: "api/News/NewsDetail",
      data: { link: self._itemId }
    }).done((data) => {
      newsData = data;
    }).catch(() => {
      console.error("api/Brokers/GetTop");
    });

    await loadingHtmlPromise;

    self._disposableListenerWrapper.call(self, () => {
      self._koApllyBindings(newsData, self._containerId);
    });
  };

  dispose() {
    super.dispose();
    ko.cleanNode(document.getElementById(this._containerId));
  };
}

export { NewsDetailInitializer }