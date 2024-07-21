import { CalendarHelper } from "./calendarHelper";

class MonthCalendar extends CalendarHelper {
  constructor() {
    super();
    this._counter = 0;
  }

  getFormattedString() {
    this._getFreshDate();
    return this.startDate.getFullYear().toString();
  }

  apiValue() {
    return "month";
  }

  get lastDate(): Date {
    let copyOfStartDate = new Date(this._startDate);
    copyOfStartDate.setFullYear(copyOfStartDate.getFullYear() + this._counter + 1);

    return copyOfStartDate;
  }

  get startDate(): Date {
    let copyOfStartDate = new Date(this._startDate);
    copyOfStartDate.setFullYear(copyOfStartDate.getFullYear() + this._counter);

    return copyOfStartDate;
  }

  get visibilityOfCalendarTitle() {
    return true;
  }

  getLabelFromDateString(dateString: string): string {
    return `${this.months[new Date(dateString).getMonth()]}`;
  }
}

export { MonthCalendar }