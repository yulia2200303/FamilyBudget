using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Prism.Commands;
using UI.Logic;
using UI.Pages;
using UI.ViewModel.Common;

namespace UI.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            var supportedLanguages = LocalizationHelper.GetSupportedLocales()
                .Select(l => new ComboboxItem()
                {
                    Text = l.Value,
                    Value = l.Key
                }).ToList();

            Languages = new ObservableCollection<ComboboxItem>(supportedLanguages);
            SelectedLanguage = supportedLanguages.First(l => l.Value.ToString() == LocalizationHelper.GetCurrentLocale());
            ChangeLocaleCommand = new DelegateCommand(OnChangeLocale);
        }


        private ObservableCollection<ComboboxItem> _languges;

        public ObservableCollection<ComboboxItem> Languages
        {
            get { return _languges; }
            set { SetProperty(ref _languges, value); }
        }

        private ComboboxItem _selectedLanguage;

        public ComboboxItem SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                SetProperty(ref _selectedLanguage, value);
            }
        }

        public ICommand ChangeLocaleCommand { get; }

        private async void OnChangeLocale()
        {
            if (SelectedLanguage == null) return;
            LocalizationHelper.SetLocale(SelectedLanguage.Value.ToString());
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            Windows.ApplicationModel.Resources.Core.ResourceContext.ResetGlobalQualifierValues();
            Windows.ApplicationModel.Resources.Core.ResourceContext.SetGlobalQualifierValue("language", SelectedLanguage.Value.ToString());

            await Task.Delay(500);
            var rootFrame = Window.Current.Content as Frame;
            rootFrame?.Navigate(rootFrame.Content.GetType());
        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
