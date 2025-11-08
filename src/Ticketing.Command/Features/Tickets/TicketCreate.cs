using FluentValidation;
using MediatR;
using Ticketing.Command.Application.Aggregates;
using Ticketing.Command.Domain.Abstracts;
using Ticketing.Command.Features.Apis;

namespace Ticketing.Command.Features.Tickets;

public sealed class TicketCreate : IMinimalApi
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/api/ticket", async (TicketCreateRequest ticketCreateRequest, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var id = Guid.CreateVersion7(DateTimeOffset.UtcNow).ToString();
            var command = new TicketCreateCommand(id, ticketCreateRequest);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(result);
        });
    }

    public sealed class TicketCreateRequest(string username, int typeError, string detailError)
    {
        public string Username { get; } = username;
        public int TypeError { get; } = typeError;
        public string DetailError { get; } = detailError;
    }   

    public record TicketCreateCommand(string Id, TicketCreateRequest ticketCreateRequest) : IRequest<bool>;

    public class TicketCreateCommandValidator : AbstractValidator<TicketCreateCommand>
    {
        public TicketCreateCommandValidator()
        {
            RuleFor(x => x.ticketCreateRequest)
                .SetValidator(new TicketCreateValidator());

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("El Id del ticket no puede estar vacío");
        }
    }

    public class TicketCreateValidator : AbstractValidator<TicketCreateRequest>
    {
        public TicketCreateValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                    .WithMessage("Ingrese un username")
                .EmailAddress()
                    .WithMessage("Debe ser un email");

            RuleFor(x => x.TypeError)
                .NotEmpty()
                    .WithMessage("Deve existir el tipo de error")
                .InclusiveBetween(1,5)
                    .WithMessage("El rango del error es de 1 a 5");

            RuleFor(x => x.DetailError)
                .NotEmpty()
                .WithMessage("Ingrese el detalle del error");
        }
    }

    public sealed class TicketCreateCommandHandler(IEventSourcingHandler<TicketAggregate> eventSourcingHandler)
        : IRequestHandler<TicketCreateCommand, bool>
    {
        private readonly IEventSourcingHandler<TicketAggregate> _eventSourcingHandler = eventSourcingHandler;

        public async Task<bool> Handle(TicketCreateCommand request, CancellationToken cancellationToken)
        {
            var aggregate = new TicketAggregate(request);
            await _eventSourcingHandler.SaveAsync(aggregate, cancellationToken);

            return true;
        }
    }    
}
