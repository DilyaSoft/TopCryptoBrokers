import { AbstractPageInitializer } from "./../../interfaces/irootbundle"

abstract class AbstractEditAddPageInitializer extends AbstractPageInitializer {
  protected _cancelBtn: string = "cancel-btn";
  protected _saveBtn: string = "save-btn";
  protected _formTitleId: string = "form-theme";
  protected abstract _urlToApiService: string;
  protected abstract _formId: string;
  protected abstract _formTitleText: string;
  protected abstract _saveApiUrl: string;
  protected abstract _templateApiUrl: string;

  protected abstract redirectAfterSuccess(): void;

  dispose() {
    super.dispose();
    this._disableJQueryEvents();
  }

  protected _getHtmlTemplatePromise(): JQuery.Promise<any> {
    let self = this;

    return self._loadingHtmlPromiseUpdateContainer(self._urlToApiService + self._templateApiUrl)
      .then(self._disposableListenerWrapper.bind(self, () => {
        let formThene = document.getElementById(this._formTitleId);
        formThene.innerHTML = self._formTitleText + formThene.innerHTML;
      }));
  }

  protected _disableJQueryEvents() {
    let self = this;
    $(`#${self._cancelBtn}`).off(".editAddPage");
    $(`#${self._saveBtn}`).off(".editAddPage");
  }

  protected _enableJQueryEvents() {
    let self = this;
    $(`#${self._cancelBtn}`).on("click.editAddPage", self.redirectAfterSuccess.bind(self));
    $(`#${self._saveBtn}`).on("click.editAddPage", function () {
      self._saveEvent();
      return false;
    });
  }

  protected _setNavigationDisabled(flag: boolean) {
    let self = this;
    $(`#${self._cancelBtn}`).prop("disabled", flag);
    $(`#${self._saveBtn}`).prop("disabled", flag);
  }

  protected _getValuesFromForm(): Object {
    return this._getSimpleFormValuesAsObject(this._formId);
  }

  protected _saveEvent() {
    let self = this;
    let addAddEditform = $(`#${self._formId}`).parsley();
    if (!addAddEditform.validate()) return false;

    self._setNavigationDisabled(true);
    let formValues = self._getValuesFromForm();

    $.ajax(self._urlToApiService + self._saveApiUrl, {
      method: "POST",
      contentType: "application/json",
      dataType: "json",
      data: JSON.stringify(formValues)
    }).done(self._disposableListenerWrapper.bind(self, self.redirectAfterSuccess.bind(self)))
      .fail(() => {
        console.error(self._urlToApiService + self._saveApiUrl);
      }).always(
      self._disposableListenerWrapper.bind(self, (data) => {
        self._setNavigationDisabled(false);
      }));
  }
}

export { AbstractEditAddPageInitializer }