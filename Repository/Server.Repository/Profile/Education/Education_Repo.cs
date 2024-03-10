using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class Education_Repo:Repo<Education>,IEducation_Repo
    {
        private readonly ERPDb _db;
        public Education_Repo(ERPDb db):base(db) 
        {
            _db = db;
        }
    }
}
