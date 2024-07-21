abstract class AbstractTableInitializer {
  protected _tableId: string;
  constructor(tableId) {
    this._tableId = tableId;
  }

  protected abstract _getTBodyValue(val: Object): HTMLTableRowElement;

  initializeTable(data: Array<Object>) {
    let table = <HTMLTableElement>document.getElementById(this._tableId);

    this._updateTableHeaders(table, data);
    this._updateTableBody(table, data);
  }

  protected _updateTableBody(table: HTMLTableElement, data: Object[]) {
    if (!table) return;
    let fragment = document.createDocumentFragment();
    for (let i = 0; i < data.length; i++) {
      fragment.appendChild(this._getTBodyValue(data[i]));
    }
    table.tBodies[0].innerHTML = "";
    table.tBodies[0].appendChild(fragment);
  }

  protected _updateTableHeaders(table: HTMLTableElement, data: Object[]) {
    if (!table) return;
    table.tHead.innerHTML = this._getTrThFromDataObject(data[0]);
  }

  protected _getTrThFromDataObject(val: Object): string {
    if (!val) return "";

    let tr = "";
    Object.keys(val).forEach((item) => { tr += `<th>${item}</th>`; });

    tr += "<th></th>";
    return `<tr>${tr}</tr>`
  }
}

export { AbstractTableInitializer }