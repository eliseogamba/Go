using System.Threading.Tasks;
using System.Windows.Input;
using Go.Common;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Go.ViewModels
{    
    public class ViewModelBase : Observable
    {
        private string _Token;

        public string URL { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand BackModalCommand { get; private set; }
        public ICommand BackPopupCommand { get; private set; }

        public string Token
        {
            get => _Token;
            set => SetValue(ref _Token, value);
        }

        public Models.Location Location
        {
            get
            {
                var LocationStorage = Preferences.Get(Constants.LocationKey, null);

                return JsonConvert.DeserializeObject<Models.Location>(LocationStorage);
            }
        }

        public ViewModelBase()
        {
            URL = $"{Constants.BaseURL}/api/v1";

            try
            {
                Token = Preferences.Get(Constants.TokenKey, null) ?? string.Empty;
            }
            catch { }

            BackCommand = new Command(Back);
            BackModalCommand = new Command(PopModalNavigate);
            BackPopupCommand = new Command(BackPopup);
        }

        public void Back()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PopAsync();
                });
            }
            catch { }
        }

        public void DisplayMessage(string message, string title = null, string accept = "Aceptar")
        {
            if (title is null)
                title = string.Empty;

            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert(title, message, accept);
                });
            }
            catch { }
        }

        public async Task<bool> DisplayQuestion(string title, string description, string accept = "Confirmar", string cancel = "Cancelar")
        {
            return await Application.Current.MainPage.DisplayAlert(title, description, accept, cancel);
        }

        public async Task<string> DisplayPrompt(string title, string description)
        {
            return await Application.Current.MainPage.DisplayPromptAsync(title, description);
        }

        public void NavigateTo(Page page)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(page);
            });
        }

        public void NavigationModalTo(Page page)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushModalAsync(page);
            });
        }

        public void PopModalNavigate()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PopModalAsync();
            });
        }

        public void BackPopup()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PopupNavigation.Instance.PopAsync();
            });
        }

        public void PopToRoot()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PopToRootAsync();
            });
        }
    }
}