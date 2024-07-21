import { AbstractPageInitializer } from "./../../interfaces/irootbundle";
import { NavigoRouter } from "../../services/navigoRouter";

class TopBrokersInitializer extends AbstractPageInitializer {
  private _getItemCount: number = 10;
  private _lastRequestId: number;

  constructor(router: NavigoRouter
    , private containerId: string
    , private _templateUrlPart: string
    , private id: object
    , private otherArgs: any) {
    super(router, containerId);
  }

  async onStart() {
    var self = this;

    var loadingHtmlPromise = self._loadingHtmlPromiseUpdateContainer(`api/Brokers/${self._templateUrlPart}`);

    var bindingModel = {
      brokers: ko.observableArray([]),
      brokersCount: ko.observable(0),
      reviewBrokerClick: (item) => {
        self._redirectTo(`broker/${encodeURIComponent(item.origname)}`);
      }
    };

    await self.updateBindingModel(bindingModel);
    await loadingHtmlPromise;

    self._disposableListenerWrapper.call(self, () => {
      self._koApllyBindings(bindingModel, self._containerId);

      $(`#${self._containerId} #price-hide`).on("click", function () {
        self._getItemCount = 10;
        self.updateBindingModel(bindingModel);
      });

      $(`#${self._containerId} #price-next-10`).on("click", function () {
        self._getItemCount += 10;
        self.updateBindingModel(bindingModel);
      });

      $(`#${self._containerId} #price-view-all`).on("click", function () {
        self._getItemCount = bindingModel.brokersCount();
        self.updateBindingModel(bindingModel);
      });
    });
  };

  private async updateBindingModel(bindingModel) {
    var self = this;

    var requestId = self._random999999();
    self._lastRequestId = requestId;

    await $.ajax({
      method: "POST",
      url: "api/Brokers/GetTop",
      dataType: "json",
      contentType: "application/json",
      data: self._getItemCount.toString()
    }).done((data) => {
      self._disposableListenerWrapper.call(self, () => {
        if (self._lastRequestId == requestId) {
          bindingModel.brokers(data.brokerTopDTOList);
          bindingModel.brokersCount(data.countVisibleTopBrokers);
          self._doAfterPageChanged();
        }
      });
    }).catch(() => {
      console.error("api/Brokers/GetTop");
    });
  }

  dispose() {
    var self = this;
    $(`#${self._containerId} #price-hide`).off();
    $(`#${self._containerId} #price-next-10`).off();
    $(`#${self._containerId} #price-view-all`).off();

    super.dispose();
    ko.cleanNode(document.getElementById(this._containerId));
  };
}

export { TopBrokersInitializer };