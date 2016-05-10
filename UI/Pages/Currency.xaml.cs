using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DAL.Common;
using Shared.Constant;
using UI.Logic;
using UI.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UI.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Currency : Page
    {
        public Currency()
        {
            InitializeComponent();

            var model = new CurrencyViewModel();
            DataContext = model;
        }
    }
}