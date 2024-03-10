using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class AssetManager_Repo:Repo<Asset>, IAssetManager_Repo
    {
        private readonly ERPDb eRPDb;
        public AssetManager_Repo(ERPDb db):base(db) 
        {
            eRPDb = db;
        }
    }
}
