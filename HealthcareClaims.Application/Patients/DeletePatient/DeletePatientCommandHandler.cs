using HealthcareClaims.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Patients.DeletePatient;

public class DeletePatientCommandHandler
    : IRequestHandler<DeletePatientCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public DeletePatientCommandHandler(IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
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

        await _auditService.LogAsync(
            AuditAction.Delete,
            "Patient",
            patient.Id,
            $"Deleted patient {patient.Id}",
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
