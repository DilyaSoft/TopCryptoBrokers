import { AbstractTableInitializer } from "./../interfaces/AbstractTableInitializer"

class SettingsListTableInitializer extends AbstractTableInitializer {
  protected _getTBodyValue(val: Object): HTMLTableRowElement {
    let innerHtmlOfTr = `<td>${val["label"]}</td>`;

    innerHtmlOfTr += `<td>${val["query"] ? val["query"] : ''}</td>`;

    let tr = document.createElement("tr");
    tr.dataset["query"] = val["query"];
    tr.dataset["id"] = val["id"];
    tr.innerHTML = innerHtmlOfTr;
    return tr;
  }

  protected _getTrThFromDataObject(val: Object): string {
    if (!val) return "";
    return `<tr><th>Label</th><th>Culture</th></tr>`;
  }
}

export { SettingsListTableInitializer }