using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class Dependent_Repo : Repo<Dependent>, IDependent_Repo
    {
        private readonly ERPDb _db;
        public Dependent_Repo(ERPDb db) : base(db)
        {
            _db = db;
        }
    }
}
