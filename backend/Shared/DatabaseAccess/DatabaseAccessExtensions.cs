using Light.SharedCore.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shared.DatabaseAccess;

public static class DatabaseAccessExtensions
{
    public static void ConfigureIdempotentId<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : GuidEntity =>
        builder.Property(x => x.Id).ValueGeneratedNever();
}