using UI.ViewModel.Common;

namespace UI.ViewModel
{
    public class AssetViewModel: BaseViewModel
    {
        public int UserId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Cost { get; set; }
    }
}
