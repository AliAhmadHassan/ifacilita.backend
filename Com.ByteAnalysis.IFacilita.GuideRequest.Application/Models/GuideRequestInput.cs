using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Enums;
using System.Collections;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Models
{
    public class GuideRequestInput
    {
        public string Id { get; set; }

        /// <summary>
        /// Inscrição do Imóvel com DV* (IPTU):
        /// </summary>
        public int Iptu { get; set; }

        /// <summary>
        /// Valor Declarado: R$
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Natureza da Transação:
        /// </summary>
        public TransactionNature TransactionNature { get; set; }

        /// <summary>
        /// PAL (APENAS PARA TERRENOS) - PAL - Projeto Aprovado de Loteamento
        /// </summary>
        public string Pal { get; set; }

        /// <summary>
        /// Parte transferida - Corresponde à parte do imóvel (percentual ou fracionária) que está sendo transferida.
        /// Ex.: 100 (%), 50 (%), ¼ (não adotar o valor decimal).
        /// Atenção: Este campo não se refere à fração ideal do imóvel.
        /// </summary>
        public string TransferredPart { get; set; }


        /// <summary>
        /// Situação do processamento pelo Robô
        /// </summary>
        /// <example>1 - Pendente, 2 - Concluído, 3 - Error</example>
        public Common.Enumerable.APIStatus Status { get; set; }

        /// <summary>
        /// Verifica se a guia já foi gerada
        /// </summary>
        /// <example>1 - Não, 2 - Sim</example>
        public int StatusGuide { get; set; }

        public bool Approved { get; set; }

        public SimulateInput Simulate { get; set; }

        public GenerationInput Generation { get; set; }

        public PurchaserTransmittedInput PurchaserTransmitted { get; set; }

        public PreProtocolInput PreProtocol { get; set; }

        public ProtocolInput Protocol { get; set; }

        public GuideInput Guide { get; set; }

        public IEnumerable<Common.Exceptions.GlobalError> Errors { get; set; }

        public string UrlCallback { get; set; } //https://ifacilita.com/api/transaction/{0}/Callback-itbi-rj     UrlCallback = stringFormat(UrlCallback, _id);

        public string UrlCallbackResponse { get; set; }

    }
}
