using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class CaseCommetns_Repo:Repo<CaseComment>, ICaseCommetns_Repo
    {
        private readonly ERPDb eRPDb;
        public CaseCommetns_Repo(ERPDb db):base(db)
        {
                eRPDb = db;
        }
    }
}
