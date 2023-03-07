using System.Threading.Tasks;
using Go.Models;
using Go.Services;
using Go.Views;

namespace Go.ViewModels
{
    public class FavoriteEventsViewModel : PaginationViewModel<Event>
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

        public FavoriteEventsViewModel()
        {
            IsLoading = true;
        }

        public override async Task<Page<Event>> GetData()
        {
            try
            {
                var ActivitiesData = await GoService.GetFavoriteEvents(URL, Page, Token);

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
    }
}