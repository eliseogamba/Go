using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Go.Controls
{
    public class Loader : ListView
    {
        public static readonly BindableProperty CountElementsProperty = BindableProperty.Create(nameof(CountElements),
                                                                                                typeof(int),
                                                                                                typeof(Loader),
                                                                                                0,
                                                                                                BindingMode.TwoWay);

        public int CountElements
        {
            get
            {
                return (int)GetValue(CountElementsProperty);
            }
            set
            {
                SetValue(CountElementsProperty, value);
            }
        }

        public Loader()
        {
            ItemTapped += LoaderItemTapped;
        }

        private void LoaderItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (!string.IsNullOrEmpty(propertyName) && propertyName.Equals(nameof(CountElements)))
            {
                var Elements = new List<int>();

                ItemsSource = null;
                for (var i = 0; i < CountElements; i++)
                {
                    Elements.Add(i);
                }
                ItemsSource = Elements;
            }
        }
    }
}