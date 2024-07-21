import { SocketConnectionServices, SocketConnectionDTO } from "./socketConnectionServices";
import { SocketSubscriber, SocketService } from "./socketService";
import { IDisposable } from "./../../interfaces/irootbundle";

class PriceService extends SocketConnectionServices<PriceDTO> implements IDisposable {

  constructor(private _socketService: SocketService) {
    super();
  }

  onStart() {
    var self = this;
    this._socketService.subscribe(new SocketSubscriber("priceCoint", self._onItemChange.bind(self, "priceCoint")));
  }

  _getAdditionalData(obj: Object): Object {
    return {
      countOfPrices: obj["countOfPrices"],
      grapthNodes: obj["grapthNodes"]
    };
  }

  _addNewItem(newItem): void { }

  _updateUpdatedItem(currentItem: PriceDTO, changedItem: PriceDTO): void {
    ["price_usd", "price_btc", "percent_change_24h", "market_cap_usd"].forEach((field) => {
      currentItem[field] = changedItem[field];
    });
  }

  updateVisibleItemsCount(pageCount: number) {
    this._socketService.send(JSON.stringify({ "visiblePriceCount": pageCount }));
  }

  _changeItemEqual(item1: PriceDTO, item2: PriceDTO): boolean {
    return item1.id == item2.id;
  }

  _sort(data: PriceDTO[]): PriceDTO[] {
    return data.sort((a, b) => { return b.price_usd - a.price_usd; });
  }
}

class PriceDTO extends SocketConnectionDTO {
  id: string;
  name: string;
  price_usd: number;
  price_btc: number;
  percent_change_24h: number;
  market_cap_usd: number;
}

export { PriceService, PriceDTO };