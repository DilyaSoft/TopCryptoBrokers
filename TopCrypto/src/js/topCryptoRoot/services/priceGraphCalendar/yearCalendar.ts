import { CalendarHelper } from "./calendarHelper"

class YearCalendar extends CalendarHelper {
  constructor() {
    super();
    this._counter = 0;
    this._startDate = null;
    this._lastDate = null;
  }

  getFormattedString() {
    return "";
  }

  apiValue() {
    return "year";
  }

  get lastDate(): Date {
    return null;
  }

  get startDate(): Date {
    return null;
  }

  get visibilityOfCalendarTitle() {
    return false;
  }

  getLabelFromDateString(dateString: string): string {
    return new Date(dateString).getFullYear().toString();
  }
}

export { YearCalendar }