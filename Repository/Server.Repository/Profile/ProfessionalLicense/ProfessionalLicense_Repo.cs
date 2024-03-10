using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class ProfessionalLicense_Repo:Repo<ProfessionalLicense>, IProfessionalLicense_Repo
    {
        private readonly ERPDb _db;
        public ProfessionalLicense_Repo(ERPDb db):base(db) 
        {
            _db = db;
        }
    }
}
