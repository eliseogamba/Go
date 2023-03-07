using System.Threading.Tasks;
using Go.Models;
using Go.Services;
using Go.Views;

namespace Go.ViewModels
{
    public class FavoriteActivitiesViewModel : PaginationViewModel<Activity>
    {
        private Activity _Activity;

        public Activity Activity
        {
            get => _Activity;
            set
            {
                _Activity = value;
                OnPropertyChanged(nameof(Activity));

                if (value is null)
                    return;

                Activity = null;
                NavigateTo(new OccurrencePage(value));
            }
        }

        public FavoriteActivitiesViewModel()
        {
            IsLoading = true;
        }

        public override async Task<Page<Activity>> GetData()
        {
            try
            {
                var ActivitiesData = await GoService.GetFavoriteActivities(URL, Page, Token);

                if (ActivitiesData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return ActivitiesData.Data;
                }
                else
                {
                    DisplayMessage("No pudimos recuperar las actividades", "Parece que hubo un problema");

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