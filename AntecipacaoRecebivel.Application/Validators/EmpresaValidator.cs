using AntecipacaoRecebivel.Application.Dtos;
using FluentValidation;

namespace AntecipacaoRecebivel.Application.Validators
{
    public class EmpresaValidator : AbstractValidator<EmpresaDto>
    {
        public EmpresaValidator()
        {
            RuleFor(c => c.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Length(14).WithMessage("O CNPJ deve ter 14 caracteres.");

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.");

            RuleFor(c => c.FaturamentoMensal)
                .GreaterThan(0).WithMessage("O faturamento mensal deve ser maior que zero.");

            RuleFor(c => c.RamoAtuacao)
                .Must(x => x == "Servicos" || x == "Produtos")
                .WithMessage("O ramo deve ser 'Servicos' ou 'Produtos'.");
        }
    }
}