using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    internal class AssetRepository : Repository<Asset>, IAssetRepository
    {
        public AssetRepository(Microsoft.Data.Entity.DbContext dataContext) : base(dataContext)
        {
        }
    }
}