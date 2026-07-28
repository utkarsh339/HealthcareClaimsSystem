using MediatR;

namespace HealthcareClaims.Application.Patients.DeletePatient;

public sealed record DeletePatientCommand(Guid Id) : IRequest;