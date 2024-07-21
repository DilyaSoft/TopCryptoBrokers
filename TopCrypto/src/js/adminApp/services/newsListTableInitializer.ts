import { AbstractTableInitializer } from "./../interfaces/AbstractTableInitializer"

class NewsListTableInitializer extends AbstractTableInitializer {
  protected _getTBodyValue(val: Object): HTMLTableRowElement {
    let clone = Object.assign(val, {});

    let idItem = clone["ID"];
    let innerHtmlOfTr = `<td>${clone["ID"]}</td>`;
    delete (<any>clone).ID;

    innerHtmlOfTr += `<td><a href='/admin/editNews/${idItem}'>${clone["Note"] ? clone["Note"] : "Empty Note"}</a></td>`;
    delete (<any>clone).Note;

    Object.keys(clone).forEach((item, i) => {
      let css = '';
      if (clone[item]) {
        css = "btn-danger";
      } else {
        css = "btn-success";
      }
      innerHtmlOfTr += `<td><button type="button" data-id-item=${idItem} data-culture="${item}" class="btn ${css} btn-xs">Edit</button></td>`;
    });

    let tr = document.createElement("tr");
    tr.innerHTML = innerHtmlOfTr;
    return tr;
  }

}

export { NewsListTableInitializer }