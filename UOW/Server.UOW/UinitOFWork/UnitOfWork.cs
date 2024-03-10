using Server.Core;
using Server.Repository;

namespace Server.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ERPDb _crmContext;
        public UnitOfWork(ERPDb candidateCRM)
        {
                _crmContext             = candidateCRM;
               PasswordReset_Repo       = new PasswordReset_Repo(_crmContext);
               Candidate_Repo           = new Candidate_Repo(_crmContext);
               EmergencyContacts_Repo   = new EmergencyContacts_Repo(_crmContext);
               CaseCommetns_Repo        = new CaseCommetns_Repo(_crmContext);
               Case_Repo                = new Case_Repo(_crmContext);
               AssetManager_Repo        = new AssetManager_Repo(_crmContext);
               Attachments_Repo         = new Attachments_Repo(_crmContext);
               HRNotes_Repo             = new HRNotes_Repo(_crmContext);
               Notifications_Repo       = new Notifications_Repo(_crmContext);
               Candidate_Repo           = new Candidate_Repo(_crmContext);
               Education_Repo           = new Education_Repo(_crmContext);
               JobExperience_Repo       = new JobExperience_Repo(_crmContext);
               ProfessionalLicense_Repo = new ProfessionalLicense_Repo(_crmContext);
               Personal_Repo            = new Personal_Repo(_crmContext);
               zoomMeeting_Repo         = new ZoomMeting_Repo(_crmContext);
               page_Repo                = new Page_Repo(_crmContext);
               blog_Repo                = new Blog_Repo(_crmContext);
               ContactPage_             = new ContactPage_Repo(_crmContext);


        }
        public IPasswordReset_Repo PasswordReset_Repo             { get; private set; }
        public IContactDetails_Repo ContactDetails_Repo           { get; private set; }
        public IEmergencyContacts_Repo EmergencyContacts_Repo     { get; private set; }
        public ICaseCommetns_Repo CaseCommetns_Repo               { get; private set; }
        public ICase_Repo Case_Repo                               { get; private set; }
        public IAssetManager_Repo AssetManager_Repo               { get; private set; }
        public IAttachments_Repo Attachments_Repo                 { get; private set; }
        public IHRNotes_Repo HRNotes_Repo                         { get; private set; }
        public INotifications_Repo Notifications_Repo             { get; private set; }
        public ICandidate_Repo Candidate_Repo                     { get; private set; }
        public IEducation_Repo Education_Repo                     { get; private set; }
        public IJobExperience_Repo JobExperience_Repo             { get; private set; }
        public IProfessionalLicense_Repo ProfessionalLicense_Repo { get; private set; }
        public IPersonal_Repo Personal_Repo                       { get; private set; }
        public IZoomMeting_Repo zoomMeeting_Repo                  { get; private set; }
        public IPage_Repo page_Repo                               { get; private set; }
        public IBlog_Repo blog_Repo                               { get; private set; }

        public IContactPage_Repo ContactPage_                     { get; private set; }

        public async void Dispose()
        {
            GC.SuppressFinalize(this);
            await _crmContext.DisposeAsync();
        }
        public async Task<int> Save()
        {
            return await _crmContext.SaveChangesAsync();
        }
    }
}
