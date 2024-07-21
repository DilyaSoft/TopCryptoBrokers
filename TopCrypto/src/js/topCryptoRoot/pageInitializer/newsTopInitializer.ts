import { AbstractPageInitializer } from "./../../interfaces/irootbundle";
import { NavigoRouter } from "../../services/navigoRouter";


class NewsTopInitializer extends AbstractPageInitializer {
  constructor(router: NavigoRouter, containerId: string) {
    super(router, containerId);
  }

  async onStart() {
    let self = this;
    let loadingHtmlPromise = self._loadingHtmlPromiseUpdateContainer("api/News/NewsTopTemplate");

    let newsData = [];

    await $.ajax({
      method: "POST",
      url: "api/News/NewsTop",
      dataType: "json",
      contentType: "application/json"
    }).done((data) => {
      newsData = data instanceof Array ? data : [];
    }).catch(() => {
      console.error("api/Brokers/GetTop");
    });

    await loadingHtmlPromise;
    
    self._disposableListenerWrapper.call(self, () => {      
      let bindingModel = {
        newsData: newsData
      };
      self._koApllyBindings(bindingModel, self._containerId);
    });
  };

  dispose() {
    super.dispose();
    ko.cleanNode(document.getElementById(this._containerId));
  };
}

export { NewsTopInitializer }