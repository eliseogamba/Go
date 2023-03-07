using System;
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
    public class EventsViewModel : PaginationViewModel<Event>
    {
        //Filters
        private TimeRange TimeRange;
        private List<Category> Categories;

        private bool _SearcherVisible = false;
        private string _Text;
        private Event _Event;

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

        public ICommand FilterCommand { get; private set; }
        public ICommand SetSearcherVisibilityCommand { get; private set; }
        public ICommand LookingForEventsCommand { get; private set; }
        public ICommand ItemAppearingCommand { get; private set; }

        public EventsViewModel()
        {
            IsLoading = true;

            FilterCommand = new Command(Filter);
            SetSearcherVisibilityCommand = new Command(SetSearcherVisibility);
            LookingForEventsCommand = new Command(LookingForEvents);
            ItemAppearingCommand = new Command<ItemVisibilityEventArgs>(ItemAppearing);
        }

        private void Filter()
        {
            NavigationModalTo(new NavigationPage(new FilterPage(FiltersApplied))
            {
                BarBackgroundColor = Color.White
            });
        }

        private void FiltersApplied(TimeRange TimeRange, List<Category> Categories)
        {
            this.TimeRange = TimeRange;
            this.Categories = Categories;

            IsLoading = true;
        }

        public override async Task<Page<Event>> GetData()
        {
            try
            {
                string CategoriesText = string.Empty;
                DateTime? Start = null;
                DateTime? End = null;

                if(TimeRange != null)
                {
                    var Dates = TimeRange.Dates();

                    Start = Dates.Item1;
                    End = Dates.Item2;
                }

                if(Categories != null && Categories.Count > 0)
                {
                    CategoriesText = string.Join(",", Categories.Select(x => x.Id));
                }

                var EventsData = await GoService.GetEvents(URL, Location.Id, Page, CategoriesText, Start, End, Text, Token);

                if(EventsData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return EventsData.Data;
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

        private void SetSearcherVisibility()
        {
            SearcherVisible = true;
        }

        private void LookingForEvents()
        {
            IsLoading = true;
        }

        private async void ItemAppearing(ItemVisibilityEventArgs args)
        {
            var Item = (Event)args.Item;

            if(!Item.WasSeen)
            {
                var AddViewData = await GoService.AddEventView(URL, Item.Id);

                if (AddViewData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Item.WasSeen = true;   
                }
            }
        }
    }
}