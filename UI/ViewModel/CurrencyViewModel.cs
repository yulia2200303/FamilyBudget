using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Popups;
using DAL.Common;
using DAL.Model;
using Microsoft.Practices.Prism.Commands;
using Shared.Constant;
using UI.Logic;
using UI.ViewModel.Common;

namespace UI.ViewModel
{
    public class CurrencyViewModel : BaseViewModel
    {
        public CurrencyViewModel()
        {
            using (var uow = new UnitOfWork())
            {
                var currencies = uow.CurrencyRepository.GetCurrenciesByCode(excludedCodes: new[] { CurrencyCode.BelarussianRub }); ;
                Currencies = new ObservableCollection<Currency>(currencies);
                UpdateDate = currencies.First().UpadeDate;
            }

            RefreshCommand = new DelegateCommand(OnRefreshCommand);
        }

        private ObservableCollection<Currency> _carrencies;

        public ObservableCollection<Currency> Currencies
        {
            get { return _carrencies; }
            set { SetProperty(ref _carrencies, value); }
        }

        private DateTime _updateDate;

        public DateTime UpdateDate
        {
            get { return _updateDate; }
            set { SetProperty(ref _updateDate, value); }
        }

        public ICommand RefreshCommand { get; }

        private async void OnRefreshCommand()
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var cwWrapper = new CurrencyServiceWrapper();
                    await cwWrapper.RefreshCurrencies();
                    var currencies = uow.CurrencyRepository.GetCurrenciesByCode(excludedCodes: new[] { CurrencyCode.BelarussianRub });
                    Currencies = new ObservableCollection<DAL.Model.Currency>(currencies);
                    UpdateDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Ошибка!");
                await dialog.ShowAsync();
            }
        }
    }
}
