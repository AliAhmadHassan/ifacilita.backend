using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.ExternalServices.Api;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Specialized;
using System.Web;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions;

namespace Com.ByteAnalysis.IFacilita.eCartorio.ExternalServices
{
    public class eCartorioClient : IeCartorioClient
    {
        private readonly IeCartorioApi _ieCartorioApi;
        private readonly IConfiguration _configuration;
        private readonly string _eCartorioUrlBase;

        private readonly ICache<IEnumerable<string>> _cache;
        private readonly ICache<IEnumerable<FuncaoListarFinalidadeResponse>> _cacheFinalidade;
        private readonly ICache<IEnumerable<FuncaoCartorioPorAtoResponse>> _cacheCartorioPorAtos;
        private readonly ICache<IEnumerable<CategoriaCertidoesPorCategoriaResponse>> _cacheCategoriaCertidoesPorCategoria;
        private readonly ICache<IEnumerable<CategoriaListarResponse>> _cacheCategoriaListar;

        private readonly ICache<IEnumerable<KitListarResponse>> _cacheKitListar;
        private readonly ICache<IEnumerable<KitCertidoesPorKitResponse>> _cacheKitCertidoesPorKit;

        private readonly string _cache_name_funcaoListarMunicipios;
        private readonly string _cache_name_funcaoListarTiposLogradouros;
        private readonly string _cache_name_funcaoListarComplementoLogradouros;
        private readonly string _cache_name_funcaoListarFinalidade;
        private readonly string _cache_name_funcaoListarCartorioPorAtos;

        private readonly string _cache_name_categoriaCertidoesPorCategoria;
        private readonly string _cache_name_categoriaListar;

        private readonly string _cache_name_KitListar;
        private readonly string _cache_name_KitCertidoesPorKit;

        public eCartorioClient(
            IConfiguration configuration,
            IeCartorioApi ieCartorioApi,
            ICache<IEnumerable<FuncaoListarFinalidadeResponse>> cacheFinalidade,
            ICache<IEnumerable<FuncaoCartorioPorAtoResponse>> cacheCartorioPorAtos,
            ICache<IEnumerable<CategoriaCertidoesPorCategoriaResponse>> cacheCategoriaCertidoesPorCategoria,
            ICache<IEnumerable<CategoriaListarResponse>> cacheCategoriaListar,
            ICache<IEnumerable<KitListarResponse>> cacheKitListar,
            ICache<IEnumerable<KitCertidoesPorKitResponse>> cacheKitCertidoesPorKit,
            ICache<IEnumerable<string>> cache)
        {
            _cache = cache;
            _cacheFinalidade = cacheFinalidade;
            _cacheCartorioPorAtos = cacheCartorioPorAtos;

            _cache_name_KitListar = "cache_KitListar";
            _cacheKitListar = cacheKitListar;

            _cacheKitCertidoesPorKit = cacheKitCertidoesPorKit;
            _cache_name_KitCertidoesPorKit = "cache_KitCertidoesPorKit";

            _cacheCategoriaCertidoesPorCategoria = cacheCategoriaCertidoesPorCategoria;
            _cacheCategoriaListar = cacheCategoriaListar;

            _cache_name_funcaoListarMunicipios = "cache_funcaoListarMunicipios";
            _cache_name_funcaoListarTiposLogradouros = "cache_funcaoListarTiposLogradouros";
            _cache_name_funcaoListarComplementoLogradouros = "cache_funcaoListarComplementoLogradouros";
            _cache_name_funcaoListarFinalidade = "cache_funcaoListarFinalidade";
            _cache_name_funcaoListarCartorioPorAtos = "cache_funcaoListarCartorioPorAtos";

            _cache_name_categoriaCertidoesPorCategoria = "cache_categoriaCertidoesPorCategoria";
            _cache_name_categoriaListar = "_cache_name_categoriaListar";

            _ieCartorioApi = ieCartorioApi;
            _configuration = configuration;
            _eCartorioUrlBase = _configuration["eCartorio:UrlBase"];
        }

        #region Ato

        public async Task<ConsultarAtoResponse> AtoConsultarAtoAsync(string cerp)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("cerp", cerp);

            var response = await _ieCartorioApi.GetAsync<ConsultarAtoResponse>(_eCartorioUrlBase + "/Ato/ConsultarAto", query);

            return response;
        }

        public async Task<string> AtoBaixarAtoAsync(string cerp)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("cerp", cerp);

            var response = await _ieCartorioApi.GetStringAsync(_eCartorioUrlBase + "/Ato/BaixarAto", query);

