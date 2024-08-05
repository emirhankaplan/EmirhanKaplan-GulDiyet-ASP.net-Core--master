using Microsoft.EntityFrameworkCore;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Core.Domain.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GulDiyet.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Diyetisyen> Diyetisyens { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<LaboratoryTest> LaboratoryTests { get; set; }
        public DbSet<LaboratoryResult> LaboratoryResults { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<DietPlan> DietPlans { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API

            #region tables
            modelBuilder.Entity<Diyetisyen>().ToTable("Diyetisyens");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<LaboratoryTest>().ToTable("LaboratoryTests");
            modelBuilder.Entity<LaboratoryResult>().ToTable("LaboratoryResults");
            modelBuilder.Entity<Appointment>().ToTable("Appointments");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Email>().ToTable("Emails");
            modelBuilder.Entity<DietPlan>().ToTable("DietPlans");
            modelBuilder.Entity<TimeSlot>().ToTable("TimeSlots");
            modelBuilder.Entity<Evaluation>().ToTable("Evaluations");
            #endregion

            #region primary keys
            modelBuilder.Entity<Diyetisyen>().HasKey(d => d.Id);
            modelBuilder.Entity<Patient>().HasKey(p => p.Id);
            modelBuilder.Entity<LaboratoryTest>().HasKey(lt => lt.Id);
            modelBuilder.Entity<LaboratoryResult>().HasKey(lr => lr.Id);
            modelBuilder.Entity<Appointment>().HasKey(a => a.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Email>().HasKey(e => e.Id);
            modelBuilder.Entity<DietPlan>().HasKey(dp => dp.Id);
            modelBuilder.Entity<TimeSlot>().HasKey(ts => ts.Id);
            modelBuilder.Entity<Evaluation>().HasKey(e => e.Id);
            #endregion

            #region relationships

            // Evaluation -> TimeSlot ilişkisi
            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.TimeSlot)
                .WithMany(t => t.Evaluations)
                .HasForeignKey(e => e.TimeSlotId)
                .OnDelete(DeleteBehavior.NoAction);

            // Diyetisyen -> Appointment ilişkisi
            modelBuilder.Entity<Diyetisyen>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Diyetisyen)
                .HasForeignKey(a => a.DiyetisyenId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment -> Patient ilişkisi
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment -> LaboratoryResult ilişkisi
            modelBuilder.Entity<Appointment>()
                .HasMany(a => a.LaboratoryResults)
                .WithOne(lr => lr.Appointment)
                .HasForeignKey(lr => lr.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // LaboratoryTest -> LaboratoryResult ilişkisi
            modelBuilder.Entity<LaboratoryTest>()
                .HasMany(lt => lt.LaboratoryResults)
                .WithOne(lr => lr.LaboratoryTest)
                .HasForeignKey(lr => lr.LaboratoryTestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Diyetisyen -> DietPlan ilişkisi
            modelBuilder.Entity<Diyetisyen>()
                .HasMany(d => d.DietPlans)
                .WithOne(dp => dp.Diyetisyen)
                .HasForeignKey(dp => dp.DiyetisyenId)
                .OnDelete(DeleteBehavior.Cascade);

            // Patient -> DietPlan ilişkisi
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.DietPlans)
                .WithOne(dp => dp.Patient)
                .HasForeignKey(dp => dp.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Evaluation -> Appointment ilişkisi
            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Appointment)
                .WithMany(a => a.Evaluations)
                .HasForeignKey(e => e.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region property configuration

            #region Diyetisyen
            modelBuilder.Entity<Diyetisyen>().Property(d => d.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Diyetisyen>().Property(d => d.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Diyetisyen>().Property(d => d.Email).IsRequired();
            modelBuilder.Entity<Diyetisyen>().Property(d => d.Phone).IsRequired();
            modelBuilder.Entity<Diyetisyen>().Property(d => d.IdNumber).IsRequired();
            #endregion

            #region Patient
            modelBuilder.Entity<Patient>().Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Patient>().Property(p => p.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Patient>().Property(p => p.Phone).IsRequired();
            modelBuilder.Entity<Patient>().Property(p => p.Email).IsRequired();
            modelBuilder.Entity<Patient>().Property(p => p.IdNumber).IsRequired();
            modelBuilder.Entity<Patient>().Property(p => p.DateBirth).IsRequired();
            modelBuilder.Entity<Patient>().Property(p => p.IsSmoker).IsRequired();
            modelBuilder.Entity<Patient>().Property(p => p.HasAllergies).IsRequired();
            #endregion

            #region LaboratoryTest
            modelBuilder.Entity<LaboratoryTest>().Property(lt => lt.Name).IsRequired().HasMaxLength(100);
            #endregion

            #region Appointment
            modelBuilder.Entity<Appointment>().Property(a => a.Day).IsRequired();
            modelBuilder.Entity<Appointment>().Property(a => a.Time).IsRequired();
            modelBuilder.Entity<Appointment>().Property(a => a.Reason).IsRequired();
            modelBuilder.Entity<Appointment>().Property(a => a.Status).IsRequired();
            #endregion

            #region User
            modelBuilder.Entity<User>().Property(u => u.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Phone).IsRequired();
            #endregion

            #region Email
            modelBuilder.Entity<Email>().Property(e => e.To).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Email>().Property(e => e.Subject).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Email>().Property(e => e.Body).IsRequired();
            modelBuilder.Entity<Email>().Property(e => e.SentDate).IsRequired();
            #endregion

            #region DietPlan
            modelBuilder.Entity<DietPlan>().Property(dp => dp.PlanDetails).IsRequired();
            modelBuilder.Entity<DietPlan>().Property(dp => dp.CreatedDate).IsRequired();
            #endregion

            #endregion
        }
    }
}
