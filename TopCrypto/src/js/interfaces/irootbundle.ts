import { NavigoRouter } from "../services/navigoRouter";

interface IDisposable {
  dispose(): void;
}

interface IStart {
  onStart(): void | Promise<any>;
}

interface ITemplate extends IStart, IDisposable {
}

abstract class AbstractPageInitializer implements ITemplate, IDisposable, IStart {
  protected _isDisposed = false;
  protected _containerId: string;
  protected _route: NavigoRouter;

  constructor(router: NavigoRouter, containerId: string) {
    this._containerId = containerId || "page";
    this._route = router;
  }

  abstract onStart(): void;

  public dispose() {
    this._isDisposed = true;
  }

  protected _loadingHtmlPromiseUpdateContainer(url: string): JQuery.Promise<any> {
    let self = this;
    return $.get({
      url: url,
      cache: true,
      dataType: "html"
    }).done(
      self._disposableListenerWrapper.bind(self,
        (data) => {
          document.getElementById(self._containerId).innerHTML = data;
        }
      )).fail(() => {
        console.error(url);
      });
  }

  protected _koApllyBindings(bindingModel, containerId) {
    if (this._isDisposed) return;
    ko.applyBindings(bindingModel, document.getElementById(containerId));
  }

  protected _doAfterPageChanged() {
    $(window).resize(); //call handler in root initializer
  }

  protected _disposableListenerWrapper(fn: Function) {
    if (this._isDisposed) return;

    let arr = Array.prototype.slice.call(arguments, 1);
    return fn.apply(this, arr);
  }

  protected _getDecodedParameter(paramName: string) {
    let searchString = window.location.search.substring(1),
      i, val, params = searchString.split("&");

    for (i = 0; i < params.length; i++) {
      val = params[i].split("=");
      if (val[0] == paramName) {
        return decodeURIComponent(val[1]);
      }
    }
    return null;
  }

  protected _getSimpleFormValuesAsObject(formId: string): object {
    let arr = $(`#${formId}`).serializeArray();
    let obj = {};
    arr.forEach((item) => {
      obj[item.name] = item.value;
    });
    return obj;
  }

  protected _redirectTo(anchor: string) {
    this._route.navigate(anchor);
  }

  protected _setFormValues(id: string, obj: Object) {
    if (!obj || !id) return;
    var self = this;

    let form = $(`#${id}`);
    let keys = Object.keys(obj);
    for (let i = 0; i < keys.length; i++) {
      let input = form.find(`input[name="${keys[i]}"]`);
      if (input.length) {
        let type = input.attr("type").toLowerCase();

        switch (type) {
          case 'text':
          case 'number':
          case 'datetime-local':
            input.val(obj[keys[i]]);
            break;
          case 'checkbox':
            input.prop('checked', obj[keys[i]]);
            break;
        }
        continue;
      }

      let textArea = form.find(`textarea[name="${keys[i]}"]`);
      if (textArea && textArea.length) {
        textArea.val(obj[keys[i]]);

        for (let j = 0; j < textArea.length; j++) {
          self.textAreaAdjust(textArea[j]);
        }

        continue;
      }
      console.error(`Not found input for ${keys[i]}`);
    }
  }

  protected textAreaAdjust(o: HTMLElement) {
    let computedStyles = getComputedStyle(o);

    let topPad = parseInt(computedStyles.paddingTop);
    let topPadVal = isNaN(topPad) ? 0 : topPad;

    let botPad = parseInt(computedStyles.paddingBottom);
    let botPadVal = isNaN(botPad) ? 0 : botPad;

    o.style.height = o.scrollHeight + topPadVal + botPad + "px";
  }

  protected _random999999() {
    return Math.random() * 999999;
  }
}

export { IDisposable, IStart, ITemplate, AbstractPageInitializer }