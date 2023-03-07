using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Go.Models;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class PaginationViewModel<T> : ViewModelBase
    {
        public int Page = 1;
        private bool _IsEmpty;
        private bool _IsLoading;
        private bool _IsRefreshing;
        private bool _IsLoadingMore;
        private List<T> _Elements;

        public bool IsEmpty
        {
            get => _IsEmpty;
            set => SetValue(ref _IsEmpty, value);
        }

        public bool IsLoading
        {
            get => _IsLoading;
            set
            {
                if (_IsLoading is false && value)
                    Task.Run(LoadData);

                _IsLoading = value;

                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public bool IsRefreshing
        {
            get => _IsRefreshing;
            set
            {
                _IsRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));

                if (IsRefreshing)
                {
                    IsLoading = true;
                    IsRefreshing = false;
                }
            }
        }

        public bool IsLoadingMore
        {
            get => _IsLoadingMore;
            set => SetValue(ref _IsLoadingMore, value);
        }

        public List<T> Elements
        {
            get => _Elements;
            set => SetValue(ref _Elements, value);
        }

        public int EmptyImage
        {
            get
            {
                Random Random = new Random();
                return Random.Next(1, 4);
            }
        }

        public ICommand ElementAppearingCommand { get; private set; }

        public PaginationViewModel()
        {
            ElementAppearingCommand = new Command<ItemVisibilityEventArgs>(ElementAppearing);
        }

        public virtual async Task<Page<T>> GetData()
        {
            await Task.CompletedTask;

            throw new NotImplementedException();
        }

        public async void LoadData()
        {
            Page = 1;
            IsEmpty = false;

            try
            {
                var ElementsData = await GetData();

                if (ElementsData != null)
                {
                    Elements = ElementsData.Results;

                    if (ElementsData.Next is null)
                        IsLoadingMore = false;
                    else
                        IsLoadingMore = true;

                    if (ElementsData.Count.Equals(0))
                        IsEmpty = true;
                }
                else
                {
                    IsEmpty = true;
                    IsLoadingMore = false;
                }
            }
            catch
            {
                DisplayMessage("No pudimos recuperar la información, intentalo nuevamnete", "Tuvimos un problema");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void ElementAppearing(ItemVisibilityEventArgs Args)
        {
            if (Args.Item is null || Elements is null)
                return;

            var Item = (T)Args.Item;

            if (IsLoadingMore && Elements.Count > 0)
            {
                if (Elements.Last().Equals(Item))
                {
                    Page++;
                    var ElementsData = await GetData();

                    if (ElementsData != null)
                    {
                        var TotalElements = Elements.ToList();
                        TotalElements.AddRange(ElementsData.Results);

                        Elements = null;
                        Elements = TotalElements;

                        if (ElementsData.Next is null)
                            IsLoadingMore = false;
                    }
                    else
                    {
                        IsLoadingMore = false;
                    }
                }
            }
        }
    }
}
