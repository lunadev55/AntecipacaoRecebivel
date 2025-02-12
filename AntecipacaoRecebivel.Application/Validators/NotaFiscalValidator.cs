using AntecipacaoRecebivel.Application.Dtos;
using FluentValidation;

namespace AntecipacaoRecebivel.Application.Validators
{
    public class NotaFiscalValidator : AbstractValidator<NotaFiscalDto>
    {
        public NotaFiscalValidator()
        {
            RuleFor(i => i.Numero)
                .GreaterThan(0)
                .WithMessage("O número da NF deve ser maior que zero.");

            RuleFor(i => i.Valor)
                .GreaterThan(0)
                .WithMessage("O valor da NF deve ser maior que zero.");

            RuleFor(i => i.DataVencimento)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("A data de vencimento deve ser maior que a data atual.");            
        }
    }
}
