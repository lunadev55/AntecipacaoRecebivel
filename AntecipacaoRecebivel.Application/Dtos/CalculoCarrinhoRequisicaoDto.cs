namespace AntecipacaoRecebivel.Application.Dtos
{
    public class CalculoCarrinhoRequisicaoDto
    {
        public Guid EmpresaId { get; set; }
        public List<Guid> NotasFiscaisIds { get; set; } = new();
    }
}
