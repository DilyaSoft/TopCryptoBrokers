import { AbstractPageInitializer } from "./../../interfaces/irootbundle";
import { PriceService, PriceDTO } from "./../services/priceService";
import { PriceGraphHelper } from "./../services/priceGraphHelper";
import { NavigoRouter } from "../../services/navigoRouter";

import { CalendarHelper } from "../services/priceGraphCalendar/calendarHelper";
import { MonthCalendar } from "../services/priceGraphCalendar/monthCalendar";
import { DayWeekCalendar } from "../services/priceGraphCalendar/dayWeekCalendar";
import { YearCalendar } from "../services/priceGraphCalendar/yearCalendar";

class PriceGraphInitializer extends AbstractPageInitializer {
  private _countOfVisibleNodes: number = 10;
  private _lastSelectedContentId;
  private _calendarIns: CalendarHelper;

  constructor(route: NavigoRouter
    , private containerId
    , private priceGraphHelper: PriceGraphHelper) {
    super(route, containerId);
  }

  onStart() {
    let self = this;

    let startFunction = () => {
      $(`#${self.containerId}`).on("click.priceGraphInitializer", ".graph-click-handler"
        , async (event) => {
          if (event.target.matches(".trade-now-handler") || event.target.matches(".trade-now-handler *")) {
            return;
          }
          await self.showGraphs.call(self, $(event.currentTarget).find("a"));
        });

      $(`#${self.containerId}`).on("click.priceGraphInitializer"
        , "#content1 .calendar-link-value",
        async (event) => { await self.changeCalendarValue.call(self, $(event.target)); });

      $(`#${self.containerId}`).on("click.priceGraphInitializer", "#content1 #price-calendar-back", async () => {
        self._calendarIns && self._calendarIns.decreaseCounter();
        self.redrawView();
        await self.updateGraphOnSelectedCoin();
      });

      $(`#${self.containerId}`).on("click.priceGraphInitializer", "#content1 #price-calendar-forward", async (e) => {
        self._calendarIns && self._calendarIns.increaseCounter();
        self.redrawView();
        await self.updateGraphOnSelectedCoin();
      });

      $(window).on("resize.priceGraphInitializer", () => {
        self.resizeView();
      });
      $(window).on("click.priceGraphInitializer", (e) => {
        self.clearGraphsIfClickOuter(e.target);
      });
    }
    self._disposableListenerWrapper.call(self, startFunction);
  };

  resizeView() {
    let self = this;
    let ct1 = document.getElementById("content1");

    if (!ct1) return;

    let tableWrap = $(`#${self.containerId} #table-wrap`);

    let boundingRect = ct1.getBoundingClientRect();
    let left = boundingRect.left;
    let right = boundingRect.right - boundingRect.width;

    tableWrap.find(".red-border-of-selected-item-left")
      .attr("style", `width:${left}px !important;left:-${left}px !important`);
    tableWrap.find(".red-border-of-selected-item-right")
      .attr("style", `width:${right}px !important;right:-${right}px !important`);

    //Fix In IE
    let firstLeftRedBorder = tableWrap.find(".red-border-of-selected-item-left:first");
    let outerHeight = firstLeftRedBorder.closest("tr").outerHeight();
    ++outerHeight; //IE round problem
    firstLeftRedBorder.attr("style", `width:${left}px !important;left:-${left}px !important; height:${outerHeight}px`);

    let firstRightRedBorder = tableWrap.find(".red-border-of-selected-item-right:first");
    firstRightRedBorder.attr("style", `width:${right}px !important;right:-${right}px !important; height:${outerHeight}px`);

    let tdWithSpan = tableWrap.find("table tr.table-striped-custom#content1 td:first");
    let collNum = window.matchMedia('(max-width: 992px)').matches ? 4 : 7;
    tdWithSpan.attr("colspan", collNum);
  }

  private _idOfApiCall = 0;
  async updateGraphOnSelectedCoin() {
    let self = this;
    self._idOfApiCall++;
    let lastId = self._idOfApiCall;
    //week
    let promisePrice = <[{ price_close: number, time_period_start: string }]>
      await self.getCoins(self._lastSelectedContentId
        , self._calendarIns.apiValue()
        , self._calendarIns.startDate
        , self._calendarIns.lastDate);

    if (self._idOfApiCall != lastId) { return; }
    self._disposableListenerWrapper(() => {
      let values = [];
      let labels = [];
      promisePrice.forEach((item) => {
        values.push(item.price_close);
        labels.push(self._calendarIns.getLabelFromDateString(item.time_period_start));
      });

      let canvasElement = <HTMLCanvasElement>$(`#${self.containerId} #content1 #grapthImage`)[0];
      if (values && values.length) {
        self.priceGraphHelper.drawGraphic(canvasElement
          , values
          , 1085
          , 297
          , labels
          , 1
          , "#ffffff");
      } else {
        self.priceGraphHelper.drawNoData(canvasElement);
      }
    });
  }

