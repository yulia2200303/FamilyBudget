using System.Collections.Generic;
using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    public interface IAssetRepository : IRepository<Asset>
    {
        bool IsAssetExists(string name);
    }
}