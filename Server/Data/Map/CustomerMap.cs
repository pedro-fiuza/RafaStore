using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RafaStore.Shared.Model;

namespace RafaStore.Server.Data.Map
{
    public class CustomerMap : IEntityTypeConfiguration<CustomerModel>
    {
        public void Configure(EntityTypeBuilder<CustomerModel> builder)
        {
            builder.ToTable("Customer");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("nvarchar")
                .HasMaxLength(255);

            builder.Property(x => x.CpfOrCnpj)
                .IsRequired()
                .HasColumnName("CpfOrCnpj")
                .HasColumnType("varchar")
                .HasMaxLength(40);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasColumnName("Address")
                .HasColumnType("nvarchar")
                .HasMaxLength(255);

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt")
                .HasColumnType("smalldatetime");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasColumnName("UpdatedAt")
                .HasColumnType("smalldatetime");

        }
    }
}