  private async changeCalendarValue(jTarget) {
    switch (jTarget.data("value")) {
      case "month": {
        this._calendarIns = new MonthCalendar();
        this.changeVisibilityOfCalendarTitle(true);
        break;
      }
      case "day": {
        this._calendarIns = new DayWeekCalendar();
        this.changeVisibilityOfCalendarTitle(true);
        break;
      }
      case "year": {
        this._calendarIns = new YearCalendar();
        this.changeVisibilityOfCalendarTitle(false);
        break;
      }
    }
    this.redrawView();
    await this.updateGraphOnSelectedCoin();
  }

  changeVisibilityOfCalendarTitle(flag: boolean) {
    let self = this;

    let content1 = $(`#${self.containerId} #content1`);
    let action = flag ? "show" : "hide";

    content1.find(`#price-calendar-forward`)[action]();
    content1.find(`#price-calendar-back`)[action]();
    content1.find(`#price-calendar-title`)[action]();
  }

  redrawView() {
    let self = this;
    if (!self._calendarIns) return;

    let content1 = $(`#${self.containerId} #content1`);

    content1.find(`#price-calendar-title`).text(self._calendarIns.getFormattedString());
    if (self._calendarIns.lastDate > new Date()) {
      content1.find(`#price-calendar-forward`).css("visibility", "hidden");
    } else {
      content1.find(`#price-calendar-forward`).css("visibility", "visible");
    }
  }

  private async clearGraphsIfClickOuter(jTarget) {
    let self = this;
    if ($(jTarget).is(`#${self.containerId} #table-wrap tr`)
      || $(jTarget).is(`#${self.containerId} #table-wrap tr *`)) {
      return;
    } else {
      this.clearSelectedGraph($(`#${self.containerId} #table-wrap a[selected-content=1]`));
    }
  }

  private async showGraphs(jTarget) {
    if (!jTarget.data("href")) return;

    if ($('tr#' + jTarget.data("href")).is(":visible")) {
      let selectedEl = $(`#${this.containerId} a[selected-content=1][data-href=${jTarget.data("href")}]`);
      if (jTarget.is(selectedEl)) {
        this.clearSelectedGraph(jTarget);
      } else {
        this.clearSelectedGraph(jTarget);
        this._calendarIns = new DayWeekCalendar();
        await this.drawSelection(jTarget);
      }
    } else {
      this._calendarIns = new DayWeekCalendar();
      await this.drawSelection(jTarget);
    }
  }

  private async drawSelection(jTarget) {
    let closestTr = jTarget.closest('tr');
    jTarget.attr("selected-content", 1);
    closestTr.after('<tr id="' + jTarget.data("href") + '"class="table-striped-custom"><td>'
      + $('#' + jTarget.data("href")).html() + '</td></tr>');

    closestTr.find('td.hide-expanded').css('opacity', 0);
    closestTr.css('background', '#f70600').addClass("table-striped-custom");
    closestTr.children('td,th').css('background-color', '#f70600').css('color', '#fff');

    closestTr.find("td:first").append('<div class="red-border-of-selected-item-left"></div>');
    closestTr.find("td:last").prepend('<div class="red-border-of-selected-item-right"></div>');

    this._lastSelectedContentId = jTarget.attr("contentId");

    this.redrawView();
    this.resizeView();
    await this.updateGraphOnSelectedCoin();
  }

  private clearSelectedGraph(jTarget: JQuery<this> | JQuery<HTMLElement>) {
    let aSelCont = $(`#${this.containerId} a[selected-content=1][data-href=${jTarget.data("href")}]`);

    $(`#${this.containerId} tr#${aSelCont.data("href")}`).remove();
    let closestTr = aSelCont.closest('tr');
    closestTr.css('background', '').children('td,th').css('background-color', '#fff').css('color', '');
    closestTr.find('td.hide-expanded').css('opacity', 1);
    closestTr.removeClass("table-striped-custom");
    closestTr.find(".red-border-of-selected-item-left, .red-border-of-selected-item-right").remove();

    aSelCont.attr("selected-content", null);
    this._lastSelectedContentId = null;
  }

  async reselectLastSelectedContentId() {
    if (!this._lastSelectedContentId) return;

    let selectedEl = $(`#${this.containerId} a[contentid=${this._lastSelectedContentId}]`);
    await this.drawSelection(selectedEl);
  }

  dispose() {
    super.dispose();
    $(`#${this.containerId}`).off(".priceGraphInitializer");
    $(window).off(".priceGraphInitializer");
  }

  async getCoins(coinId: string, timeInterval: string, startDate: Date, endDate: Date) {
    let self = this;
    return await $.ajax({
      method: "POST",
      url: "api/CoinGraph/GetCoins",
      data: {
        coinId: coinId
        , timeInterval: timeInterval
        , startDate: startDate ? self.dateToISOString(startDate) : startDate
        , endDate: endDate ? self.dateToISOString(endDate) : endDate
      }
    });
  }

  private dateToISOString(dt: Date) {
    return new Date(Date.UTC(dt.getFullYear(), dt.getMonth(), dt.getDate(), 0, 0, 0)).toISOString()
  }
}

export { PriceGraphInitializer };