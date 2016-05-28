using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModel.Common
{
    public class AssetViewModel: BaseViewModel
    {
        public int UserId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Cost { get; set; }
    }
}
