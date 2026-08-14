using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Patients.Common;
using HealthcareClaims.Domain.Entities;
using MediatR;
using HealthcareClaims.Domain.Enums;

namespace HealthcareClaims.Application.Patients.CreatePatient;

public class CreatePatientCommandHandler
    : IRequestHandler<CreatePatientCommand, PatientResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public CreatePatientCommandHandler(
        IApplicationDbContext context,
        IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<PatientResponse> Handle(
        CreatePatientCommand request,
        CancellationToken cancellationToken)
    {
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            CreatedAt = DateTime.UtcNow
        };

        _context.Patients.Add(patient);

        await _auditService.LogAsync(
            AuditAction.Create,
            "Patient",
            patient.Id,
            $"Created patient {patient.Id}",
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new PatientResponse
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            Email = patient.Email,
            PhoneNumber = patient.PhoneNumber,
            Address = patient.Address,
            CreatedAt = patient.CreatedAt
        };
    }
}
