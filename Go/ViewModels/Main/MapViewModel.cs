using System.Threading.Tasks;
using System.Windows.Input;
using Go.Common;
using Go.Models;
using Go.Services;
using Go.Views;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Go.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly Frame _popup;

        private Map _Map;
        private Occurrence _Occurrence;
        private string _Kind;

        public Map Map
        {
            get => _Map;
            set => SetValue(ref _Map, value);
        }

        public Occurrence Occurrence
        {
            get => _Occurrence;
            set => SetValue(ref _Occurrence, value);
        }

        public string Kind
        {
            get => _Kind;
            set => SetValue(ref _Kind, value);
        }

        public ICommand CloseCommand { get; private set; }
        public ICommand PopupTappedCommand { get; private set; }

        public MapViewModel(Map map, Frame popup)
        {
            _popup = popup;
            Map = map;

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Location.Latitude, Location.Longitude), Distance.FromKilometers(5)));

            map.MapClicked += MapClicked;

            Task.Run(LoadMap);

            CloseCommand = new Command(ClosePopup);
            PopupTappedCommand = new Command(PopupTapped);
        }

        public async void LoadMap()
        {
            try
            {
                var MapData = await GoService.GetMapData(URL, Location.Id, Token);

                if (MapData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MapData.Data.ForEach((item) =>
                    {
                        item.Photo = $"{Constants.BaseURL}{item.Photo}";
                    });

                    foreach(var item in MapData.Data)
                    {
                        var pin = new Pin()
                        {
                            Position = new Position(item.Latitude, item.Longitude),
                            Label = string.Empty,
                            BindingContext = item
                        };

                        pin.MarkerClicked += PinClicked;

                        Map.Pins.Add(pin);
                    }
                }
            }
            catch
            {
                DisplayMessage("Ócurrio un error, intentalo nuevamente", "Ups!");
            }
        }

        private void ClosePopup()
        {
            _popup.ScaleTo(0);
        }

        private void MapClicked(object sender, MapClickedEventArgs e)
        {
            _popup.ScaleTo(0);
        }

        private void PinClicked(object sender, PinClickedEventArgs e)
        {
            var pin = sender as Pin;

            Occurrence = pin.BindingContext as Occurrence;

            if (Occurrence.DatetimeStart.HasValue)
            {
                Kind = "Evento";
            }
            else
            {
                Kind = "Actividad";
            }

            _popup.ScaleTo(1);
        }

        private void PopupTapped()
        {
            NavigateTo(new OccurrencePage(Occurrence));
        }
    }
}