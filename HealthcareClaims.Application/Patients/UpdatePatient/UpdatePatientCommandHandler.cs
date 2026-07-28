using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Patients.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Patients.UpdatePatient;

public class UpdatePatientCommandHandler
    : IRequestHandler<UpdatePatientCommand, PatientResponse?>
{
    private readonly IApplicationDbContext _context;

    public UpdatePatientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PatientResponse?> Handle(
        UpdatePatientCommand request,
        CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(
                p => p.Id == request.Id,
                cancellationToken);

        if (patient is null)
        {
            return null;
        }

        patient.FirstName = request.FirstName;
        patient.LastName = request.LastName;
        patient.DateOfBirth = request.DateOfBirth;
        patient.Gender = request.Gender;
        patient.Email = request.Email;
        patient.PhoneNumber = request.PhoneNumber;
        patient.Address = request.Address;

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