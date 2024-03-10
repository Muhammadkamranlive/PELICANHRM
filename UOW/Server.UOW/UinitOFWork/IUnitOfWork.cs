﻿using Server.Repository;

namespace Server.UOW
{
    public interface IUnitOfWork:IDisposable
    {
        public IPasswordReset_Repo       PasswordReset_Repo       { get;}
        public IContactDetails_Repo      ContactDetails_Repo      { get;}
        public IEmergencyContacts_Repo   EmergencyContacts_Repo   { get;}
        public ICaseCommetns_Repo        CaseCommetns_Repo        { get;}
        public ICase_Repo                Case_Repo                { get;}
        public IAssetManager_Repo        AssetManager_Repo        { get;}
        public IAttachments_Repo         Attachments_Repo         { get;}
        public IHRNotes_Repo             HRNotes_Repo             { get;}
        public INotifications_Repo       Notifications_Repo       { get;}
        public ICandidate_Repo           Candidate_Repo           { get;}
        public IEducation_Repo           Education_Repo           { get;}
        public IJobExperience_Repo       JobExperience_Repo       { get;}
        public IProfessionalLicense_Repo ProfessionalLicense_Repo { get;}
        public IPersonal_Repo            Personal_Repo            { get;}
        public IZoomMeting_Repo          zoomMeeting_Repo         { get;}
        public IPage_Repo                page_Repo                { get;}
        public IBlog_Repo                blog_Repo                { get;}
        public IContactPage_Repo         ContactPage_ { get; }
        Task<int> Save();
    }
}
