﻿using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class EmergencyContact_Service:Base_Service<EmergencyContacts>, IEmergencyContact_Service
    {

        public EmergencyContact_Service(IUnitOfWork unitOfWork, IEmergencyContacts_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
