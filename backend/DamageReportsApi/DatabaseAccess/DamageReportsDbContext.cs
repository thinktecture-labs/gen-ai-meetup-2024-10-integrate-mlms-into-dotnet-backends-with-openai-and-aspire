using DamageReportsApi.DatabaseAccess.Model;
using Microsoft.EntityFrameworkCore;
using Shared.DatabaseAccess;

namespace DamageReportsApi.DatabaseAccess;

public sealed class DamageReportsDbContext : DbContext
{
    public DamageReportsDbContext(DbContextOptions<DamageReportsDbContext> options) : base(options) { }

    public DbSet<DamageReport> DamageReports => Set<DamageReport>();
    public DbSet<Passenger> Passengers => Set<Passenger>();
    public DbSet<OtherPartyContact> OtherPartyContacts => Set<OtherPartyContact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DamageReport>(
            report =>
            {
                report.ConfigureIdempotentId();
                report
                   .HasMany(dr => dr.Passengers)
                   .WithOne(r => r.DamageReport)
                   .HasForeignKey(p => p.DamageReportId)
                   .IsRequired();

                report
                   .HasOne(dr => dr.OtherPartyContact)
                   .WithOne(c => c.DamageReport)
                   .HasForeignKey<OtherPartyContact>(c => c.DamageReportId)
                   .IsRequired();

                report.Property(dr => dr.FirstName).HasMaxLength(200);
                report.Property(dr => dr.LastName).HasMaxLength(200);
                report.Property(dr => dr.Street).HasMaxLength(200);
                report.Property(dr => dr.ZipCode).HasMaxLength(10);
                report.Property(dr => dr.Location).HasMaxLength(200);
                report.Property(dr => dr.InsuranceId).HasMaxLength(30);
                report.Property(dr => dr.Telephone).HasMaxLength(30);
                report.Property(dr => dr.Email).HasMaxLength(100);
                report.Property(dr => dr.LicensePlate).HasMaxLength(20);
                report.Property(dr => dr.LicensePlate).HasMaxLength(20);
                report.Property(dr => dr.CountryCode).HasMaxLength(2);
                report.Property(dr => dr.ReasonOfTravel).HasMaxLength(200);
                report.Property(dr => dr.CarType).HasMaxLength(50);
                report.Property(dr => dr.CarColor).HasMaxLength(50);
            }
        );

        ConfigurePerson<Passenger>(modelBuilder);
        ConfigurePerson<OtherPartyContact>(modelBuilder);
    }

    private static void ConfigurePerson<TEntity>(ModelBuilder modelBuilder)
        where TEntity : Person =>
        modelBuilder.Entity<TEntity>(
            person =>
            {
                person.ConfigureIdempotentId();
                person.Property(p => p.FirstName).HasMaxLength(200);
                person.Property(p => p.LastName).HasMaxLength(200);
            }
        );
}