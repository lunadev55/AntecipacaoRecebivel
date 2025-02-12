using AntecipacaoRecebivel.Application.Dtos;

namespace AntecipacaoRecebivel.Application.Interfaces
{
    public interface INotaFiscalService
    {
        Task<Guid> CreateNotaFiscal(NotaFiscalDto notaFiscalDto);
        Task<List<NotaFiscalDto>> GetNotasFiscaisByEmpresaId(Guid empresaId);
        Task<NotaFiscalDto> GetNotaFiscalByNumero(int id);
    }
}
