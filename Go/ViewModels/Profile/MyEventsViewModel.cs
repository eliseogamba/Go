using System.Threading.Tasks;
using System.Windows.Input;
using Go.Models;
using Go.Services;
using Go.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class MyEventsViewModel : PaginationViewModel<Event>
    {
        private Event _Event;

        public Event Event
        {
            get => _Event;
            set
            {
                _Event = value;
                OnPropertyChanged(nameof(Event));

                if (value is null)
                    return;

                Event = null;
                NavigateTo(new OccurrencePage(value));
            }
        }

        public ICommand OptionsCommand { get; private set; }

        public MyEventsViewModel()
        {
            IsLoading = true;

            OptionsCommand = new Command<Event>(ShowOptions);
        }

        public override async Task<Page<Event>> GetData()
        {
            try
            {
                var ActivitiesData = await GoService.GetOwnedEvents(URL, Page, Token);

                if (ActivitiesData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return ActivitiesData.Data;
                }
                else
                {
                    DisplayMessage("No pudimos recuperar los eventos", "Parece que hubo un problema");

                    return null;
                }
            }
            catch
            {
                DisplayMessage("Ócurrio un error, intentalo nuevamente", "Ups!");

                return null;
            }
        }

        private void ShowOptions(Event currentEvent)
        {
            PopupNavigation.Instance.PushAsync(new OptionsPopup(currentEvent, typeof(Event)));
        }
    }
}