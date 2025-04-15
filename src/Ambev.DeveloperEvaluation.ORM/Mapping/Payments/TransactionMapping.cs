using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Entities.Payments;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.Payments
{
    public class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Total)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // 1 : 1 => Transaction : Payment
            builder.HasOne(c => c.Payment)
                .WithOne(c => c.Transaction);

            builder.ToTable("Transactions");
        }
    }
}
