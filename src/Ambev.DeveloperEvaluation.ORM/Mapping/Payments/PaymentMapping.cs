using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Entities.Payments;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.Payments
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CardName)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(c => c.CardNumber)
                .IsRequired()
                .HasColumnType("varchar(16)");

            builder.Property(c => c.CardExpiration)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(c => c.CardCvv)
                .IsRequired()
                .HasColumnType("varchar(4)");

            // 1 : 1 => Payment : Transaction
            builder.HasOne(c => c.Transaction)
                .WithOne(c => c.Payment);

            builder.ToTable("Payments");
        }
    }
}
