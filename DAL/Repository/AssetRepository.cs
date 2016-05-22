using System;
using System.Linq;
using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    internal class AssetRepository : Repository<Asset>, IAssetRepository
    {
        public AssetRepository(Microsoft.Data.Entity.DbContext dataContext) : base(dataContext)
        {
        }

        public bool IsAssetExists(string name)
        {
            return DbContext.Assets.Any(a => a.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}