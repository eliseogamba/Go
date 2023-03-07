using System.Windows.Input;
using Go.Common;
using Go.Services;
using Go.Views;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private bool _IsLoading;

        public bool IsLoading
        {
            get => _IsLoading;
            set => SetValue(ref _IsLoading, value);
        }

        public ICommand CreateCommand { get; private set; }
        public ICommand MyEventsCommand { get; private set; }
        public ICommand MyActivitiesCommand { get; private set; }
        public ICommand FavoriteEventsCommand { get; private set; }
        public ICommand FavoriteActivitiesCommand { get; private set; }
        public ICommand ChangePasswordCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public UserViewModel()
        {
            CreateCommand = new Command(Create);
            MyEventsCommand = new Command(MyEvents);
            MyActivitiesCommand = new Command(MyActivities);
            FavoriteEventsCommand = new Command(FavoriteEvents);
            FavoriteActivitiesCommand = new Command(FavoriteActivities);
            ChangePasswordCommand = new Command(ChangePassword);
            LogoutCommand = new Command(Logout);
        }

        private async void Create()
        {
            IsLoading = true;

            var CategoryData = await GoService.GetCategories(URL);

            if (CategoryData.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var CountryData = await GoService.GetCountries(URL);

                if (CountryData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NavigateTo(new CreatePage(CategoryData.Data, CountryData.Data));
                }
                else
                {
                    if (CountryData.ErrorData != null)
                    {
                        DisplayMessage(CountryData.ErrorData.Message, "Tuvimos un problema");
                    }
                    else
                    {
                        DisplayMessage("No pudimos recuperar los paises, intentalo nuevamente", "Tuvimos un problema");
                    }
                }
            }
            else
            {
                if (CategoryData.ErrorData != null)
                {
                    DisplayMessage(CategoryData.ErrorData.Message, "Tuvimos un problema");
                }
                else
                {
                    DisplayMessage("No pudimos recuperar las categorías, intentalo nuevamente", "Tuvimos un problema");
                }
            }

            IsLoading = false;
        }
        
        private void MyEvents()
        {
            NavigateTo(new MyEventsPage());
        }
        
        private void MyActivities()
        {
            NavigateTo(new MyActivitiesPage());
        }

        private void FavoriteEvents()
        {
            NavigateTo(new FavoriteEventsPage());
        }

        private void FavoriteActivities()
        {
            NavigateTo(new FavoriteActivitiesPage());
        }

        private void ChangePassword()
        {
            NavigateTo(new ChangePasswordPage());
        }

        private void Logout()
        {
            Xamarin.Essentials.Preferences.Remove(Constants.TokenKey);

            MessagingCenter.Send<ViewModelBase>(this, "TokenUpdated");
        }
    }
}