            return response;
        }

        public async Task<string> AtoBaixarAtoBase64Async(string cerp)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("cerp", cerp);

            var response = await _ieCartorioApi.GetStringAsync(_eCartorioUrlBase + "/Ato/BaixarAtoBase64", query);

            return response;
        }

        public async Task<object> AtoVistartAtoAsync(string cerp)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("cerp", cerp);

            var response = await _ieCartorioApi.GetAsync<string>(_eCartorioUrlBase + "/Ato/VistarAto", query);

            return true;
        }

        #endregion

        #region Categoria

        public async Task<IEnumerable<CategoriaCertidoesPorCategoriaResponse>> CategoriaCertidoesPorCategoriaAsync(int idCategoria, string municipio)
        {
            var listReturn = new List<CategoriaCertidoesPorCategoriaResponse>();
            var cache = await _cacheCategoriaCertidoesPorCategoria.GetAsync(_cache_name_categoriaCertidoesPorCategoria);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("idCategoria", idCategoria.ToString());
            query.Add("municipio", municipio);

            var uri = _eCartorioUrlBase + "/Categoria/CertidoesPorCategoria";

            if (cache == null)
            {
                var response = await _ieCartorioApi.GetAsync<IEnumerable<CategoriaCertidoesPorCategoriaResponse>>(uri, query);
                response.ToList().ForEach(x => x.IdCategoria = idCategoria);

                await _cacheCategoriaCertidoesPorCategoria.AddAsync(_cache_name_categoriaCertidoesPorCategoria, response, DateTime.Now.AddDays(1));
                listReturn = response.ToList();
            }
            else
            {
                if (cache.FirstOrDefault()?.IdCategoria == idCategoria)
                    listReturn = cache.ToList();
                else
                {
                    var response = await _ieCartorioApi.GetAsync<IEnumerable<CategoriaCertidoesPorCategoriaResponse>>(uri, query);
                    response.ToList().ForEach(x => x.IdCategoria = idCategoria);

                    await _cacheCategoriaCertidoesPorCategoria.AddAsync(_cache_name_categoriaCertidoesPorCategoria, response, DateTime.Now.AddDays(1));
                    listReturn = response.ToList();
                }
            }

            return listReturn;
        }

        public async Task<IEnumerable<CategoriaListarResponse>> CategoriaListarAsync(string municipio)
        {
            var listReturn = new List<CategoriaListarResponse>();
            var cache = await _cacheCategoriaListar.GetAsync(_cache_name_categoriaListar);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("municipio", municipio);

            var uri = _eCartorioUrlBase + "/Categoria/Listar";

            if (cache == null)
            {
                var response = await _ieCartorioApi.GetAsync<IEnumerable<CategoriaListarResponse>>(uri, query);
                response.ToList().ForEach(x => x.Municipio = municipio);

                await _cacheCategoriaListar.AddAsync(_cache_name_categoriaListar, response, DateTime.Now.AddDays(1));
                listReturn = response.ToList();
            }
            else
            {
                if (cache.Any(x => x.Municipio == municipio))
                    listReturn = cache.ToList();
                else
                {
                    var response = await _ieCartorioApi.GetAsync<IEnumerable<CategoriaListarResponse>>(uri, query);
                    response.ToList().ForEach(x => x.Municipio = municipio);

                    cache.ToList().AddRange(response);

                    _cacheCategoriaListar.Remove(_cache_name_categoriaListar);

                    await _cacheCategoriaListar.AddAsync(_cache_name_categoriaListar, cache, DateTime.Now.AddDays(1));
                    listReturn = response.ToList();
                }
            }

            return listReturn;
        }

        #endregion

        #region Funções

        public async Task<IEnumerable<FuncaoCartorioPorAtoResponse>> FuncaoListarCartorioPorAtoAsync(int idAto, string municipio)
        {
            var listReturn = new List<FuncaoCartorioPorAtoResponse>();
            var cache = await _cacheCartorioPorAtos.GetAsync(_cache_name_funcaoListarCartorioPorAtos);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("idAto", idAto.ToString());
            query.Add("municipio", municipio.ToString());

            if (cache == null)
            {
                var response = await _ieCartorioApi.GetAsync<IEnumerable<FuncaoCartorioPorAtoResponse>>(_eCartorioUrlBase + "/Funcao/ListarCartoriosPorAto", query);
                response.ToList().ForEach(x => x.IdAto = idAto);

                await _cacheCartorioPorAtos.AddAsync(_cache_name_funcaoListarCartorioPorAtos, response, DateTime.Now.AddDays(1));
                listReturn = response.ToList();
            }
            else
            {
                if (cache.FirstOrDefault()?.IdAto == idAto)
                    listReturn = cache.ToList();
                else
                {
                    var response = await _ieCartorioApi.GetAsync<IEnumerable<FuncaoCartorioPorAtoResponse>>(_eCartorioUrlBase + "/Funcao/ListarCartoriosPorAto", query);
                    response.ToList().ForEach(x => x.IdAto = idAto);

                    await _cacheCartorioPorAtos.AddAsync(_cache_name_funcaoListarCartorioPorAtos, response, DateTime.Now.AddDays(1));
                    listReturn = response.ToList();
                }
            }

            return listReturn;
        }

        public async Task<IEnumerable<string>> FuncaoListarComplementosLogradouroAsync()
        {
            var listReturn = new List<string>();
            var cache = await _cache.GetAsync(_cache_name_funcaoListarComplementoLogradouros);

            if (cache == null)
            {
                var response = await _ieCartorioApi.GetAsync<IEnumerable<string>>(_eCartorioUrlBase + "/Funcao/ListarComplementosLogradouro", null);
                await _cache.AddAsync(_cache_name_funcaoListarComplementoLogradouros, response, DateTime.Now.AddDays(1));
                listReturn = response.ToList();
            }
            else
            {
                listReturn = cache.ToList();
            }

            return listReturn;
        }

        public async Task<IEnumerable<FuncaoListarFinalidadeResponse>> FuncaoListarFinalidadesAsync()
        {
            var listReturn = new List<FuncaoListarFinalidadeResponse>();
            var cache = await _cacheFinalidade.GetAsync(_cache_name_funcaoListarFinalidade);

            if (cache == null)
            {
                var response = await _ieCartorioApi.GetAsync<IEnumerable<FuncaoListarFinalidadeResponse>>(_eCartorioUrlBase + "/Funcao/ListarFinalidades", null);
                await _cacheFinalidade.AddAsync(_cache_name_funcaoListarFinalidade, response, DateTime.Now.AddDays(1));
                listReturn = response.ToList();
            }
            else
            {
                listReturn = cache.ToList();
            }

            return listReturn;
        }

        public async Task<IEnumerable<string>> FuncaoListarMunicipiosAsync()
        {
            var listaMunicipios = new List<string>();
            var cache = await _cache.GetAsync(_cache_name_funcaoListarMunicipios);

            if (cache == null)
            {
                var response = await _ieCartorioApi.GetAsync<IEnumerable<string>>(_eCartorioUrlBase + "/Funcao/ListarMunicipios", null);
                await _cache.AddAsync(_cache_name_funcaoListarMunicipios, response, DateTime.Now.AddDays(1));
                listaMunicipios = response.ToList();
            }
            else
            {
                listaMunicipios = cache.ToList();
            }

            return listaMunicipios;
        }

        public async Task<IEnumerable<string>> FuncaoListarTiposLogradouroAsync()
        {
            var listReturn = new List<string>();
            var cache = await _cache.GetAsync(_cache_name_funcaoListarTiposLogradouros);

            if (cache == null)
            {
                var response = await _ieCartorioApi.GetAsync<IEnumerable<string>>(_eCartorioUrlBase + "/Funcao/ListarTiposLogradouro", null);
                await _cache.AddAsync(_cache_name_funcaoListarTiposLogradouros, response, DateTime.Now.AddDays(1));
                listReturn = response.ToList();
            }
            else
            {
                listReturn = cache.ToList();
            }

            return listReturn;
        }

        #endregion

        #region Kit

        public async Task<IEnumerable<KitListarResponse>> KitListarAsync(string municipio)
        {
            var listReturn = new List<KitListarResponse>();
            var cache = await _cacheKitListar.GetAsync(_cache_name_KitListar);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("municipio", municipio);

            var uri = _eCartorioUrlBase + "/Kit/Listar";

            if (cache == null)
            {
                var response = await _ieCartorioApi.GetAsync<IEnumerable<KitListarResponse>>(uri, query);
                response.ToList().ForEach(x => x.Municipio = municipio);

                await _cacheKitListar.AddAsync(_cache_name_KitListar, response, DateTime.Now.AddDays(1));
                listReturn = response.ToList();
            }
            else
            {
                if (cache.Any(x => x.Municipio == municipio))
                    listReturn = cache.ToList();
                else
                {
                    var response = await _ieCartorioApi.GetAsync<IEnumerable<KitListarResponse>>(uri, query);
                    response.ToList().ForEach(x => x.Municipio = municipio);

                    cache.ToList().AddRange(response);

                    _cacheKitListar.Remove(_cache_name_KitListar);

                    await _cacheKitListar.AddAsync(_cache_name_KitListar, cache, DateTime.Now.AddDays(1));
                    listReturn = response.ToList();
                }
            }

            return listReturn;
        }

        public async Task<IEnumerable<KitCertidoesPorKitResponse>> KitListarCertidoesPorKitAsync(int idKit, string municipio)
        {

            if (idKit == 0)
                throw new BadRequestException("O idKit é obrigatório.");

            if (string.IsNullOrEmpty(municipio))
                throw new BadRequestException("O município é obrigatório.");

            var listReturn = new List<KitCertidoesPorKitResponse>();
            var cache = await _cacheKitCertidoesPorKit.GetAsync(_cache_name_KitCertidoesPorKit);

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("municipio", municipio);
            query.Add("idKit", idKit.ToString());

            var uri = _eCartorioUrlBase + "/Kit/CertidoesPorKit";

            if (cache == null)
            {
                var response = await _ieCartorioApi.GetAsync<IEnumerable<KitCertidoesPorKitResponse>>(uri, query);
                response.ToList().ForEach(x => { x.Municipio = municipio; x.IdKit = idKit; });

                await _cacheKitCertidoesPorKit.AddAsync(_cache_name_KitCertidoesPorKit, response, DateTime.Now.AddDays(1));
                listReturn = response.ToList();
            }
            else
            {
                if (cache.Any(x => x.Municipio == municipio))
                    listReturn = cache.ToList();
                else
                {
                    var response = await _ieCartorioApi.GetAsync<IEnumerable<KitCertidoesPorKitResponse>>(uri, query);
                    response.ToList().ForEach(x => { x.Municipio = municipio; x.IdKit = idKit; });

                    cache.ToList().AddRange(response);

                    _cacheKitCertidoesPorKit.Remove(_cache_name_KitCertidoesPorKit);

                    await _cacheKitCertidoesPorKit.AddAsync(_cache_name_KitCertidoesPorKit, cache, DateTime.Now.AddDays(1));
                    listReturn = response.ToList();
                }
            }

            return listReturn;
        }

        #endregion

        #region Exigencia

        public async Task<ExigenciaListarResponse> ExigenciaListarExigenciaAsync(string cerp)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("cerp", cerp);

            var response = await _ieCartorioApi.GetAsync<ExigenciaListarResponse>(_eCartorioUrlBase + "/Exigencia/ListarExigencias", query);

            return response;
        }

        public async Task<IEnumerable<PesquisarExigenciasPorRequerenteResponse>> ExigenciaPesquisarExigenciasPorRequerenteAsync(string documento)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("documento", documento);

            var response = await _ieCartorioApi.GetAsync<IEnumerable<PesquisarExigenciasPorRequerenteResponse>>(_eCartorioUrlBase + "/Exigencia/PesquisarExigenciasPorRequerente", query);

            return response;
        }

        public async Task<bool> ExigenciaResponderExigenciaAsync(string idExigencia, string mensagem)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("idExigencia", idExigencia);
            query.Add("mensagem", mensagem);

            _ = await _ieCartorioApi.PostAsync<object, object>(_eCartorioUrlBase + "/Exigencia/ResponderExigencia", null, query);

            return true;
        }

        #endregion

        #region Pedido

        public async Task<PedidoSolicitarResponse> PedidoSolicitarAsync(PedidoSolicitarRequest pedido)
        {
            var url = $"{_eCartorioUrlBase}/Pedido/Solicitar";
            var result = await _ieCartorioApi.PostAsync<PedidoSolicitarResponse, PedidoSolicitarRequest>(url, pedido, null);
            return result;
        }

        public async Task<string> PedidoBaixarBoletoAsync(string numeroPedido)
        {
            var url = $"{_eCartorioUrlBase}/Pedido/BaixarBoletoBase64";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("numeroPedido", numeroPedido);

            var result = await _ieCartorioApi.GetStringAsync(url, query);
            return result;
        }

        public async Task<PedidoConsultarPedidoResponse> PedidoConsultarAsync(string numeroPedido)
        {
            var url = $"{_eCartorioUrlBase}/Pedido/ConsultarPedido";
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("numeroPedido", numeroPedido);

            var result = await _ieCartorioApi.GetAsync<PedidoConsultarPedidoResponse>(url, query);
            return result;
        }

        public Task<IEnumerable<PedidoListarResponse>> PedidoListarAsync(string dataInicial, string dataFinal, string numeroPedido, string[] status, string numeroAto, string cerp, string seloAleatorio, string nomeRequerente, string emailRequerente, string cartorio)
        {
            throw new NotImplementedException();
        }

        public async Task<string> PedidoBaixarAtosAsync(string numeroPedido)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("numeroPedido", numeroPedido);

            var response = await _ieCartorioApi.GetStringAsync(_eCartorioUrlBase + "/Pedido/BaixarAtosBase64", query);

            return response;
        }

        #endregion
    }
}
