import { AbstractPageInitializer } from "./../../../interfaces/irootbundle"
import { NavigoRouter } from "../../../services/navigoRouter";

class EditBrokerLocalizationPageInitializer extends AbstractPageInitializer {
  private _urlToApiService: string = "/api/Brokers";
  private _itemId: number;
  private _culture: string;

  private _cancelBtn: string = "cancel-btn";
  private _saveBtn: string = "save-btn";
  private _editBrokerLocalizationForm: string = "edit-broker-localization-form";

  constructor(router: NavigoRouter, containerId: string, itemId: number, args: any) {
    super(router, containerId);
    this._itemId = itemId;
    this._culture = args.culture;
  }

  async onStart() {
    let self = this;

    let loadingHtmlPromise = self._loadingHtmlPromiseUpdateContainer(
      self._urlToApiService + "/GetEditBrokerLocalizationTemplate");

    let localizationData = null;
    let loadLocalizationData = $.ajax(`${self._urlToApiService}/GetBrokerLocalizationData`, {
      method: "POST",
      data: {
        id: self._itemId,
        culture: self._culture
      }
    }).done(self._disposableListenerWrapper.bind(self, (data) => {
      localizationData = data;
    })).fail(() => {
      console.error(`${self._urlToApiService}/GetBrokerLocalizationData`);
    });

    await loadingHtmlPromise;
    await loadLocalizationData;

    self._disposableListenerWrapper(() => {
      self._setFormValues(self._editBrokerLocalizationForm, localizationData);
      self._enableJQueryEvents();
      self._setNavigationDisabled(false);
    });
  }

  dispose() {
    super.dispose();
    this._disableJQueryEvents();
  }

  private _enableJQueryEvents() {
    let self = this;
    $(`#${this._cancelBtn}`).on("click.cancel", function () {
      self._redirectTo("/admin/brokerList");
    });

    $(`#${this._saveBtn}`).on("click.save", function () {
      self._saveLocalizationEvent();
      return false;
    });
  }

  private _saveLocalizationEvent() {
    let self = this;
    let localizationForm = $(`#${self._editBrokerLocalizationForm}`).parsley();
    if (!localizationForm.validate()) return;

    self._setNavigationDisabled(true);
    let formValues = self._getSimpleFormValuesAsObject(self._editBrokerLocalizationForm);
    formValues["id"] = self._itemId;
    formValues["culture"] = self._culture;

    $.ajax(self._urlToApiService + "/UpdateBrokerLocalization", {
      method: "POST",
      contentType: "application/json",
      dataType: "json",
      data: JSON.stringify(formValues)
    }).done(self._disposableListenerWrapper.bind(self, (data) => {
      self._redirectTo("/admin/brokerList");
    })).fail((data) => {
      console.error(self._urlToApiService + "/UpdateBrokerLocalization");
    }).always(
      self._disposableListenerWrapper.bind(self, (data) => {
        self._setNavigationDisabled(false);
      }));
  }

  private _disableJQueryEvents() {
    $(`#${this._cancelBtn}`).off("click.cancel");
    $(`#${this._saveBtn}`).off("click.save");
  }

  private _setNavigationDisabled(flag: boolean) {
    $(`#${this._cancelBtn}`).prop("disabled", flag);
    $(`#${this._saveBtn}`).prop("disabled", flag);
  }
}

export { EditBrokerLocalizationPageInitializer }