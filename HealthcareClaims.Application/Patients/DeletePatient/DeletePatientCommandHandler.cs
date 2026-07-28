using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Patients.DeletePatient;

public class DeletePatientCommandHandler
    : IRequestHandler<DeletePatientCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePatientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeletePatientCommand request,
        CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(
                p => p.Id == request.Id,
                cancellationToken);

        if (patient is null)
        {
            throw new KeyNotFoundException("Patient not found.");
        }

        _context.Patients.Remove(patient);

        await _context.SaveChangesAsync(cancellationToken);
    }
}