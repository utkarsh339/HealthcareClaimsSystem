using HealthcareClaims.Application.Patients.Common;
using MediatR;

namespace HealthcareClaims.Application.Patients.GetPatientById;

public sealed record GetPatientByIdQuery(Guid Id)
    : IRequest<PatientResponse?>;