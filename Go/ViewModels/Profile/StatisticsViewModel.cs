using System;
using System.Windows.Input;
using Go.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly Type _type;

        private Occurrence _Occurrence;

        public Occurrence Occurrence
        {
            get => _Occurrence;
            set => SetValue(ref _Occurrence, value);
        }

        public string Kind
        {
            get
            {
                if (_type == typeof(Event))
                {
                    return "Evento Publicitado";
                }
                else
                {
                    return "Actividad Publicitada";
                }
            }
        }

        public ICommand AdCommand { get; private set; }

        public StatisticsViewModel(Occurrence occurrence, Type type)
        {
            _type = type;

            Occurrence = occurrence;

            AdCommand = new Command(Ad);
        }

        //TODO Add the URL
        private void Ad()
        {
            Browser.OpenAsync("www.google.com");
        }
    }
}