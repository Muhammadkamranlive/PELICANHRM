using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class Candidate_Repo:Repo<CandidateInfo>,ICandidate_Repo
    {
        private readonly ERPDb _db;
        public Candidate_Repo(ERPDb db):base(db) 
        {
            _db = db;
        }
    }
}
