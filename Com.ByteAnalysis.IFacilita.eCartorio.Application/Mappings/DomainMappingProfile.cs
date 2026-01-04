using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Mapping
{
    public class DomainMappingProfile : AutoMapper.Profile
    {
        public DomainMappingProfile()
        {
            Mapped();
        }

        public void Mapped()
        {

            CreateMap<PropertyDetailsInput, PropertyDetailsInputDto>()
              .ForMember(x => x.Cep, x => x.MapFrom(y => y.CEP))
              .ForMember(x => x.City, x => x.MapFrom(y => y.Municipio))
              .ForMember(x => x.Complements, x => x.MapFrom(y => y.ListaComplementos))
              .ForMember(x => x.Neighborhood, x => x.MapFrom(y => y.Bairro))
              .ForMember(x => x.Number, x => x.MapFrom(y => y.Numero))
              .ForMember(x => x.Registration, x => x.MapFrom(y => y.Matricula))
              .ForMember(x => x.Street, x => x.MapFrom(y => y.Logradouro))
              .ForMember(x => x.StreetType, x => x.MapFrom(y => y.TipoLogradouro))
              .ReverseMap();

             CreateMap<DataActSearchInput, DataActSearchInputDto>()
              .ForMember(x => x.BirthDate, x => x.MapFrom(y => y.DataNascimento))
              .ForMember(x => x.CpfCnpj, x => x.MapFrom(y => y.CpfCnpj))
              .ForMember(x => x.FatherName, x => x.MapFrom(y => y.NomePai))
              .ForMember(x => x.MatherName, x => x.MapFrom(y => y.NomeMae))
              .ForMember(x => x.NameSearch, x => x.MapFrom(y => y.NomeBusca))
              .ReverseMap();

            CreateMap<RequerenteInput, ApplicantInputDto>()
              .ForMember(x => x.ActRegistry, x => x.MapFrom(y => y.ActRegistry))
              .ForMember(x => x.Cnpj, x => x.MapFrom(y => y.Cnpj))
              .ForMember(x => x.Cpf, x => x.MapFrom(y => y.Cpf))
              .ForMember(x => x.DataActSearch, x => x.MapFrom(y => y.DataActSearch))
              .ForMember(x => x.Email, x => x.MapFrom(y => y.Email))
              .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
              .ForMember(x => x.Name, x => x.MapFrom(y => y.Nome))
              .ForMember(x => x.PropertyDetails, x => x.MapFrom(y => y.PropertyDetails))
              .ForMember(x => x.UrlCallback, x => x.MapFrom(y => y.UrlCallback))
              .ReverseMap();

            CreateMap<OrderInput, OrderInputDto>()
              .ForMember(x => x.Acts, x => x.MapFrom(y => y.Atos))
              .ForMember(x => x.UrlActsZip, x => x.MapFrom(y => y.AtosCompactados))
              .ForMember(x => x.Applicant, x => x.MapFrom(y => y.Requerente))
              .ForMember(x => x.Billet, x => x.MapFrom(y => y.Billet))
              .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
              .ForMember(x => x.OrderDate, x => x.MapFrom(y => y.DataPedido))
              .ForMember(x => x.OrderNumber, x => x.MapFrom(y => y.NumeroPedido))
              .ForMember(x => x.OrderValue, x => x.MapFrom(y => y.ValorPedido))
              .ForMember(x => x.QuantityActs, x => x.MapFrom(y => y.QuantidadeAtos))
              .ForMember(x => x.UrlCallback, x => x.MapFrom(y => y.UrlCallback))
              .ForMember(x => x.UrlCallbackResponse, x => x.MapFrom(y => y.UrlCallbackResponse))
              .ReverseMap();

            CreateMap<PedidoSolicitarResponse, OrderResponseDto>()
              .ForMember(x => x.OrderNumber, x => x.MapFrom(y => y.NumeroPedido))
              .ForMember(x => x.OrderValue, x => x.MapFrom(y => y.ValorPedido))
              .ForMember(x => x.QuantityCertificates, x => x.MapFrom(y => y.QuantidadeCertidoesSolicitadas))
              .ReverseMap();

             CreateMap<ApplicantInfoResponse, ApplicantInfoDto>()
              .ForMember(x => x.Name, x => x.MapFrom(y => y.Nome))
              .ReverseMap();

             CreateMap<RequirementResponse, RequirementDto>()
              .ForMember(x => x.DateCompliance, x => x.MapFrom(y => y.DataCumprimento))
              .ForMember(x => x.DueDate, x => x.MapFrom(y => y.DataPrazo))
              .ForMember(x => x.IdRequeriment, x => x.MapFrom(y => y.IdExigencia))
              .ForMember(x => x.Messages, x => x.MapFrom(y => y.Mensagens))
              .ForMember(x => x.RegistryDate, x => x.MapFrom(y => y.DataRegistro))
              .ForMember(x => x.ReleaseDate, x => x.MapFrom(y => y.DataLiberacao))
              .ForMember(x => x.Status, x => x.MapFrom(y => y.Status))
              .ForMember(x => x.TypeRequirement, x => x.MapFrom(y => y.TipoExigencia))
              .ForMember(x => x.ValueRequirement, x => x.MapFrom(y => y.ValorExigencia))
              .ReverseMap();

            CreateMap<MessageResponse, MessageDto>()
              .ForMember(x => x.DateTime, x => x.MapFrom(y => y.DataHora))
              .ForMember(x => x.From, x => x.MapFrom(y => y.Remetente))
              .ForMember(x => x.MessageText, x => x.MapFrom(y => y.MensagemText))
              .ReverseMap();

            CreateMap<ActResponse, ActDto>()
              .ForMember(x => x.ActNumber, x => x.MapFrom(y => y.NumeroAto))
              .ForMember(x => x.ActType, x => x.MapFrom(y => y.TipoAto))
              .ForMember(x => x.Cerp, x => x.MapFrom(y => y.Cerp))
              .ForMember(x => x.Certificate, x => x.MapFrom(y => y.Certidao))
              .ForMember(x => x.Goal, x => x.MapFrom(y => y.Finalidade))
              .ForMember(x => x.IdItem, x => x.MapFrom(y => y.IdItem))
              .ForMember(x => x.OrderDetail, x => x.MapFrom(y => y.InformacaoPedido))
              .ForMember(x => x.PaymentDate, x => x.MapFrom(y => y.DataPagamento))
              .ForMember(x => x.Registry, x => x.MapFrom(y => y.Cartorio))
              .ForMember(x => x.Status, x => x.MapFrom(y => y.Status))
              .ForMember(x => x.UrlAct, x => x.MapFrom(y => y.UrlAct))
              .ReverseMap();

             CreateMap<OrderInfoResponse, OrderDetailDto>()
              .ForMember(x => x.Applicante, x => x.MapFrom(y => y.InfoRequerente))
              .ForMember(x => x.OrderDate, x => x.MapFrom(y => y.DataPedido))
              .ForMember(x => x.OrderNumber, x => x.MapFrom(y => y.NumeroPedido))
              .ReverseMap();

             CreateMap<AtoInput, ActInputDto>()
              .ForMember(x => x.ActNumber, x => x.MapFrom(y => y.NumeroAto))
              .ForMember(x => x.ActType, x => x.MapFrom(y => y.TipoAto))
              .ForMember(x => x.Cerp, x => x.MapFrom(y => y.Cerp))
              .ForMember(x => x.Certificate, x => x.MapFrom(y => y.Certidao))
              .ForMember(x => x.Goal, x => x.MapFrom(y => y.Finalidade))
              .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
              .ForMember(x => x.PaymentDate, x => x.MapFrom(y => y.DataPagamento))
              .ForMember(x => x.Register, x => x.MapFrom(y => y.Cartorio))
              .ForMember(x => x.Status, x => x.MapFrom(y => y.Status))
              .ForMember(x => x.UrlAct, x => x.MapFrom(y => y.UrlAct))
              .ReverseMap();

            CreateMap<CategoryCertificatesResponse, CertificateByCategoryDto>()
              .ForMember(x => x.Act, x => x.MapFrom(y => y.Ato))
              .ForMember(x => x.ActDescription, x => x.MapFrom(y => y.AtoDescricao))
              .ForMember(x => x.ActId, x => x.MapFrom(y => y.AtoId))
              .ForMember(x => x.ActValue, x => x.MapFrom(y => y.AtoValor))
              .ForMember(x => x.RegisterCode, x => x.MapFrom(y => y.CodigoCartorio))
              .ReverseMap();

            CreateMap<CategoryResponse, CategoryDto>()
               .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
               .ForMember(x => x.Name, x => x.MapFrom(y => y.Nome))
               .ForMember(x => x.Description, x => x.MapFrom(y => y.Descricao))
               .ReverseMap();

            CreateMap<KitListarResponse, KitDto>()
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.Nome))
                .ForMember(x => x.City, x => x.MapFrom(y => y.Municipio))
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Description))
                .ReverseMap();

            CreateMap<KitCertidoesPorKitResponse, CertificateByKitDto>()
                .ForMember(x => x.IdKit, x => x.MapFrom(y => y.IdKit))
                .ForMember(x => x.ActId, x => x.MapFrom(y => y.AtoId))
                .ForMember(x => x.City, x => x.MapFrom(y => y.Municipio))
                .ForMember(x => x.ActType, x => x.MapFrom(y => y.TipoAto))
                .ForMember(x => x.ActType, x => x.MapFrom(y => y.TipoAto))
                .ForMember(x => x.Required, x => x.MapFrom(y => y.EnvioObrigatorio))
                .ForMember(x => x.Registers, x => x.MapFrom(y => y.Cartorios))
                .ForMember(x => x.Values, x => x.MapFrom(y => y.Valores))
                .ForMember(x => x.ActDescription, x => x.MapFrom(y => y.AtoDescricao))
                .ReverseMap();

            CreateMap<OrderInput, PedidoSolicitarRequest>().ReverseMap();
            CreateMap<OrderInput, PedidoConsultarPedidoResponse>().ReverseMap();
            CreateMap<Order, OrderInput>().ReverseMap();
            CreateMap<Order, PedidoConsultarPedidoResponse>().ReverseMap();

            CreateMap<AtoConsultaPedidoResponse, eCartorio.Domain.Entities.Ato>()
                .ForMember(x => x.NumeroAto, x => x.MapFrom(y => y.NumeroAto)).ReverseMap();

            CreateMap<eCartorio.Domain.Entities.Requerente, eCartorio.Application.Models.ExternalServices.Requerente>().ReverseMap();
            CreateMap<AtoInput, eCartorio.Application.Models.ExternalServices.Ato>().ReverseMap();
            CreateMap<AtoInput, AtoConsultaPedidoResponse>().ReverseMap();
            CreateMap<AtoInput, ConsultarAtoResponse>().ReverseMap();

            CreateMap<InformacaoPedido, OrderInfoResponse>().ReverseMap();
            CreateMap<ApplicantInfoResponse, InfoRequerente>().ReverseMap();

            CreateMap<AtoInput, eCartorio.Domain.Entities.Ato>().ReverseMap();
            CreateMap<RequerenteInput, eCartorio.Application.Models.ExternalServices.Requerente>().ReverseMap();
            CreateMap<RequerenteInput, eCartorio.Domain.Entities.Requerente>().ReverseMap();
            CreateMap<CategoriaListarResponse, CategoryResponse>().ReverseMap();
            CreateMap<CategoriaCertidoesPorCategoriaResponse, CategoryCertificatesResponse>().ReverseMap();
            CreateMap<ConsultarAtoResponse, ActResponse>().ReverseMap();
            CreateMap<Mensagem, MessageResponse>().ReverseMap();
            CreateMap<ExigenciaListarResponse, RequirementResponse>().ReverseMap();
            CreateMap<DadosAtoImovel, PropertyDetailsInput>().ReverseMap();
            CreateMap<DadosAtoComplementoImovel, PropertyDetailsComplementInput>().ReverseMap();
            CreateMap<DadosAtoBuscar, DataActSearchInput>().ReverseMap();
            CreateMap<eCartorio.Domain.Entities.Ato, eCartorio.Application.Models.ExternalServices.Ato>().ReverseMap();
        }
    }
}
