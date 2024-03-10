using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class EmergencyContacts_Repo:Repo<EmergencyContacts>,IEmergencyContacts_Repo
    {
        private readonly ERPDb eRPDb;
        public EmergencyContacts_Repo(ERPDb db):base(db) 
        {
                eRPDb = db;
        }
    }
}
