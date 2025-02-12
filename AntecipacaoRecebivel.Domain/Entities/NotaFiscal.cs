namespace AntecipacaoRecebivel.Domain.Entities
{
    public class NotaFiscal
    {
        private const decimal Taxa = 0.0465m; // 4.65% ao mês

        public Guid Id { get; set; }
        public int Numero { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        public decimal GetDesagio()
        {
            int diasAteVencimento = (DataVencimento - DateTime.UtcNow.Date).Days;
            if (diasAteVencimento <= 0) return Valor;

            decimal fatorDeDesconto = (decimal)Math.Pow((double)(1 + Taxa), diasAteVencimento / 30.0);
            decimal desconto = Valor / fatorDeDesconto;
            return Valor - desconto;
        }
    }
}
