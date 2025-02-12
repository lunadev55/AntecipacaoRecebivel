using AntecipacaoRecebivel.Application.Dtos;
using AntecipacaoRecebivel.Domain.Entities;
using AutoMapper;

namespace AntecipacaoRecebivel.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Empresa, EmpresaDto>().ReverseMap();
            CreateMap<NotaFiscal, NotaFiscalDto>().ReverseMap();
        }
    }
}