namespace AntecipacaoRecebivel.Application.Dtos
{
    public class CarrinhoDto
    {
        public string Empresa { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public decimal Limite { get; set; }
        public List<AntecipacaoNotaFiscalDto> NotasFiscais { get; set; } = new();
        public decimal TotalBruto => CalcularTotalBruto();
        public decimal TotalLiquido => CalcularTotalLiquido();

        private decimal CalcularTotalBruto()
        {
            decimal total = 0;
            foreach (var nota in NotasFiscais)
            {
                total += nota.ValorBruto;
            }
            return total;
        }

        private decimal CalcularTotalLiquido()
        {
            decimal total = 0;
            foreach (var nota in NotasFiscais)
            {
                total += nota.ValorLiquido;
            }
            return total;
        }
    }

    public class AntecipacaoNotaFiscalDto
    {
        public int Numero { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal ValorLiquido { get; set; }
    }
}