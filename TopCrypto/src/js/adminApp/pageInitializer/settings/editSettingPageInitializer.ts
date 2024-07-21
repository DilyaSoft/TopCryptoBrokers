import { AbstractPageInitializer } from "./../../../interfaces/irootbundle"
import { AbstractEditAddPageInitializer } from "./../../interfaces/abstractEditAddPageInitializer"
import { NavigoRouter } from "../../../services/navigoRouter";

class EditSettingPageInitializer extends AbstractEditAddPageInitializer {
  protected _urlToApiService: string = "/api/Settings";
  protected _formId: string = "add-setting-form";
  protected _formTitleText = "Edit Setting";
  protected _saveApiUrl = "/UpdateSettingByIdQuery";
  protected _templateApiUrl: string = "/GetSettingTemplate";

  private _itemId: string;
  private _query: string;

  constructor(router: NavigoRouter, containerId: string, itemId: number, args: any) {
    super(router, containerId);
    this._itemId = args.idStr;
    this._query = args.query;
  }

  async onStart() {
    let self = this;
    let settingDTO = null;

    let loadingHtmlPromise = self._getHtmlTemplatePromise();
    let loadData = $.ajax(`${self._urlToApiService}/GetSettingByIdQuery`, {
      method: "POST",
      contentType: "application/json",
      dataType: "json",
      data: JSON.stringify({
        id: self._itemId,
        query: self._query
      })
    }).done(self._disposableListenerWrapper.bind(self, (data) => {
      settingDTO = data;
    })).fail(() => {
      console.error(`${self._urlToApiService}/GetSettingByIdQuery`);
    });

    await loadingHtmlPromise;
    await loadData;

    self._disposableListenerWrapper.call(self, () => {
      $(`#${self._containerId} #commonSetting`).html(`Id: ${self._itemId}&nbsp;&nbsp;&nbsp;query: ${self._query}`);

      if (settingDTO) {
        self._setFormValues(self._formId, { value: settingDTO.value });
      }
      self._enableJQueryEvents();
      self._setNavigationDisabled(false);
    });
  }

  protected _getValuesFromForm(): Object {
    let formValues = super._getValuesFromForm();
    formValues["id"] = this._itemId;
    formValues["query"] = this._query;

    return formValues;
  }

  protected redirectAfterSuccess(): void {
    if (!~this._itemId.indexOf("link_")) {
      this._redirectTo("settingsTemplatesUrls");
    } else {
      this._redirectTo("settingsTitles");
    }
  }
}

export { EditSettingPageInitializer }