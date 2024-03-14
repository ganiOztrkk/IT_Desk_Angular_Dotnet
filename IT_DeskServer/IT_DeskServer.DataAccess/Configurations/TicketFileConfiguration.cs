using IT_DeskServer.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IT_DeskServer.DataAccess.Configurations;

public sealed class TicketFileConfiguration : IEntityTypeConfiguration<TicketFile>
{
    public void Configure(EntityTypeBuilder<TicketFile> builder)
    {
        builder.ToTable("TicketFiles");
    }
}