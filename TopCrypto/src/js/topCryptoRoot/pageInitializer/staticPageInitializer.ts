import { AbstractPageInitializer } from "./../../interfaces/irootbundle";
import { NavigoRouter } from "../../services/navigoRouter";

class StaticPageInitializer extends AbstractPageInitializer {
  private _templateId: string;
  constructor(router: NavigoRouter, containerId: string, itemId: number, args: any) {
    super(router, containerId);
    this._templateId = args.templateId;
  }

  async onStart() {
    let self = this;
    let url = "/api/Template/GetStaticTemplate";

    await $.get({
      url: url,
      cache: true,
      dataType: "html",
      data: { id: self._templateId }
    }).done(
      self._disposableListenerWrapper.bind(self, (data) => {
        document.getElementById(self._containerId).innerHTML = data;
      })
      ).fail(() => {
        console.error(url);
      });

    self._route._initializeTitle(`static_page_title_${self._templateId}`);
  }
}

export { StaticPageInitializer }