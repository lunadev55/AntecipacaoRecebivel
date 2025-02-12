using AntecipacaoRecebivel.Domain.Entities;

namespace AntecipacaoRecebivel.Application.Interfaces
{
    public interface IEmpresaRepository
    {
        Task Add(Empresa empresa);
        Task<Empresa?> GetById(Guid empresaId);
    }
}
