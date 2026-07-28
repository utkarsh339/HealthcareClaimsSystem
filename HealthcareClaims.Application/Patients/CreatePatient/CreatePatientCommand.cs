using HealthcareClaims.Application.Patients.Common;
using HealthcareClaims.Domain.Enums;
using MediatR;

namespace HealthcareClaims.Application.Patients.CreatePatient;

public record CreatePatientCommand(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    Gender Gender,
    string Email,
    string PhoneNumber,
    string Address
) : IRequest<PatientResponse>;