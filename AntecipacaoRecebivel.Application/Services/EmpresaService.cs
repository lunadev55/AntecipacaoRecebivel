using AntecipacaoRecebivel.Application.Dtos;
using AntecipacaoRecebivel.Application.Interfaces;
using AntecipacaoRecebivel.Domain.Entities;
using AutoMapper;

namespace AntecipacaoRecebivel.Application.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IMapper _mapper;

        public EmpresaService(IEmpresaRepository empresaRepository, IMapper mapper)
        {
            _empresaRepository = empresaRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateEmpresa(EmpresaDto empresaDto)
        {
            var empresa = _mapper.Map<Empresa>(empresaDto);
            await _empresaRepository.Add(empresa);
            return empresa.Id;
        }

        public async Task<EmpresaDto?> GetEmpresaById(Guid empresaId)
        {
            var empresa = await _empresaRepository.GetById(empresaId);
            return _mapper.Map<EmpresaDto>(empresa);
        }
    }
}