import { AbstractPageInitializer } from "./../../interfaces/irootbundle";
import { PriceService, PriceDTO } from "./../services/priceService";
import { PriceGraphHelper } from "./../services/priceGraphHelper";
import { PriceGraphInitializer } from "./priceGraphInitializer"
import { NavigoRouter } from "../../services/navigoRouter";

class PriceInitializer extends AbstractPageInitializer {
  private _countOfVisibleNodes: number = 10;
  private _grapthNodes;
  constructor(route: NavigoRouter
    , private priceService: PriceService
    , containerId
    , private priceGraphHelper: PriceGraphHelper
    , private priceGraphInitializer: PriceGraphInitializer) {
    super(route, containerId);
  }

  async onStart() {
    let self = this;

    await self._loadingHtmlPromiseUpdateContainer("api/CoinInfo/PriceTemplate");

    await self.priceGraphInitializer.onStart();

    self._disposableListenerWrapper.call(self, () => {
      self.priceGraphInitializer.redrawView();

      let bindingModel = {
        prices: ko.observableArray([])
        , countOfVisibleNodes: ko.observable(self._countOfVisibleNodes)
        , count: ko.observable(0)
      };

      $(`#${self._containerId} #price-hide`).click(() => {
        self._countOfVisibleNodes = 10;
        self.priceService.updateVisibleItemsCount(self._countOfVisibleNodes);
        self.updateCountOfVisibleNodes(bindingModel, self._countOfVisibleNodes);
      });

      $(`#${self._containerId} #price-view-all`).click(() => {
        let visibleNodes = bindingModel.count() + 200;
        self.priceService.updateVisibleItemsCount(visibleNodes);
        self.updateCountOfVisibleNodes(bindingModel, visibleNodes);
      });

      $(`#${self._containerId} #price-next-10`).click(() => {
        self._countOfVisibleNodes += 10;
        self.priceService.updateVisibleItemsCount(self._countOfVisibleNodes);
        self.updateCountOfVisibleNodes(bindingModel, self._countOfVisibleNodes);
      });

      self._koApllyBindings(bindingModel, self._containerId);
      self.priceService.subscribe(self.updatePricesInviewModel.bind(self, bindingModel));
    });
  };

  updateCountOfVisibleNodes(bindedModel, countOfVisibleNodes: number) {
    bindedModel.countOfVisibleNodes(countOfVisibleNodes);
  }

  private updateTime() {
    let timePlusNull = (num: number) => {
      if (num < 10) {
        return '0' + num;
      }
      return num;
    }

    let dt = new Date();

    let date = timePlusNull(dt.getUTCDate());
    let year = dt.getFullYear();
    let mnth = timePlusNull(dt.getUTCMonth());

    let hh = timePlusNull(dt.getUTCHours());
    let mm = timePlusNull(dt.getUTCMinutes());
    let sec = timePlusNull(dt.getUTCSeconds());

    let str = `\u00A0\u00A0\u00A0\u00A0${date}.${mnth}.${year} ${hh}:${mm}:${sec} GMT`;

    $(".market-update-time").text(str);
  }

  async updatePricesInviewModel(bindedModel, newPrices: PriceDTO[], additionalData: Object) {
    bindedModel.prices(newPrices);
    bindedModel.count(additionalData["countOfPrices"]);
    if (additionalData["grapthNodes"]) { this._grapthNodes = additionalData["grapthNodes"]; }
    this.priceGraphHelper.writeSimpleGraphForObject("price-graph-", this._grapthNodes);
    await this.priceGraphInitializer.reselectLastSelectedContentId();
    this.updateTime();
    this._doAfterPageChanged();
  }

  dispose() {
    super.dispose();
    ko.cleanNode(document.getElementById(this._containerId));
  }
}

export { PriceInitializer };