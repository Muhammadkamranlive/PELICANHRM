﻿using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Training_Service:Base_Service<Trainings>, ITraining_Service
    {
        public Training_Service(IUnitOfWork unitOfWork, ITraining_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
