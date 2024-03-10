﻿using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Profile
{
    [Route("api/[controller]")]
    [ApiController]
    public class EucationController : ParentController<Education, EducationModel>
    {
        private readonly IEducations_Service _Service;
        private readonly IAuthManager        _authManager;
        public EucationController
        (
            IEducations_Service service,
            IMapper mapper,
            IAuthManager authManager
        ) : base(service, mapper)
        {
            _Service     = service;
            _authManager = authManager;
        }
    }
}