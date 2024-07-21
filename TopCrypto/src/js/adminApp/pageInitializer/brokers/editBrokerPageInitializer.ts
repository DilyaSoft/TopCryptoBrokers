import { AbstractEditAddPageInitializer } from "./../../interfaces/abstractEditAddPageInitializer"
import { NavigoRouter } from "../../../services/navigoRouter"

class EditBrokerPageInitializer extends AbstractEditAddPageInitializer {
  private _itemId: number;
  protected _urlToApiService: string = "/api/Brokers";
  protected _formTitleText = "Edit Broker";
  protected _saveApiUrl = "/UpdateBrokerAdmin";
  protected _formId: string = "add-broker-form";
  protected _templateApiUrl: string = "/GetAddBrokerTemplate";

  constructor(router: NavigoRouter
    , containerId: string
    , itemId: number) {
    super(router, containerId);
    this._itemId = +itemId;
  }

  async onStart() {
    let self = this;

    let loadingHtmlPromise = self._getHtmlTemplatePromise();

      let brokerData = null;
      console.log()
    let loadBrokerData = $.ajax(`${self._urlToApiService}/GetBrokerAdmin`, {
      method: "POST",
      data: {
        id: self._itemId
      }
    }).done(self._disposableListenerWrapper.bind(self, (data) => {
      brokerData = data;
    })).fail(() => {
      console.error(`${self._urlToApiService}/GetBrokerAdmin`);
    });

    await loadingHtmlPromise;
    await loadBrokerData;

    self._disposableListenerWrapper.call(self, () => {
      self._setFormValues(self._formId, brokerData);
      self._enableJQueryEvents();
      self._setNavigationDisabled(false);
    });
  }

  protected _getValuesFromForm(): Object {
    let formValues = super._getValuesFromForm();
    formValues["id"] = this._itemId;

    return formValues;
  }

  protected redirectAfterSuccess(): void {
    this._redirectTo("/admin/brokerList");
  }
}

export { EditBrokerPageInitializer }