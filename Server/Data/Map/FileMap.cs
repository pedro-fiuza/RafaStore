using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RafaStore.Shared.Model;

namespace RafaStore.Server.Data.Map;

public class FileMap : IEntityTypeConfiguration<NoteFileModel>
{
    public void Configure(EntityTypeBuilder<NoteFileModel> builder)
    {
        builder.ToTable("NoteFileModel");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasColumnName("FileName")
            .HasColumnType("nvarchar")
            .HasMaxLength(255);

        builder.Property(x => x.Blob)
            .IsRequired()
            .HasColumnName("Blob")
            .HasColumnType("varchar")
            .HasMaxLength(40);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("CreatedAt")
            .HasColumnType("smalldatetime");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnName("UpdatedAt")
            .HasColumnType("smalldatetime");

        builder.HasOne(x => x.CustomerModel)
            .WithMany(x => x.File)
            .HasConstraintName("FK_Customer_File")
            .OnDelete(DeleteBehavior.NoAction);
    }
}