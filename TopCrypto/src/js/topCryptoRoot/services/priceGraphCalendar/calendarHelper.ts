abstract class CalendarHelper {
  protected _counter: number;
  protected _startDate: Date;
  protected _lastDate: Date;

  //toLocaleDateString
  protected months = (<any>window).monthsShort;

  increaseCounter() {
    this._counter++;
  }

  decreaseCounter() {
    this._counter--;
  }

  protected _getFreshDate() {
    this._startDate = new Date();
  }

  abstract get lastDate(): Date;
  abstract get startDate(): Date;
  abstract get visibilityOfCalendarTitle(): boolean;
  abstract getFormattedString(): string;
  abstract apiValue(): string;
  abstract getLabelFromDateString(dateString: string): string;
}

export { CalendarHelper };