using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class Training_Repo:Repo<Trainings>, ITraining_Repo
    {
        private readonly ERPDb _db;
        public Training_Repo(ERPDb DB) : base(DB)
        {
            _db = DB;
        }
    }
}
