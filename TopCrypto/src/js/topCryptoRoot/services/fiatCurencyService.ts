import { SocketConnectionServices, SocketConnectionDTO } from "./socketConnectionServices";
import { SocketSubscriber, SocketService } from "./socketService";
import { IDisposable } from "./../../interfaces/irootbundle";

class FiatCurencyService extends SocketConnectionServices<FiatCurrencyDTO> implements IDisposable {

  constructor(private _socketService: SocketService) {
    super();
  }

  onStart() {
    var self = this;
    this._socketService.subscribe(new SocketSubscriber("fiatCurency", self._onItemChange.bind(self, "fiatCurency")));
  }

  _getAdditionalData(obj: Object): Object {
    return {};
  }

  _addNewItem(newItem): void {
    newItem["price_usd"] = newItem.value;
    newItem["id"] = newItem["name"];
  }

  _updateUpdatedItem(currentItem: FiatCurrencyDTO, changedItem: FiatCurrencyDTO): void {
    currentItem["price_usd"] = changedItem["value"];
    currentItem["value"] = changedItem["value"];
  }

  _changeItemEqual(item1: FiatCurrencyDTO, item2: FiatCurrencyDTO): boolean {
    return item1.name == item2.name;
  }

  _sort(data: FiatCurrencyDTO[]): FiatCurrencyDTO[] {
    return data.sort((a, b) => { return a.name > b.name ? 1 : -1; });
  }
}

class FiatCurrencyDTO extends SocketConnectionDTO {
  id: string;
  name: string;
  price_usd: number;
  value: number;
}

export { FiatCurencyService, FiatCurrencyDTO };