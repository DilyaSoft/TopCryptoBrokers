import { AbstractPageInitializer } from "./../../interfaces/irootbundle";
import { NavigoRouter } from "../../services/navigoRouter";

class BrokerDetailInitializer extends AbstractPageInitializer {
  constructor(router: NavigoRouter, containerId: string, private _itemId: number) {
    super(router, containerId);
  }

    async onStart() {
        var self = this;

    var loadingHtmlPromise = self._loadingHtmlPromiseUpdateContainer("api/Brokers/GetBrokerDetailTemplate");

    var brokerData = {};
    var pr = $.ajax({
      method: "POST",
      url: "api/Brokers/GetBroker",
      data: { name: self._itemId }
    }).done(
      self._disposableListenerWrapper.bind(self, (data) => {
        brokerData = data;
      })).catch(() => {
        console.error("api/Brokers/GetBroker");
      });

    await loadingHtmlPromise;
    await pr;

    self._disposableListenerWrapper.call(self, () => {

      var viewModel = brokerData;
      self._koApllyBindings(viewModel, self._containerId);
    });
  }

  dispose() {
    super.dispose();
    ko.cleanNode(document.getElementById(this._containerId));
  }
}

export { BrokerDetailInitializer };