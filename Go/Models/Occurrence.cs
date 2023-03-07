using System;
using Go.Common;

namespace Go.Models
{
    public class Occurrence : Observable
    {
        private bool _IsFavorite;

        public long Id { get; set; }
        public bool Active { get; set; }
        public DateTime? DatetimeStart { get; set; }
        public DateTime? DatetimeEnd { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string Place { get; set; }
        public Category Category { get; set; }
        public Location Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Views { get; set; }
        public int Openings { get; set; }
        public bool IsAd { get; set; }
        public DateTime? AdTo { get; set; }

        public bool WasSeen { get; set; }
        public bool WasOpened { get; set; }

        public bool IsFavorite
        {
            get => _IsFavorite;
            set => SetValue(ref _IsFavorite, value);
        }

        public bool HasDate
        {
            get
            {
                if(DatetimeStart.HasValue)
                {
                    return true;
                }

                return false;
            }
        }

        public Tuple<bool, string> Validate(bool validateDate = true, bool isSameDay = false)
        {
            if(string.IsNullOrEmpty(Title))
            {
                return new Tuple<bool, string>(false, "Debes indicarnos el titulo");
            }

            if (string.IsNullOrEmpty(Description))
            {
                return new Tuple<bool, string>(false, "Debes indicarnos la descripción");
            }

            if (string.IsNullOrEmpty(Street))
            {
                return new Tuple<bool, string>(false, "Debes indicarnos la calle");
            }

            if (string.IsNullOrEmpty(StreetNumber))
            {
                return new Tuple<bool, string>(false, "Debes indicarnos la altura de la calle");
            }

            if (string.IsNullOrEmpty(Place))
            {
                return new Tuple<bool, string>(false, "Debes indicarnos el lugar donde se hará");
            }

            if(validateDate)
            {
                if (DatetimeStart.HasValue)
                {
                    if (DatetimeStart <= DateTime.Now)
                    {
                        return new Tuple<bool, string>(false, "La fecha de inicio debe ser mayor a hoy");
                    }
                }

                if(!isSameDay)
                {
                    if (DatetimeEnd.HasValue)
                    {
                        if (DatetimeEnd <= DateTime.Now)
                        {
                            return new Tuple<bool, string>(false, "La fecha de fin debe ser mayor a hoy");
                        }
                        else if (DatetimeEnd < DatetimeStart)
                        {
                            return new Tuple<bool, string>(false, "La fecha de fin debe ser luego de la fecha de inicio");
                        }
                    }
                }
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}