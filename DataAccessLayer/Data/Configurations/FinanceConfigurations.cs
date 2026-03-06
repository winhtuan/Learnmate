using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

// ─── payments ─────────────────────────────────────────────────────────────────
internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasIndex(p => p.StudentId);
        builder.HasIndex(p => p.ClassId);
        builder.HasIndex(p => p.InvoiceId);

        builder.Property(p => p.Type).HasConversion<string>();
        builder.Property(p => p.Status)
               .HasConversion<string>()
               .HasDefaultValue(PaymentStatus.PENDING);

        builder.Property(p => p.Amount).HasColumnType("numeric(12,2)");

        builder.ToTable(t =>
            t.HasCheckConstraint("ck_payments_amount", "amount > 0"));

        builder.HasOne(p => p.Student)
               .WithMany(u => u.Payments)
               .HasForeignKey(p => p.StudentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Class)
               .WithMany(c => c.Payments)
               .HasForeignKey(p => p.ClassId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Invoice)
               .WithMany(i => i.Payments)
               .HasForeignKey(p => p.InvoiceId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}

// ─── invoices ─────────────────────────────────────────────────────────────────
internal sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasIndex(i => i.TeacherId);
        builder.HasIndex(i => i.ClassId);

        builder.Property(i => i.Status)
               .HasConversion<string>()
               .HasDefaultValue(InvoiceStatus.PENDING);

        builder.Property(i => i.TotalAmount).HasColumnType("numeric(12,2)");

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_invoices_total_amount", "total_amount > 0");
            t.HasCheckConstraint("ck_invoices_period", "period_start < period_end");
        });

        builder.HasOne(i => i.Teacher)
               .WithMany(u => u.Invoices)
               .HasForeignKey(i => i.TeacherId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Class)
               .WithMany(c => c.Invoices)
               .HasForeignKey(i => i.ClassId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
