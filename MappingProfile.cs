using AutoMapper;
using WebApplication2.Entities;
using WebApplication4.Dtos.Cliente;
using WebApplication4.Dtos.Desconto;
using WebApplication4.Dtos.Estoque;
using WebApplication4.Dtos.Filial;
using WebApplication4.Dtos.Funcionario;
using WebApplication4.Dtos.HistoricoVenda;
using WebApplication4.Dtos.Industria;
using WebApplication4.Dtos.Medicamento;
using WebApplication4.Dtos.Venda;
using WebApplication4.Entities;

namespace WebApplication4
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Desconto, ReadDescontoDto>()
                .ForMember(dest => dest.NomeMedicamento, opt => opt.MapFrom(src => src.Medicamento.Nome));
            CreateMap<DescontoCreateDTO, Desconto>();
            CreateMap<DescontoUpdateDTO, Desconto>();

            CreateMap<Cliente, ReadClienteDto>();
            CreateMap<CreateClienteDto, Cliente>();
            CreateMap<UpdateClienteDto, Cliente>();

            CreateMap<Estoque, ReadEstoqueDto>()
                .ForMember(dest => dest.NomeFilial, opt => opt.MapFrom(src => src.Filial.Nome));
            CreateMap<CreateEstoqueDto, Estoque>();
            CreateMap<UpdateEstoqueDto, Estoque>();

            CreateMap<Filial, ReadFilialDto>();
            CreateMap<CreateFilialDto, Filial>();
            CreateMap<UpdateFilialDto, Filial>();

            CreateMap<Funcionario, ReadFuncionarioDto>();
            CreateMap<CreateFuncionarioDto, Funcionario>();
            CreateMap<UpdateFuncionarioDto, Funcionario>();
            CreateMap<Funcionario, UpdateFuncionarioDto>(); 

            CreateMap<Industria, ReadIndustriaDto>();
            CreateMap<CreateIndustriaDto, Industria>();
            CreateMap<UpdateIndustriaDto, Industria>();

            CreateMap<Medicamento, ReadMedicamentoDto>()
                .ForMember(dest => dest.NomeIndustria, opt => opt.MapFrom(src => src.Industria.Nome))
                .ForMember(dest => dest.NomeFilial, opt => opt.MapFrom(src => src.Estoque.Filial.Nome));
            CreateMap<CreateMedicamentoDto, Medicamento>();
            CreateMap<UpdateMedicamentoDto, Medicamento>();

            CreateMap<HistoricoVenda, ReadHistoricoVendaDto>()
                .ForMember(dest => dest.FuncionarioNome,
                           opt => opt.MapFrom(src => src.Funcionario.Nome));
            CreateMap<CreateHistoricoVendaDto, HistoricoVenda>();
            CreateMap<UpdateHistoricoVendaDto, HistoricoVenda>();

            CreateMap<Venda, ReadVendaDto>()
                .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.MedicamentoNome, opt => opt.MapFrom(src => src.Medicamento.Nome));
            CreateMap<CreateVendaDto, Venda>();
            CreateMap<UpdateVendaDto, Venda>();
            CreateMap<Venda, UpdateVendaDto>();
        }
    }
}
