using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.Catalog
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(p => p.Description)                
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Image)                
                .HasColumnType("varchar(250)");

            builder.OwnsOne(p => p.Dimensions, obj =>
            {
                obj.Property(p => p.Height)
                    .HasColumnName("Height")
                    .HasColumnType("double precision");

                obj.Property(p => p.Width)
                    .HasColumnName("Width")
                    .HasColumnType("double precision");

                obj.Property(p => p.Depth)
                    .HasColumnName("Depth")
                    .HasColumnType("double precision");
            });

            builder.OwnsOne(p => p.Rating, obj =>
            {
                obj.Property(p => p.Rate)
                    .HasColumnName("Rate")
                    .HasColumnType("double precision");

                obj.Property(p => p.Count)
                    .HasColumnName("Rating_Count")
                    .HasColumnType("smallint");
                
            });

            builder.ToTable("Products");
        }
    }
}
