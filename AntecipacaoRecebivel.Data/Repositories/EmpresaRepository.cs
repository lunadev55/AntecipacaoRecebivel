using AntecipacaoRecebivel.Application.Interfaces;
using AntecipacaoRecebivel.Data.Context;
using AntecipacaoRecebivel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntecipacaoRecebivel.Data.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly ApplicationDbContext _context;

        public EmpresaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Empresa company)
        {
            _context.Empresas.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task<Empresa?> GetById(Guid companyId)
        {
            return await _context.Empresas
                .Include(c => c.NotasFiscais)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }
    }
}
