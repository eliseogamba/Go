using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Go.Models;
using Go.Services;
using Go.Views;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class ActivitiesViewModel : PaginationViewModel<Activity>
    {
        //Filters
        private List<Category> Categories;

        private bool _SearcherVisible = false;
        private string _Text;
        private Activity _Activity;

        public bool SearcherVisible
        {
            get => _SearcherVisible;
            set => SetValue(ref _SearcherVisible, value);
        }

        public string Text
        {
            get => _Text;
            set => SetValue(ref _Text, value);
        }

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

        public ICommand FilterCommand { get; private set; }
        public ICommand SetSearcherVisibilityCommand { get; private set; }
        public ICommand LookingForActivitiesCommand { get; private set; }
        public ICommand ItemAppearingCommand { get; private set; }

        public ActivitiesViewModel()
        {
            IsLoading = true;

            FilterCommand = new Command(Filter);
            SetSearcherVisibilityCommand = new Command(SetSearcherVisibility);
            LookingForActivitiesCommand = new Command(LookingFoActivities);
            ItemAppearingCommand = new Command<ItemVisibilityEventArgs>(ItemAppearing);
        }

        private void Filter()
        {
            NavigationModalTo(new NavigationPage(new FilterPage(FiltersApplied, true))
            {
                BarBackgroundColor = Color.White
            });
        }

        private void FiltersApplied(TimeRange TimeRange, List<Category> Categories)
        {
            this.Categories = Categories;

            IsLoading = true;
        }

        public override async Task<Page<Activity>> GetData()
        {
            try
            {
                string CategoriesText = string.Empty;

                if (Categories != null && Categories.Count > 0)
                {
                    CategoriesText = string.Join(",", Categories.Select(x => x.Id));
                }

                var ActivitiesData = await GoService.GetActivities(URL, Location.Id, Page, CategoriesText, Text);

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

        private void SetSearcherVisibility()
        {
            SearcherVisible = true;
        }

        private void LookingFoActivities()
        {
            IsLoading = true;
        }

        private async void ItemAppearing(ItemVisibilityEventArgs args)
        {
            var Item = (Activity)args.Item;

            if (!Item.WasSeen)
            {
                var AddViewData = await GoService.AddActivityView(URL, Item.Id);

                if (AddViewData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Item.WasSeen = true;
                }
            }
        }
    }
}