using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class Personal_Repo:Repo<Personal>,IPersonal_Repo
    {
        private readonly ERPDb _db;
        public Personal_Repo(ERPDb db):base(db) 
        {
            _db = db;
        }
    }
}
