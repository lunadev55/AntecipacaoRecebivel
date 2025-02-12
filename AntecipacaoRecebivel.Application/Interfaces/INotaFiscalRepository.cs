using AntecipacaoRecebivel.Domain.Entities;

namespace AntecipacaoRecebivel.Application.Interfaces
{
    public interface INotaFiscalRepository
    {
        Task Create(NotaFiscal notaFiscal);
        Task<List<NotaFiscal>> GetByEmpresaId(Guid empresaId);
        Task<List<NotaFiscal>> GetByIdsAsync(List<Guid> notasFiscaisIds);
        Task<NotaFiscal> GetByNumero(int numero);
    }
}
