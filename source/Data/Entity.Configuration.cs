using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Product.WebApi.Data;

#pragma warning disable IDE0058

public sealed class EntityConfiguration : IEntityTypeConfiguration<Entity>
{
    public void Configure(EntityTypeBuilder<Entity> builder)
    {
        builder.ToTable("entity");

        builder
            .HasKey(a => a.Id)
            .HasName("entity_pk");

        builder
            .Property(a => a.Id)
            .ValueGeneratedNever();
    }
}
