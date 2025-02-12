using AntecipacaoRecebivel.Application.Dtos;
using AntecipacaoRecebivel.Application.Interfaces;
using AntecipacaoRecebivel.Domain.Entities;
using AutoMapper;

namespace AntecipacaoRecebivel.Application.Services
{
    public class NotaFiscalService : INotaFiscalService
    {
        private readonly INotaFiscalRepository _notaFiscalRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IMapper _mapper;

        public NotaFiscalService(INotaFiscalRepository notaFiscalRepository, IEmpresaRepository empresaRepository, IMapper mapper)
        {
            _notaFiscalRepository = notaFiscalRepository;
            _empresaRepository = empresaRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateNotaFiscal(NotaFiscalDto notaFiscalDto)
        {
            var empresa = await _empresaRepository.GetById(notaFiscalDto.EmpresaId);
            if (empresa == null)
                throw new Exception("Empresa não encontrada.");

            var notaFiscal = _mapper.Map<NotaFiscal>(notaFiscalDto);
            await _notaFiscalRepository.Create(notaFiscal);
            return notaFiscal.Id;
        }

        public async Task<List<NotaFiscalDto>> GetNotasFiscaisByEmpresaId(Guid empresaId)
        {
            var notasFiscais = await _notaFiscalRepository.GetByEmpresaId(empresaId);
            return _mapper.Map<List<NotaFiscalDto>>(notasFiscais);
        }

        public async Task<NotaFiscalDto> GetNotaFiscalByNumero(int numero)
        {
            var notaFiscal = await _notaFiscalRepository.GetByNumero(numero);
            return _mapper.Map<NotaFiscalDto>(notaFiscal);
        }
    }
}
