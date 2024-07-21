import { AbstractEditAddPageInitializer } from "./../../interfaces/abstractEditAddPageInitializer"
import { NavigoRouter } from "../../../services/navigoRouter"

class EditNewsPageInitializer extends AbstractEditAddPageInitializer {
  private _itemId: number;
  protected _urlToApiService: string = "/api/News";
  protected _formTitleText = "Edit News";
  protected _saveApiUrl = "/UpdateNewsAdmin";
  protected _formId: string = "add-news-form";
  protected _templateApiUrl: string = "/GetAddEditTemplate";

  constructor(router: NavigoRouter
    , containerId: string
    , itemId: number) {
    super(router, containerId);
    this._itemId = +itemId;
  }

  async onStart() {
    let self = this;

    let loadingHtmlPromise = self._getHtmlTemplatePromise();

    let newsDto = null;
    let loadNewsDto = $.ajax(`${self._urlToApiService}/GetNewsAdminDTO`, {
      method: "POST",
      data: {
        id: self._itemId
      }
    }).done(self._disposableListenerWrapper.bind(self, (data) => {
      newsDto = data;
    })).fail(() => {
      console.error(`${self._urlToApiService}/GetNewsAdminDTO`);
    });

    await loadingHtmlPromise;
    await loadNewsDto;

    self._disposableListenerWrapper.call(self, () => {
      self._setFormValues(self._formId, newsDto);
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
    this._redirectTo("/admin/newsList");
  }
}

export { EditNewsPageInitializer }