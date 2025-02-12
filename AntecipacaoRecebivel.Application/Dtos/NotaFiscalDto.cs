namespace AntecipacaoRecebivel.Application.Dtos
{
    public class NotaFiscalDto
    {
        public int Numero { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public Guid EmpresaId { get; set; }
    }
}