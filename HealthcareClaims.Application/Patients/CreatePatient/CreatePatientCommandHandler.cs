using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Patients.Common;
using HealthcareClaims.Domain.Entities;
using MediatR;

namespace HealthcareClaims.Application.Patients.CreatePatient;

public class CreatePatientCommandHandler
    : IRequestHandler<CreatePatientCommand, PatientResponse>
{
    private readonly IApplicationDbContext _context;

    public CreatePatientCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
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