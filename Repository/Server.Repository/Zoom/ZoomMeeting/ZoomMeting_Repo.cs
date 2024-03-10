using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class ZoomMeting_Repo : Repo<ZoomMeetings>, IZoomMeting_Repo
    {
        public ZoomMeting_Repo(ERPDb dbContext) : base(dbContext)
        {

        }
    }
}
