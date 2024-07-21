import { SocketService } from "./../services/socketService";
import { PriceService } from "./../services/priceService";
import { FiatCurencyService } from "./../services/fiatCurencyService";

import { CalculatorInitializer } from "./calculatorInitializer";
import { PriceInitializer } from "./../pageInitializer/priceInitializer";
import { PriceGraphInitializer } from "./priceGraphInitializer";
import { TopBrokersIndexInitializer } from "./topBrokersIndexInitializer";
import { NewsTopInitializer } from "./newsTopInitializer";

import { IDisposable, IStart, ITemplate } from "./../../interfaces/irootbundle";
import { AbstractPageInitializer } from "./../../interfaces/irootbundle";
import { PriceGraphHelper } from "./../services/priceGraphHelper";

class HomeInitializer extends AbstractPageInitializer {
  private disposable: IDisposable[] = [];

  public async onStart() {
    let self = this;
    await self._loadingHtmlPromiseUpdateContainer("api/Template/GetHomeTemplate");

    self._disposableListenerWrapper.call(self, () => {
      //brokerWidget
      let brokerWidgetInitializer = new TopBrokersIndexInitializer(self._route, "broker-widget", null, null);
      self.doDefaultThingsWithInitializar(brokerWidgetInitializer);

      //socket service
      let socketService = new SocketService;
      self.doDefaultThingsWithInitializar(socketService);

      //price service
      let priceService = new PriceService(socketService);
      self.doDefaultThingsWithInitializar(priceService);

      let priceGraphHelper = new PriceGraphHelper();

      //price graph
      let priceGraphService = new PriceGraphInitializer(this._route
        , "price-widget-price"
        , priceGraphHelper);
      //self.doDefaultThingsWithInitializar(priceGraphService);
      self.addToDisposeList(priceGraphService);

      //price widget
      let priceInitializer = new PriceInitializer(this._route
        , priceService
        , "price-widget-price"
        , priceGraphHelper
        , priceGraphService);
      self.doDefaultThingsWithInitializar(priceInitializer);

      let newsTopInitializer = new NewsTopInitializer(this._route
        , "news-widget-index");
      self.doDefaultThingsWithInitializar(newsTopInitializer);
    });
  }

  doDefaultThingsWithInitializar(init: IDisposable & IStart) {
    init.onStart();
    this.addToDisposeList(init);
  }

  addToDisposeList(init: IDisposable) {
    this.disposable.push(init);
  }

  public dispose() {
    super.dispose();
    this.disposable.forEach((item) => { item.dispose(); });
  }
}

export { HomeInitializer };