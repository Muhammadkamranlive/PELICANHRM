using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class Page_Repo:Repo<WebPages>, IPage_Repo
    {
        public Page_Repo(ERPDb dbContext) : base(dbContext)
        {

        }
    }
}
