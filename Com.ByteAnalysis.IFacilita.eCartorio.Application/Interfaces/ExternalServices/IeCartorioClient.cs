using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces.ExternalServices
{
    public interface IeCartorioClient
    {
        #region Kit

        Task<IEnumerable<KitListarResponse>> KitListarAsync(string municipio);

        Task<IEnumerable<KitCertidoesPorKitResponse>> KitListarCertidoesPorKitAsync(int idKit, string municipio);

        #endregion

        #region Funcao

        Task<IEnumerable<string>> FuncaoListarMunicipiosAsync();

        Task<IEnumerable<string>> FuncaoListarTiposLogradouroAsync();

        Task<IEnumerable<string>> FuncaoListarComplementosLogradouroAsync();

        Task<IEnumerable<FuncaoListarFinalidadeResponse>> FuncaoListarFinalidadesAsync();

        Task<IEnumerable<FuncaoCartorioPorAtoResponse>> FuncaoListarCartorioPorAtoAsync(int idAto, string municipio);

        #endregion

        #region Categoria

        Task<IEnumerable<CategoriaListarResponse>> CategoriaListarAsync(string municipio);

        Task<IEnumerable<CategoriaCertidoesPorCategoriaResponse>> CategoriaCertidoesPorCategoriaAsync(int idCategoria, string municipio);

        #endregion

        #region Ato

        Task<ConsultarAtoResponse> AtoConsultarAtoAsync(string cerp);

        Task<string> AtoBaixarAtoAsync(string cerp);

        Task<string> AtoBaixarAtoBase64Async(string cerp);

        //TODO - Verificar o retorno do endpoint
        Task<object> AtoVistartAtoAsync(string cerp);

        #endregion

        #region Exigencia

        Task<ExigenciaListarResponse> ExigenciaListarExigenciaAsync(string cerp);

        Task<IEnumerable<PesquisarExigenciasPorRequerenteResponse>> ExigenciaPesquisarExigenciasPorRequerenteAsync(string documento);

        Task<bool> ExigenciaResponderExigenciaAsync(string idExigencia, string mensagem);

        #endregion

        #region Pedido

        Task<PedidoSolicitarResponse> PedidoSolicitarAsync(PedidoSolicitarRequest pedido);

        Task<string> PedidoBaixarBoletoAsync(string numeroPedido);

        Task<PedidoConsultarPedidoResponse> PedidoConsultarAsync(string numeroPedido);

        Task<IEnumerable<PedidoListarResponse>> PedidoListarAsync(
                string dataInicial,
                string dataFinal,
                string numeroPedido,
                string[] status,
                string numeroAto,
                string cerp,
                string seloAleatorio,
                string nomeRequerente,
                string emailRequerente,
                string cartorio
            );

        Task<string> PedidoBaixarAtosAsync(string numeroPedido);

        #endregion

    }
}
