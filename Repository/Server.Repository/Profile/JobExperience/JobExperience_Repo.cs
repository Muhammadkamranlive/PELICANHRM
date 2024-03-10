using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class JobExperience_Repo : Repo<JobExperience>, IJobExperience_Repo
    {
        private readonly ERPDb _db;
        public JobExperience_Repo(ERPDb db) : base(db)
        {
            _db = db;
        }
    }
}
