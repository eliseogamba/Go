using System;
using System.Collections.Generic;
using System.Windows.Input;
using Go.Models;
using Go.Services;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class LocationsViewModel : ViewModelBase
    {
        private Action<Location> _locationPicked;

        private string _Text;
        private bool _IsLoading;
        private List<Location> _Locations;

        public string Text
        {
            get => _Text;
            set => SetValue(ref _Text, value);
        }

        public bool IsLoading
        {
            get => _IsLoading;
            set => SetValue(ref _IsLoading, value);
        }

        public List<Location> Locations
        {
            get => _Locations;
            set => SetValue(ref _Locations, value);
        }

        public Location SelectedItem
        {
            set
            {
                if (value is null)
                    return;

                _locationPicked.Invoke(value);
                PopModalNavigate();
            }
        }

        public ICommand SearchingForLocationsCommand { get; private set; }

        public LocationsViewModel(Action<Location> LocationPicked)
        {
            _locationPicked = LocationPicked;

            SearchingForLocationsCommand = new Command(SearchingForLocations);
        }

        private async void SearchingForLocations()
        {
            IsLoading = true;

            try
            {
                var LocationsData = await GoService.SearchingForLocations(URL, Text);

                if(LocationsData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Locations = LocationsData.Data;
                }
                else
                {
                    DisplayMessage("Ócurrio un error recuperando la información", "Ups!");
                }
            }
            catch
            {
                DisplayMessage("Ócurrio un error recuperando la información", "Ups!");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}