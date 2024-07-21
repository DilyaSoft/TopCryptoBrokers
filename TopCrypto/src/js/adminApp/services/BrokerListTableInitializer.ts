import { AbstractTableInitializer } from "./../interfaces/AbstractTableInitializer"

class BrokerListTableInitializer extends AbstractTableInitializer  {
  protected _getTBodyValue(val: Object): HTMLTableRowElement {
    let clone = Object.assign(val, {});

    let idItem = clone["ID"];
    let innerHtmlOfTr = `<td>${clone["ID"]}</td>`;
    delete (<any>clone).ID;

    innerHtmlOfTr += `<td><a href='/admin/edditBroker/${idItem}'>${clone["Name"]}</a></td>`;
    delete (<any>clone).Name;

    Object.keys(clone).forEach((item, i) => {
      if (i <= 0) {
        innerHtmlOfTr += `<td>${clone[item]}</td>`;
      } else {
        let css = '';
        if (!clone[item]) {
          css = "btn-danger";
        } else {
          css = "btn-success";
        }
        innerHtmlOfTr += `<td><button type="button" data-id-item=${idItem} data-culture="${item}" class="btn ${css} btn-xs">Edit</button></td>`;
      }
    });

    innerHtmlOfTr += `<td><a class="remove-broker" data-id-item=${idItem}><i class="fas fa-trash fa-2x"></i></a></td>`

    let tr = document.createElement("tr");
    tr.innerHTML = innerHtmlOfTr;
    return tr;
  }
}

export { BrokerListTableInitializer }