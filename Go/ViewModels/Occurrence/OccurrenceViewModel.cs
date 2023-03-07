using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Go.Models;
using Go.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Go.ViewModels
{
    public class OccurrenceViewModel : ViewModelBase
    {
        private bool _IsLoading;
        private readonly Type _OccurrenceType; 
        private Occurrence _Occurrence;

        public bool IsLoading
        {
            get => _IsLoading;
            set => SetValue(ref _IsLoading, value);
        }

        public Occurrence Occurrence
        {
            get => _Occurrence;
            set => SetValue(ref _Occurrence, value);
        }

        public ICommand SetFavoriteCommand { get; private set; }

        public OccurrenceViewModel(Occurrence Occurrence, Map map)
        {
            this.Occurrence = Occurrence;

            _OccurrenceType = Occurrence.GetType();

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Occurrence.Latitude, Occurrence.Longitude), Distance.FromKilometers(1)));

            map.Pins.Add(new Pin()
            {
                Position = new Position(Occurrence.Latitude, Occurrence.Longitude),
                Label = string.Empty
            });

            if(!Occurrence.WasOpened)
            {
                Task.Run(AddOpen);
            }

            SetFavoriteCommand = new Command(SetFavorite);
        }

        private async void SetFavorite()
        {
            IsLoading = true;

            if(Occurrence.IsFavorite)
            {
                //Delete favorite
                Response DeleteResponse;

                if(_OccurrenceType == typeof(Event))
                {
                    DeleteResponse = await GoService.RemoveEventFromFavorities(URL, Occurrence.Id, Token);
                }
                else
                {
                    DeleteResponse = await GoService.RemoveActivityFromFavorities(URL, Occurrence.Id, Token);
                }

                if (DeleteResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    Occurrence.IsFavorite = false;
                }
                else
                {
                    if (DeleteResponse.ErrorData != null)
                    {
                        DisplayMessage(DeleteResponse.ErrorData.Message, "Tuvimos un problema");
                    }
                    else
                    {
                        DisplayMessage("No quitarlo de tus favoritos, intentalo nuevamente", "Tuvimos un problema");
                    }
                }
            }
            else
            {
                //Add to favorities
                Response AddResponse;

                if (_OccurrenceType == typeof(Event))
                {
                    AddResponse = await GoService.AddEventToFavorities(URL, Occurrence.Id, Token);
                }
                else
                {
                    AddResponse = await GoService.AddActivityToFavorities(URL, Occurrence.Id, Token);
                }
                

                if (AddResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Occurrence.IsFavorite = true;
                }
                else
                {
                    if (AddResponse.ErrorData != null)
                    {
                        DisplayMessage(AddResponse.ErrorData.Message, "Tuvimos un problema");
                    }
                    else
                    {
                        DisplayMessage("No pudimos agregarlo a tus favoritos, intentalo nuevamente", "Tuvimos un problema");
                    }
                }
            }

            IsLoading = false;
        }

        private async void AddOpen()
        {
            if(_OccurrenceType == typeof(Event))
            {
                var AddOpenData = await GoService.AddEventOpen(URL, Occurrence.Id);

                if (AddOpenData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Occurrence.WasOpened = true;
                }
            }
            else
            {
                var AddOpenData = await GoService.AddActivityOpen(URL, Occurrence.Id);

                if (AddOpenData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Occurrence.WasOpened = true;
                }
            }
        }
    }
}