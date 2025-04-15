using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Entities.Payments;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.Payments
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CardName)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(p => p.CardNumber)
                .IsRequired()
                .HasColumnType("varchar(16)");

            builder.Property(p => p.CardExpiration)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(p => p.CardCvv)
                .IsRequired()
                .HasColumnType("varchar(4)");

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // 1 : 1 => Payment : Transaction
            builder.HasOne(c => c.Transaction)
                .WithOne(c => c.Payment);

            builder.ToTable("Payments");
        }
    }
}
