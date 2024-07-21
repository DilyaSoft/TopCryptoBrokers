import { CalendarHelper } from "./calendarHelper"

class DayWeekCalendar extends CalendarHelper {
  constructor() {
    super();
    this._counter = 0;
  }

  _getFreshDate() {
    //week
    let lastDate = new Date();
    if (!lastDate.getDay()) {
      lastDate.setDate(lastDate.getDate() - 1);
    } else {
      lastDate.setDate(lastDate.getDate() + 7 - lastDate.getDay());
    }

    this._lastDate = lastDate;

    let startDate = new Date(lastDate);
    startDate.setDate(startDate.getDate() - 7);
    this._startDate = startDate;
  }

  getFormattedString() {
    this._getFreshDate();

    let copyOfStartDate = this.startDate;
    let startStr = copyOfStartDate.getDate() + " " + this.months[copyOfStartDate.getMonth()];

    let copyOfLastDate = this.lastDate;
    let lastStr = copyOfLastDate.getDate() + " " + this.months[copyOfLastDate.getMonth()];

    return startStr + " - " + lastStr;
  }

  apiValue() {
    return "day";
  }

  get lastDate(): Date {
    let copyOfLastDate = new Date(this._lastDate);
    copyOfLastDate.setDate(copyOfLastDate.getDate() + 7 * this._counter);

    return copyOfLastDate;
  }

  get startDate(): Date {
    let copyOfStartDate = new Date(this._startDate);
    copyOfStartDate.setDate(copyOfStartDate.getDate() + 7 * this._counter);

    return copyOfStartDate;
  }

  get visibilityOfCalendarTitle() {
    return true;
  }

  getLabelFromDateString(dateString: string): string {
    let date = new Date(dateString);
    return `${date.getDate()} ${this.months[date.getMonth()]}`;
  }
}

export { DayWeekCalendar }