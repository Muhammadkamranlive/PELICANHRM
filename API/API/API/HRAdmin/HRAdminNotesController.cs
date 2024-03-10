﻿using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.HRAdmin
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRAdminNotesController : ParentController<HRNotes, HRNoteModel>
    {
        private readonly IHRNotes_Service       _Service;
        private readonly IAuthManager           _authManager;
        public HRAdminNotesController
        (
            IHRNotes_Service service,
            IMapper mapper,
            IAuthManager authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }
    }
}
