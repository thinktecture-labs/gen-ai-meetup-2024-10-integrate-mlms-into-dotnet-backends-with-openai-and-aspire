using AiInformationExtractionApi.DatabaseAccess.Model;
using Microsoft.EntityFrameworkCore;
using Shared.DatabaseAccess;

namespace AiInformationExtractionApi.DatabaseAccess;

public sealed class AiInformationExtractionDbContext : DbContext
{
    public AiInformationExtractionDbContext(DbContextOptions<AiInformationExtractionDbContext> options) :
        base(options) { }

    public DbSet<MediaItem> MediaItems => Set<MediaItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>         modelBuilder.Entity<MediaItem>(
        options =>
        {
            options.ConfigureIdempotentId();
            options.Property(mi => mi.Name).IsRequired().HasMaxLength(200);
            options.Property(mi => mi.MimeType).IsRequired().HasMaxLength(100);
        }
    );
}