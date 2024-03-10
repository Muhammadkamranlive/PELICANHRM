using Server.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Server.Core
{
    public class ERPDb : IdentityDbContext<ApplicationUser, CustomRole, string>
    {

        private readonly ITenantResolve _tenantResolve;
        public ERPDb
        (
            DbContextOptions<ERPDb> dbContextOptions,
            ITenantResolve tenantResolve

        ) : base(dbContextOptions)
        {

            _tenantResolve = tenantResolve;


        }
        public virtual DbSet<PasswordResetDomain> PasswordResetDomains { get; set; }
        public virtual DbSet<CONTACTDETAILS> CONTACTDETAILs { get; set; }
        public virtual DbSet<EmergencyContacts> EmergencyContacts { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<CaseComment> CaseComments { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<Attachments> Attachments { get; set; }
        public virtual DbSet<HRNotes> HRNotes { get; set; }
        public virtual DbSet<NOTIFICATIONS> NOTIFICATIONs { get; set; }
        public virtual DbSet<CandidateInfo> CandidateInfos { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<JobExperience> JobExperiences { get; set; }
        public virtual DbSet<Personal> Personals { get; set; }
        public virtual DbSet<ProfessionalLicense> ProfessionalLicenses { get; set; }
        public virtual DbSet<GENERALTASK> GENERALTASKs { get; set; }
        public virtual DbSet<Dependent> Dependents { get; set; }
        public virtual DbSet<ZoomMeetings> ZoomMeetings { get; set; }
        public virtual DbSet<WebPages> WebPages { get; set; }
        public virtual DbSet<BlogPage> BlogPages { get; set; }
        public virtual DbSet<Trainings> Trainings { get; set; }
        public virtual DbSet<ContactPage> ContactPages { get; set; }

        public override int SaveChanges()
        {
            SetTenantId();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTenantId();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTenantId()
        {
            int TenantId = _tenantResolve.GetTenantId();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity.GetType().GetProperty("TenantId") != null)
                {
                    entry.Property("TenantId").CurrentValue = TenantId;
                }
            }
        }

        private int SetTenant()
        {
            return _tenantResolve.GetTenantId();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BlogPage>()
               .Property(w => w.Id)
               .ValueGeneratedOnAdd();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ContactPage>()
               .Property(w => w.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<WebPages>()
                .Property(w => w.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Trainings>()
               .Property(w => w.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<WebPages>()
                 .HasQueryFilter(x => x.TenantId == SetTenant());
            modelBuilder.Entity<ContactPage>()
                 .HasQueryFilter(x => x.TenantId == SetTenant());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {

                modelBuilder.Entity<CustomRole>(entity =>
                {
                    entity.Property(r => r.Permissions).HasMaxLength(255);
                });

                var idProperty = entityType.FindProperty("Id");
                if (idProperty != null && idProperty.ClrType == typeof(Guid))
                {

                    modelBuilder.Entity(entityType.Name)
                        .HasKey("Id")
                        .HasName($"PK_{entityType.Name}_Id");


                    modelBuilder.Entity(entityType.Name)
                        .Property<Guid>("Id")
                        .ValueGeneratedOnAdd();
                    modelBuilder.Entity<Asset>()
                   .Property(a => a.Price)
                   .HasPrecision(18, 2);
                }
            }



        }



    }

}