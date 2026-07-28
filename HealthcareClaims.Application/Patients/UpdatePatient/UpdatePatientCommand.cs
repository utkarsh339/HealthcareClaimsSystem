using HealthcareClaims.Application.Patients.Common;
using HealthcareClaims.Domain.Enums;
using MediatR;

namespace HealthcareClaims.Application.Patients.UpdatePatient;

public sealed record UpdatePatientCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    Gender Gender,
    string Email,
    string PhoneNumber,
    string Address)
    : IRequest<PatientResponse?>;