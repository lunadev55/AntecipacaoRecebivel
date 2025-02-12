using AntecipacaoRecebivel.Application.Interfaces;
using AntecipacaoRecebivel.Data.Context;
using AntecipacaoRecebivel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntecipacaoRecebivel.Data.Repositories
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        private readonly ApplicationDbContext _context;

        public NotaFiscalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(NotaFiscal notaFiscal)
        {
            _context.NotasFiscais.Add(notaFiscal);
            await _context.SaveChangesAsync();
        }

        public async Task<List<NotaFiscal>> GetByEmpresaId(Guid empresaId)
        {
            return await _context.NotasFiscais
                .Where(i => i.EmpresaId == empresaId)
                .ToListAsync();
        }

        public async Task<List<NotaFiscal>> GetByIdsAsync(List<Guid> notasFiscaisIds)
        {
            return await _context.NotasFiscais
                .Where(notaFiscal => notasFiscaisIds.Contains(notaFiscal.Id))
                .ToListAsync();
        }

        public async Task<NotaFiscal> GetByNumero(int numero)
        {
            return await _context.NotasFiscais
                 .Where(notaFiscal => notaFiscal.Numero == numero)
                 .FirstOrDefaultAsync();
        }
    }
}