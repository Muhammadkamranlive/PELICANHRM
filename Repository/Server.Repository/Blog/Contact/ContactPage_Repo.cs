using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class ContactPage_Repo:Repo<ContactPage>,IContactPage_Repo
    {
        public ContactPage_Repo(ERPDb dbContext) : base(dbContext)
        {

        }
    }
}
