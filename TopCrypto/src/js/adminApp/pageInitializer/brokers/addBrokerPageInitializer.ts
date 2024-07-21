import { AbstractEditAddPageInitializer } from "./../../interfaces/abstractEditAddPageInitializer"

class AddBrokerPageInitializer extends AbstractEditAddPageInitializer {
  protected _urlToApiService: string = "/api/Brokers";
  protected _formTitleText = "Add Broker";
  protected _saveApiUrl = "/AddBroker";
  protected _formId: string = "add-broker-form";
  protected _templateApiUrl: string = "/GetAddBrokerTemplate";

  async onStart() {
    let self = this;

    await self._getHtmlTemplatePromise();

    self._disposableListenerWrapper.call(self, () => {
      this._enableJQueryEvents();
      self._setNavigationDisabled(false);
    });
  }

  protected redirectAfterSuccess(): void {
    this._redirectTo("/admin/brokerList");
  }
}

export { AddBrokerPageInitializer }