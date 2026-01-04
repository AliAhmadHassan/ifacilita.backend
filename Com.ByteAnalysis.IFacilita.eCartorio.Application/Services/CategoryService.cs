using AutoMapper;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IeCartorioClient _ieCartorioClient;

        public CategoryService(IMapper mapper, IeCartorioClient ieCartorioClient)
        {
            _mapper = mapper;
            _ieCartorioClient = ieCartorioClient;
        }

        public async Task<IEnumerable<CategoryCertificatesResponse>> CertificatesByCategory(int idCategory, string city)
        {
            if(idCategory <= 0)
                throw new BadRequestException("O id da categoria é obrigatóio.");

            if (string.IsNullOrEmpty(city))
                throw new BadRequestException("O município é obrigatóio.");

            var result = await _ieCartorioClient.CategoriaCertidoesPorCategoriaAsync(idCategory, city);
            var resultMapped = _mapper.Map<IEnumerable<CategoryCertificatesResponse>>(result);

            return resultMapped;
        }

        public async Task<IEnumerable<CategoryResponse>> List(string city)
        {
            if (string.IsNullOrEmpty(city))
                throw new BadRequestException("O município é obrigatóio.");

            var result = await _ieCartorioClient.CategoriaListarAsync(city);
            var resultMapped = _mapper.Map<IEnumerable<CategoryResponse>>(result);

            return resultMapped;
        }
    }
}
