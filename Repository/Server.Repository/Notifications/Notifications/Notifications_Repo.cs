using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class Notifications_Repo:Repo<NOTIFICATIONS>, INotifications_Repo
    {
        private readonly ERPDb _db;
        public Notifications_Repo(ERPDb db):base(db) 
        {
            _db = db;
        }
    }
}
