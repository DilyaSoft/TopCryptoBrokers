import { PriceService, PriceDTO } from "./../services/priceService";
import { FiatCurencyService, FiatCurrencyDTO } from "./../services/fiatCurencyService";
import { AbstractPageInitializer } from "./../../interfaces/irootbundle";
import { NavigoRouter } from "../../services/navigoRouter";

import { SocketService } from "../services/socketService";

import { IDisposable, IStart, ITemplate } from "./../../interfaces/irootbundle";

class CalculatorDTO {
  id: string;
  name: string;
  price_usd: number;
}

interface IFromToCalculatorDTOStorage {
  FromObj: CalculatorDTO;
  ToObj: CalculatorDTO;
}

class CalculatorInitializer extends AbstractPageInitializer {
  private _disposable: IDisposable[] = [];
  private _priceService: PriceService;
  private _fiatCurencyService: FiatCurencyService;

  constructor(
    router: NavigoRouter,
    containerId: string) {
    super(router, containerId);

    var socketService = new SocketService;
    this.doDefaultThingsWuthInitializar(socketService);

    //price service
    this._priceService = new PriceService(socketService);
    this.doDefaultThingsWuthInitializar(this._priceService);

    //fiat Curency Service
    this._fiatCurencyService = new FiatCurencyService(socketService);
    this.doDefaultThingsWuthInitializar(this._fiatCurencyService);
  }

  private doDefaultThingsWuthInitializar(init: IDisposable & IStart) {
    init.onStart();
    this._disposable.push(init);
  }

  async onStart() {
    var self = this;
    await self._loadingHtmlPromiseUpdateContainer("api/Template/GetCalculatorTemplate");

    self._disposableListenerWrapper.call(self, () => {
      var bindingModel = {
        prices: ko.observableArray([])
      };

      var recalculatePriceFromTo = self.recalculatePrice.bind(self, bindingModel, "From", "To");
      $(`#${self._containerId} #calculatorFromCount`).on("input", recalculatePriceFromTo);
      $(`#${self._containerId} #calculatorToCoin, #${self._containerId} #calculatorFromCoin`).on("change", recalculatePriceFromTo);

      $(`#${self._containerId} #calculatorToCount`).on("input", self.recalculatePrice.bind(self, bindingModel, "To", "From"));

      self._koApllyBindings(bindingModel, self._containerId);

      self._priceService.subscribe(self.updatePriceDataViewModel.bind(self, bindingModel));
      self._priceService.updateVisibleItemsCount(200);

      self._fiatCurencyService.subscribe(self.updatePriceDataViewModel.bind(self, bindingModel));
    });
  }

  private recalculatePrice(bindedModel, part1, part2) {
    var self = this;
    var calculatorItems = $(`#${self._containerId} .calculator-items`);
    var countCurrency = +calculatorItems.find(`#calculator${part1}Count`).val();

    var fromId = calculatorItems.find(`#calculatorFromCoin`).val();
    var toId = calculatorItems.find(`#calculatorToCoin`).val();

    if (!countCurrency || !fromId || !toId) return;

    var prices = bindedModel.prices().filter((item) => { return item.id == fromId || item.id == toId; });

    var storeDTO: IFromToCalculatorDTOStorage = { FromObj: null, ToObj: null };
    storeDTO.FromObj = prices.find((item) => { return item.id == fromId; });
    storeDTO.ToObj = prices.find((item) => { return item.id == toId; });

    if (storeDTO[`${part1}Obj`].price_usd == null ||
      storeDTO[`${part2}Obj`].price_usd == null) {
      $(`#${self._containerId} #calculator${part2}Count`).val("No Data");
    } else {
      $(`#${self._containerId} #calculator${part2}Count`)
        .val((storeDTO[`${part1}Obj`].price_usd / storeDTO[`${part2}Obj`].price_usd * countCurrency).toFixed(6));
    }
  }

  private updatePriceDataViewModel(bindedModel) {
    var self = this;

    var buff = <CalculatorDTO[]>self._priceService.getCurrentData();
    buff = buff.concat(<CalculatorDTO[]>self._fiatCurencyService.getCurrentData());
    bindedModel.prices(buff);
    self.recalculatePrice(bindedModel, "From", "To");
  }

  dispose() {
    var self = this;
    $(`#${self._containerId} #calculatorFromCount`).off();
    $(`#${self._containerId} #calculatorToCoin, #${self._containerId} #calculatorFromCoin`).off();
    $(`#${self._containerId} #calculatorToCount`).off();

    super.dispose();
    ko.cleanNode(document.getElementById(this._containerId));
    this._disposable.forEach((item) => { item.dispose(); })
  }
}

export { CalculatorInitializer };