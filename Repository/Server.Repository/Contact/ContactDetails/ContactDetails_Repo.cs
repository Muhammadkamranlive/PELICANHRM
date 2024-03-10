using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class ContactDetails_Repo:Repo<CONTACTDETAILS>, IContactDetails_Repo
    {
        private readonly ERPDb _db;
        public ContactDetails_Repo(ERPDb db):base(db) 
        {
                _db = db;
        }
    }
}
