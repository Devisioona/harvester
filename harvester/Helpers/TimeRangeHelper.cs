using System;

namespace Devisioona.Harvest.CLI.Helpers {
	public static class TimeRangeHelper {
		public static void GetTimerangeFromString(string timerange, out DateTime from, out DateTime until) {
			switch (timerange) {
				case "today": {
						until = DateTime.Today;
						from = DateTime.Today;
					}
					break;
				case "last7days": {
						until = DateTime.Today;
						from = until.AddDays(-7);
					}
					break;
				case "last30days": {
						until = DateTime.Today;
						from = until.AddDays(-30);
					}
					break;
				case "yesterday": {
						from = DateTime.Today.AddDays(-1);
						until = DateTime.Today.AddDays(-1);
					}
					break;
				case "thisweek": {
						var today = (int)DateTime.Today.DayOfWeek;
						if (today == 0) today = 7;
						var monday = today - 1;

						from = DateTime.Today.AddDays(-monday);
						until = DateTime.Today.AddDays(-monday + 6);
					}
					break;
				case "lastweek": {
						var today = (int)DateTime.Today.DayOfWeek;
						if (today == 0) today = 7;
						var monday = today - 1;

						from = DateTime.Today.AddDays(-monday - 7);
						until = DateTime.Today.AddDays(-monday - 1);
					}
					break;
				case "thismonth": {
						var thismonth = DateTime.Today;
						from = new DateTime(thismonth.Year, thismonth.Month, 1);
						until = new DateTime(thismonth.Year, thismonth.Month, DateTime.DaysInMonth(thismonth.Year, thismonth.Month));
					}
					break;
				case "lastmonth": {
						var lastmonth = DateTime.Today.AddMonths(-1);
						from = new DateTime(lastmonth.Year, lastmonth.Month, 1);
						until = new DateTime(lastmonth.Year, lastmonth.Month, DateTime.DaysInMonth(lastmonth.Year, lastmonth.Month));
					}
					break;
				case "thisyear": {
						var thisyear = DateTime.Today;
						from = new DateTime(thisyear.Year, 1, 1);
						until = thisyear;
					}
					break;
				default:
					throw new NotImplementedException();
			}

		}
	}
}
