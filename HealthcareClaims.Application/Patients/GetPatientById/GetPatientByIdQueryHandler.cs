using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Patients.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Patients.GetPatientById;

public class GetPatientByIdQueryHandler
    : IRequestHandler<GetPatientByIdQuery, PatientResponse?>
{
    private readonly IApplicationDbContext _context;

    public GetPatientByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PatientResponse?> Handle(
        GetPatientByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Patients
            .Where(p => p.Id == request.Id)
            .Select(p => new PatientResponse
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Address = p.Address,
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}