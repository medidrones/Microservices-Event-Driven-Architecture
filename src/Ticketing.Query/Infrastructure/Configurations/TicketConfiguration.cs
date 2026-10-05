using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Query.Domain.Tickets;
using Ticketing.Query.Domain.TicketTypes;

namespace Ticketing.Query.Infrastructure.Configurations;

public class TicketEmployeeConfiguration : IEntityTypeConfiguration<TicketEmployee>
{
    public void Configure(EntityTypeBuilder<TicketEmployee> builder)
    {
        builder.ToTable("ticket_employees");
    }
}

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.TicketType).HasConversion(
            ticketType => ticketType!.Id,
            value => TicketType.Create(value));

        builder
            .HasMany(t => t.Employees)
            .WithMany(t => t.Tickets)
            .UsingEntity<TicketEmployee>(
                j => j
                    .HasOne(p => p.Employee)
                    .WithMany(p => p.TicketEmployees)
                    .HasForeignKey(p => p.EmployeeId),
                j => j
                    .HasOne(p => p.Ticket)
                    .WithMany(p => p.TicketEmployees)
                    .HasForeignKey(p => p.TicketId),
                j =>
                {
                    j.HasKey(t => new { t.TicketId, t.EmployeeId });
                });
    }
}