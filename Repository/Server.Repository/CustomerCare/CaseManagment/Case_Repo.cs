using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class Case_Repo:Repo<Case>, ICase_Repo
    {
        private readonly ERPDb _db;
        public Case_Repo(ERPDb eRP):base(eRP) 
        {
            _db = eRP;   
        }
    }
}
