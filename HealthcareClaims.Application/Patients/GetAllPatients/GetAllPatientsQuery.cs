using HealthcareClaims.Application.Patients.Common;
using MediatR;

namespace HealthcareClaims.Application.Patients.GetAllPatients;

public sealed record GetAllPatientsQuery
    : IRequest<IReadOnlyList<PatientResponse>>;