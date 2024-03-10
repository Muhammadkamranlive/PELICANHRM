using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class GeneralTask_Repo:Repo<GENERALTASK>, IGeneralTask_Repo
    {
        private readonly ERPDb _Db;
        public GeneralTask_Repo(ERPDb db):base(db) 
        {
            _Db = db;
        }
    }
}
