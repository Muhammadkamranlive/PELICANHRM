using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class Attachments_Repo:Repo<Attachments>, IAttachments_Repo
    {
        private readonly ERPDb _db;
        public Attachments_Repo(ERPDb DB):base(DB) 
        {
            _db = DB;
        }
    }
}
