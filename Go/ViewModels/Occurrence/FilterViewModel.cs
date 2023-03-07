using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Go.Common;
using Go.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class FilterViewModel : ViewModelBase
    {
        //Cached data
        public static List<TimeRange> _cachedRanges;
        public static List<Category> _cachedCategories;

        private Action<TimeRange, List<Category>> ApplyFiltersCallback;

        private List<TimeRange> _TimeRanges;
        private List<Category> _Categories;
        private TimeRange _TimeRange;
        private bool _HideWhen;

        public List<TimeRange> TimeRanges
        {
            get => _TimeRanges;
            set => SetValue(ref _TimeRanges, value);
        }

        public List<Category> Categories
        {
            get => _Categories;
            set => SetValue(ref _Categories, value);
        }

        public TimeRange TimeRange
        {
            get => _TimeRange;
            set
            {
                _TimeRange = value;
                OnPropertyChanged(nameof(TimeRange));

                if (value is null)
                    return;

                TimeRanges.ForEach(x => x.IsSelected = false);
                value.IsSelected = true;
            }
        }

        public bool HideWhen
        {
            get => _HideWhen;
            set => SetValue(ref _HideWhen, value);
        }

        public ICommand ChangeLocationCommand { get; private set; }
        public ICommand ApplyFilterCommand { get; private set; }

        public FilterViewModel(Action<TimeRange, List<Category>> ApplyFiltersCallback, bool HideWhen = false)
        {
            this.HideWhen = HideWhen;
            this.ApplyFiltersCallback = ApplyFiltersCallback;

            TimeRanges = _cachedRanges ?? Utils.BuildRanges();
            Categories = _cachedCategories ?? Utils.BuildCategories();

            ChangeLocationCommand = new Command(ChangeLocation);
            ApplyFilterCommand = new Command(ApplyFilter);
        }

        private void ChangeLocation()
        {
            NavigationModalTo(new NavigationPage(new Views.LocationsPage(LocationPicked))
            {
                BarBackgroundColor = Color.White
            });
        }

        private void LocationPicked(Location Location)
        {
            Xamarin.Essentials.Preferences.Set(Constants.LocationKey, JsonConvert.SerializeObject(Location));
            OnPropertyChanged(nameof(Location));
        }

        private void ApplyFilter()
        {
            _cachedRanges = TimeRanges;
            _cachedCategories = Categories;

            ApplyFiltersCallback?.Invoke(TimeRange, Categories?.Where(x => x.IsSelected).ToList());
            PopModalNavigate();
        }
    }
}