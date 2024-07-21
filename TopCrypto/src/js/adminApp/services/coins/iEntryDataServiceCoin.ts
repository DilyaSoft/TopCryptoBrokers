import { IEntryDataService } from "./../../interfaces/iEntryDataService";
import { CoinIds } from "./EntryDataServiceCoin";

interface IEntryDataServiceCoin extends IEntryDataService<CoinIds, string> {
  selectEntryMarket(id: string, marketId: string): void;
}

export { IEntryDataServiceCoin }