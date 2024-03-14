using IT_DeskServer.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IT_DeskServer.DataAccess.Configurations;

public sealed class TicketDetailConfiguration : IEntityTypeConfiguration<TicketDetail>
{
    public void Configure(EntityTypeBuilder<TicketDetail> builder)
    {
        builder.ToTable("TicketDetails");
    }
}