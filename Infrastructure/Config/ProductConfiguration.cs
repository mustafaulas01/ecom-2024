using System;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(a=>a.Price).HasColumnType("decimal(18,2)");
        builder.Property(a=>a.Name).HasMaxLength(250);
        builder.Property(a=>a.PictureUrl).HasMaxLength(150);
        builder.Property(a=>a.Brand).HasMaxLength(100);

        builder.Property(a=>a.Type).HasMaxLength(100);

    }
}
