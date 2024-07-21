import { IDisposable } from "./../../interfaces/irootbundle";

abstract class SocketConnectionServices<T extends SocketConnectionDTO> implements IDisposable {
  protected _subscribers: ((obj: T[], additionalData: Object) => void)[] = [];
  protected _currentData: T[] = [];
  protected _additionalData: Object = {};

  _notifySubscribers(newData: T[], additionalData: Object) {
    this._subscribers.forEach((fn) => {
      fn(newData, additionalData);
    });
  }

  dispose() {
    this._currentData = [];
    this._subscribers = [];
  }

  getCurrentData(): T[] {
    return this._sort(JSON.parse(JSON.stringify(this._currentData)));
  }

  getCurrentAdditionalData(): T[] {
    return JSON.parse(JSON.stringify(this._additionalData));
  }

  subscribe(fn: (obj: T[], additionalData: Object) => void) {
    fn(this.getCurrentData(), this.getCurrentAdditionalData());
    this._subscribers.push(fn);
  }

  abstract _updateUpdatedItem(currentItem: T, changedItem: T): void;
  abstract _changeItemEqual(item1: T, item2: T): boolean;
  abstract _addNewItem(newItem: T): void;
  abstract _getAdditionalData(obj: Object): Object;
  abstract _sort(data: T[]): T[];

  _onItemChange(key: string, responseObject: Object) {
    var returnedData = <T[]>responseObject[key];
    var self = this;

    if (returnedData && returnedData.length) {
      for (var i = 0; i < returnedData.length; i++) {
        var item = returnedData[i];

        switch (item.Code) {
          //added
          case (0):
            var findedItem = self._currentData.find((filteredItem) => { return self._changeItemEqual(item, filteredItem); });
            if (findedItem) break;
            self._addNewItem(item);
            self._currentData.push(item);
            break;
          //updated
          case (1):
            var findedItem = self._currentData.find((filteredItem) => { return self._changeItemEqual(item, filteredItem); });
            if (!findedItem) {
              break;
            }
            self._updateUpdatedItem(findedItem, item);
            break;
          //deleted
          case (2):
            for (var i = 0; i < self._currentData.length; i++) {
              if (!self._changeItemEqual(self._currentData[i], item)) continue;

              self._currentData.splice(i, 1);
              break;
            }
            break;
          default:
        }
        item.Code = null;
      }
    }

    self._additionalData = self._getAdditionalData(responseObject);
    self._notifySubscribers(self.getCurrentData(), self.getCurrentAdditionalData());
  }
}

class SocketConnectionDTO {
  Code: number;
}

export { SocketConnectionServices, SocketConnectionDTO };