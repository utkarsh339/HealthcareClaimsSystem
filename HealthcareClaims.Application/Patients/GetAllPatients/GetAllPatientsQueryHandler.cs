using HealthcareClaims.Application.Common.Interfaces;
using HealthcareClaims.Application.Patients.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthcareClaims.Application.Patients.GetAllPatients;

public class GetAllPatientsQueryHandler
    : IRequestHandler<GetAllPatientsQuery, IReadOnlyList<PatientResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAllPatientsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PatientResponse>> Handle(
        GetAllPatientsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Patients
            .AsNoTracking()
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
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
            .ToListAsync(cancellationToken);
    }
}