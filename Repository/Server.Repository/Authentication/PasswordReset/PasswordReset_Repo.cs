using Server.Core;
using Server.Domain;

namespace Server.Repository
{
    public class PasswordReset_Repo : Repo<PasswordResetDomain>, IPasswordReset_Repo
    {
        public PasswordReset_Repo(ERPDb db) : base(db)
        {

        }
    }
}