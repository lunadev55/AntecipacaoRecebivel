using AntecipacaoRecebivel.Application.Dtos;

namespace AntecipacaoRecebivel.Application.Interfaces
{
    public interface IAntecipacaoRecebivelService
    {
        Task<CarrinhoDto> CalcularAntecipacao(CalculoCarrinhoRequisicaoDto request);
    }
}