using System;
using Go.Common;

namespace Go.Models
{
    public class TimeRange : Observable
    {
        private bool _IsSelected;

        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsSelected
        {
            get => _IsSelected;
            set => SetValue(ref _IsSelected, value);
        }

        public Tuple<DateTime?, DateTime?> Dates()
        {
            int dayToRest;
            int dayToAdd;
            DateTime? Start = null;
            DateTime? End = null;

            switch(Id)
            {
                case (int)TimeRanges.All:
                    Start = null;
                    End = null;

                    break;

                case (int)TimeRanges.Today:
                    Start = DateTime.Now;
                    End = DateTime.Now;

                    break;

                case (int)TimeRanges.Tomorrow:
                    Start = DateTime.Now.AddDays(1);
                    End = DateTime.Now.AddDays(1);

                    break;

                case (int)TimeRanges.ThisWeek:
                    dayToRest = DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 6 : ((int)DateTime.Now.DayOfWeek) - 1;

                    Start = DateTime.Now.AddDays(-dayToRest);
                    End = DateTime.Now.AddDays(6 - dayToRest);

                    break;

                case (int)TimeRanges.ThisWeekend: 
                    dayToAdd = DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? -1 : 6 - ((int)DateTime.Now.DayOfWeek);

                    Start = DateTime.Now.AddDays(dayToAdd);
                    End = Start.Value.AddDays(1);

                    break;

                case (int)TimeRanges.NextWeek:
                    dayToAdd = DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 1 : 8 - ((int)DateTime.Now.DayOfWeek);

                    Start = DateTime.Now.AddDays(dayToAdd);
                    End = Start.Value.AddDays(6);

                    break;

                case (int)TimeRanges.ThisMonth:
                    Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                    break;
            }

            return new Tuple<DateTime?, DateTime?>(Start, End);
        }
    }
}