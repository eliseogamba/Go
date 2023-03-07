using System;
using System.Collections.Generic;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms;

namespace Go.Controls
{
    public class ChangeableSvgCachedImage : SvgCachedImage
    {
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected),
                                                                                            typeof(bool),
                                                                                            typeof(ChangeableSvgCachedImage),
                                                                                            propertyChanged: OnIsSelectedPropertyChanged);

        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(IsSelectedProperty);
            }
            set
            {
                SetValue(IsSelectedProperty, value);
                OnPropertyChanged(nameof(IsSelected));

                if (value)
                {
                    var dict = new Dictionary<string, string>();
                    dict.Add("fill=\"#000000\"", "fill=\"#ED1C24\"");

                    ReplaceStringMap = dict;
                }
                else
                {
                    var dict = new Dictionary<string, string>();
                    dict.Add("fill=\"#ED1C24\"", "fill=\"#000000\"");

                    ReplaceStringMap = dict;
                }
            }
        }

        private static void OnIsSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if(newValue != null)
            {
                ((ChangeableSvgCachedImage)bindable).IsSelected = (bool)newValue;
            }
        }

        public ChangeableSvgCachedImage()
        {
        }
    }
}
