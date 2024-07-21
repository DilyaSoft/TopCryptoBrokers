import { AbstractEditAddPageInitializer } from "./../../interfaces/abstractEditAddPageInitializer"

class AddNewsPageInitializer extends AbstractEditAddPageInitializer {
  protected _urlToApiService: string = "/api/News";
  protected _formTitleText = "Add News";
  protected _saveApiUrl = "/AddNews";
  protected _formId: string = "add-news-form";
  protected _templateApiUrl: string = "/GetAddEditTemplate";

  async onStart() {
    let self = this;

    await self._getHtmlTemplatePromise();

    self._disposableListenerWrapper.call(self, () => {
      this._enableJQueryEvents();
      self._setNavigationDisabled(false);
    });
  }

  protected redirectAfterSuccess(): void {
    this._redirectTo("/admin/newsList");
  }
}

export { AddNewsPageInitializer }