import { AbstractPageInitializer } from "./../../interfaces/irootbundle";
import { NavigoRouter } from "../../services/navigoRouter";

class TopBrokersIndexInitializer extends AbstractPageInitializer {

  constructor(router: NavigoRouter
    , private containerId: string
    , private id: object
    , private otherArgs: any) {
    super(router, containerId);
  }

  async onStart() {
    var self = this;

    var loadingHtmlPromise = self._loadingHtmlPromiseUpdateContainer(`api/Brokers/GetTopIndexTemplate`);

    var brokersData = [];
    await $.ajax({
      method: "POST",
      url: "api/Brokers/GetTopIndex",
      dataType: "json",
      contentType: "application/json"
    }).done((data) => {
      brokersData = data;
    }).catch(() => {
      console.error("api/Brokers/GetTopIndex");
    });

    await loadingHtmlPromise;

    self._disposableListenerWrapper.call(self, () => {
      var bindingModel = {
        brokers: brokersData,
        reviewBrokerClick: (item) => {
          self._redirectTo(`broker/${encodeURIComponent(item.origname)}`);
        }
      };
      self._koApllyBindings(bindingModel, self._containerId);
    });
  };

  dispose() {
    super.dispose();
    ko.cleanNode(document.getElementById(this._containerId));
  };
}

export { TopBrokersIndexInitializer };