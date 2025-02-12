namespace AntecipacaoRecebivel.Domain.Entities
{
    public class Empresa
    {
        public Guid Id { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public decimal FaturamentoMensal { get; set; }
        public RamoAtuacao RamoAtuacao { get; set; }
        public ICollection<NotaFiscal> NotasFiscais { get; set; } = new List<NotaFiscal>(); 

        public decimal GetLimiteDeCredito()
        {
            if (FaturamentoMensal <= 50000)
                return FaturamentoMensal * 0.50m;
            if (FaturamentoMensal <= 100000)
                return RamoAtuacao == RamoAtuacao.Servicos ? FaturamentoMensal * 0.55m : FaturamentoMensal * 0.60m;

            return RamoAtuacao == RamoAtuacao.Servicos ? FaturamentoMensal * 0.60m : FaturamentoMensal * 0.65m;
        }
    }

    public enum RamoAtuacao
    {
        Servicos,
        Produtos
    }
}