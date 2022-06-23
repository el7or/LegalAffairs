import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})
export class DateTimeService {
  constructor() {}

  // =====> to send date without time:
  dateWithoutTime(date: Date) {
    return (
      new Date(date).getFullYear() +
      "-" +
      ("0" + (new Date(date).getMonth() + 1)).slice(-2) +
      "-" +
      ("0" + new Date(date).getDate()).slice(-2)
    );
  }

  // =====> to merge date with time
  mergDateTime(date: Date, time: Date) {
    const dateTime = new Date(
      date.getFullYear(),
      date.getMonth(),
      date.getDate(),
      time.getHours(),
      time.getMinutes()
    );
    return this.clearTime(dateTime);
  }

  // =====> to clear time from timezone:
  clearTime(d: Date) {
    d = new Date(d);
    const timeZoneDifference = (d.getTimezoneOffset() / 60) * -1;
    d.setTime(d.getTime() + timeZoneDifference * 60 * 60 * 1000);
    return d.toISOString();
  }
}
