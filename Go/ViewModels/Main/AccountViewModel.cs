using Go.Common;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private bool _IsLoged;

        public bool IsLoged
        {
            get => _IsLoged;
            set => SetValue(ref _IsLoged, value);
        }

        public AccountViewModel()
        {
            ValidateLogin();

            MessagingCenter.Subscribe<ViewModelBase>(this, "TokenUpdated", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ValidateLogin();
                });
            });
        }

        public void ValidateLogin()
        {
            Token = Preferences.Get(Constants.TokenKey, null);

            if (string.IsNullOrEmpty(Token))
            {
                IsLoged = false;
            }
            else
            {
                IsLoged = true;
            }
        }
    }
}