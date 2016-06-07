using System.Collections.Generic;
using System.Linq;
using DAL.Common;
using UI.ViewModel;
using UI.ViewModel.Common;

namespace UI.Logic
{
    class AssetHelper
    {
        private readonly int _userId;

        public AssetHelper(int userId)
        {
            _userId = userId;
        }

        public List<AssetViewModel> GetAssets()
        {
           
            List<AssetViewModel> assets = new List<AssetViewModel>();

            using (var uow = new UnitOfWork())
            {
                var dbassets = uow.AssetRepository.GetByQuery(a => a.UserId == _userId).ToList();
                foreach (var asset in dbassets)
                {
                    var cost = uow.AssetRepository.GetSummary(asset.Id);
                    assets.Add(new AssetViewModel()
                    {
                        UserId = asset.UserId,
                        Cost = (decimal) cost,
                        Name = asset.Name,
                        Id = asset.Id,
                    });
                }
            }
            return assets;
        } 
    }
}
