using System.Collections.Generic;
using DAL.Model;
using DAL.Repository.Common;

namespace DAL.Repository
{
    public interface IAssetRepository : IRepository<Asset>
    {
        bool IsAssetExists(string name);
        double GetSummary(int assetId);
        List<Asset> GetByUserId(int userId);
        void RemoveByUserId(int userId);
        void Insert(int userId, IEnumerable<string> assets);
    }
}