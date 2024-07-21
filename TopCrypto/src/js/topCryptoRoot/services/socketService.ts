import { IDisposable } from "./../../interfaces/irootbundle";

class SocketSubscriber {
  constructor(public key: string, public fn: (obj: Object) => void) { }
}

class SocketService implements IDisposable {
  private _subscribers: SocketSubscriber[] = [];
  private _socket: WebSocket;
  private _connectionPromise: Promise<{}>;

  subscribe(obj: SocketSubscriber) {
    this._subscribers.push(obj);
  }

  onStart() {
    let self = this;

    let protocol = location.protocol === "https:" ? "wss:" : "ws:";
    let wsUri = protocol + "//" + window.location.host + '/api/MarketDataSocket/ConnectSocket';
    self._socket = new WebSocket(wsUri);

    self._connectionPromise = new Promise(function (resolve, reject) {
      self._socket.onopen = function () {
        resolve();
      };
    });


    self._socket.onmessage = (e) => {
      let returnedData = JSON.parse(e.data);

      self._subscribers.forEach((obj) => {
        if (returnedData[obj.key]) obj.fn(returnedData);
      });
    };

    this._socket.onclose = (e) => {
      console.warn("socket closed");
    }

    this._socket.onerror = (e) => {
      console.error(e);
    }
  }

  send(text: string) {
    var self = this;
    self._connectionPromise.then(() => {
      if (self._socket != null) self._socket.send(text);
    });
  }

  dispose() {
    this._subscribers = [];
    if (this._socket != null) this._socket.close();
  }
}

export { SocketService, SocketSubscriber };