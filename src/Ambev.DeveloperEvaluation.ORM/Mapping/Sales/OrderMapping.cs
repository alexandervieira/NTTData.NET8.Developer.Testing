using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.Sales
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Code)
                .HasDefaultValueSql("nextval('\"MySequence\"')");

            builder.Property(o => o.CustomerId)
                .IsRequired();

            builder.Property(o => o.IsVoucherUsed)
                .IsRequired();

            builder.Property(o => o.Discount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.TotalValue)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            // 1 : N => Order : OrderItems
            builder.HasMany(c => c.OrderItems)
                .WithOne(c => c.Order)
                .HasForeignKey(c => c.OrderId);

            builder.ToTable("Orders");
        }
    }
}
