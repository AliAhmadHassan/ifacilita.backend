using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Models;
using FluentValidation;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Validators
{
    public class GenerationValidation : BaseValidator<GenerationInput>
    {
        public GenerationValidation()
        {
            RuleFor(x => x.Purchaser).NotNull().NotEmpty().WithMessage("Purchaser: Obrigatório");
            RuleFor(x => x.Transmitted).NotNull().NotEmpty().WithMessage("Transmitted: Obrigatório");
        }
    }
}
