using System.Windows.Input;
using Go.Common;
using Go.Models;
using Go.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class SetLocationViewModel : ViewModelBase
    {
        private Location _Location;
        
        public new Location Location
        {
            get => _Location;
            set => SetValue(ref _Location, value);
        }

        public bool HasLocation
        {
            get
            {
                if (Location is null)
                    return false;

                return true;
            }
        }

        public ICommand PickLocationCommand { get; private set; }
        public ICommand StartAppCommand { get; private set; }

        public SetLocationViewModel()
        {
            PickLocationCommand = new Command(PickLocation);
            StartAppCommand = new Command(StartApp);
        }

        private void PickLocation()
        {
            NavigationModalTo(new NavigationPage(new LocationsPage(LocationPicked)));
        }

        private void LocationPicked(Location Location)
        {
            this.Location = Location;
            OnPropertyChanged(nameof(HasLocation));
        }

        private void StartApp()
        {
            Xamarin.Essentials.Preferences.Set(Constants.LocationKey, JsonConvert.SerializeObject(Location));
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}