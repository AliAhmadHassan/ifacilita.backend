using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Enums;
using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Models;
using FluentValidation;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Validators
{
    public class GuideRequestValidation : BaseValidator<GuideRequestInput>
    {
        public GuideRequestValidation()
        {
            RuleFor(x => x.Iptu).NotNull().NotEmpty().WithMessage("Iptu: Obrigatório");
            RuleFor(x => x.Value).NotNull().NotEmpty().WithMessage("Value: Obrigatório");
            RuleFor(x => x.TransactionNature).NotNull().NotEmpty().WithMessage("TransactionNature: Obrigatório");
            RuleFor(x => x.TransactionNature).Must(ValidateTransactionNature).WithMessage("TransactionNature: Valor inválido");
            RuleFor(x => x.PurchaserTransmitted).NotNull().WithMessage("PurchaserTransmitted Obrigatório");
            RuleFor(x => x.PurchaserTransmitted).SetValidator(new PurchaserTransmittedValidation());
            RuleFor(x => x.Generation).NotNull().WithMessage("Generation Obrigatório");
            RuleFor(x => x.Generation).SetValidator(new GenerationValidation());
        }

        private bool ValidateTransactionNature(TransactionNature transaction)
        {
            switch (transaction)
            {
                case TransactionNature.Aquisicao_da_Nua_Propriedade:
                case TransactionNature.Cessao_de_Direitos_Hereditarios:
                case TransactionNature.Cessao_de_Dir_Aquisitivos_Decorrentes_de_Promessa:
                case TransactionNature.Compra_e_Venda:
                case TransactionNature.Consolidacao_de_Propriedade:
                case TransactionNature.Dacao_em_Pagamento:
                case TransactionNature.Inst_Usufruto_Uso_e_Habitacao:
                case TransactionNature.Integraliz_Real_Cap_Pgto_ITBI_ANTES_Reg_Junta:
                case TransactionNature.Outorga_Dir_Real_Superficie:
                case TransactionNature.Permuta:

                    return true;
                default:
                    return false;
            }
        }
    }
}
