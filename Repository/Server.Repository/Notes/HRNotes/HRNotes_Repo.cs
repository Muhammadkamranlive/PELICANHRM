using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class HRNotes_Repo:Repo<HRNotes>, IHRNotes_Repo
    {
        private readonly ERPDb _db;
        public HRNotes_Repo(ERPDb db):base(db)
        {
            _db = db;
        }
    }
}
