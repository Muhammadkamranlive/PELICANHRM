using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class Blog_Repo:Repo<BlogPage>, IBlog_Repo
    {
        private readonly ERPDb _db;
        public Blog_Repo(ERPDb db) : base(db)
        {
            _db = db;
        }
    }
}
