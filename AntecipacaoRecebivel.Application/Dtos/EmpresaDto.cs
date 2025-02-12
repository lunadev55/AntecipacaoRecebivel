namespace AntecipacaoRecebivel.Application.Dtos
{
    public class EmpresaDto
    {
        public string Cnpj { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public decimal FaturamentoMensal { get; set; }
        public string RamoAtuacao { get; set; } = string.Empty;
    }
}