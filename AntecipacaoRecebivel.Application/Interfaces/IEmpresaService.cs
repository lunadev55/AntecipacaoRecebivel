using AntecipacaoRecebivel.Application.Dtos;

namespace AntecipacaoRecebivel.Application.Interfaces
{
    public interface IEmpresaService
    {
        Task<Guid> CreateEmpresa(EmpresaDto empresaDto);
        Task<EmpresaDto?> GetEmpresaById(Guid empresaId);
    }
}