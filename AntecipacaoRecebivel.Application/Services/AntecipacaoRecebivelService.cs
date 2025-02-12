using AntecipacaoRecebivel.Application.Dtos;
using AntecipacaoRecebivel.Application.Interfaces;
using AntecipacaoRecebivel.Domain.Entities;

namespace AntecipacaoRecebivel.Application.Services
{
    public class AntecipacaoRecebivelService : IAntecipacaoRecebivelService
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly INotaFiscalRepository _notaFiscalRepository;

        public AntecipacaoRecebivelService(IEmpresaRepository empresaRepository, INotaFiscalRepository notaFiscalRepository)
        {
            _empresaRepository = empresaRepository;
            _notaFiscalRepository = notaFiscalRepository;
        }

        public async Task<CarrinhoDto> CalcularAntecipacao(CalculoCarrinhoRequisicaoDto request)
        {
            var empresa = await _empresaRepository.GetById(request.EmpresaId);
            if (empresa == null)
                throw new Exception("Empresa não encontrada.");

            var notasFiscais = await _notaFiscalRepository.GetByIdsAsync(request.NotasFiscaisIds);
            if (notasFiscais.Count == 0)
                throw new Exception("Nenhuma nota fiscal encontrada.");

            var limiteCredito = empresa.GetLimiteDeCredito();
           
            var totalBruto = notasFiscais.Sum(nf => nf.Valor);
            if (totalBruto > limiteCredito)
                throw new Exception($"O valor total das notas fiscais ({totalBruto:C}) excede o limite de crédito da empresa ({limiteCredito:C}).");

            var carrinho = new CarrinhoDto()
            {
                Empresa = empresa.Nome,
                Cnpj = empresa.Cnpj,
                Limite = limiteCredito
            };

            foreach (var notaFiscal in notasFiscais)
            {
                var valorLiquido = CalcularValorLiquido(notaFiscal);
                carrinho.NotasFiscais.Add(new AntecipacaoNotaFiscalDto()
                {
                    Numero = notaFiscal.Numero,
                    ValorBruto = notaFiscal.Valor,
                    ValorLiquido = valorLiquido
                });
            }

            return carrinho;
        }      

        private decimal CalcularValorLiquido(NotaFiscal notaFiscal)
        {
            var prazo = (notaFiscal.DataVencimento - DateTime.UtcNow.Date).Days;
            decimal taxaMensal = 4.65m / 100;
            decimal desagio = notaFiscal.Valor / (decimal)Math.Pow(1 + (double)taxaMensal, prazo / 30.0);
            
            decimal valorLiquido = notaFiscal.Valor - desagio;
            return Math.Round(valorLiquido, 2); 
        }       
    }
}
