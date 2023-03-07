using System.Threading.Tasks;
using System.Windows.Input;
using Go.Models;
using Go.Services;
using Go.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class MyActivitiesViewModel : PaginationViewModel<Activity>
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

        public ICommand OptionsCommand { get; private set; }

        public MyActivitiesViewModel()
        {
            IsLoading = true;

            OptionsCommand = new Command<Activity>(ShowOptions);
        }

        public override async Task<Page<Activity>> GetData()
        {
            try
            {
                var ActivitiesData = await GoService.GetOwnedActivities(URL, Page, Token);

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

        private void ShowOptions(Activity currentActivity)
        {
            PopupNavigation.Instance.PushAsync(new OptionsPopup(currentActivity, typeof(Activity)));
        }
    }
}