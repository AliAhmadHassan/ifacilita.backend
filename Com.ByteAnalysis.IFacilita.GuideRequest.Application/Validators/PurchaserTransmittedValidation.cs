using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Models;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Validators
{
    public class PurchaserTransmittedValidation : BaseValidator<PurchaserTransmittedInput>
    {
        public PurchaserTransmittedValidation()
        {
            RuleFor(x => x.Address).NotNull().WithMessage("Address: Obrigatório");
            RuleFor(x => x.Balcony).NotNull().WithMessage("Balcony: Obrigatório");
            RuleFor(x => x.BathroomMaid).NotNull().WithMessage("BathroomMaid: Obrigatório");
            RuleFor(x => x.Cep).NotNull().WithMessage("Cep: Obrigatório");
            RuleFor(x => x.Cep).NotNull().MinimumLength(8).WithMessage("Cep: Inválido");
            RuleFor(x => x.City).NotNull().WithMessage("City: Obrigatório");
            RuleFor(x => x.Complement).NotNull().WithMessage("Complement: Obrigatório");
            RuleFor(x => x.CountBathroomExceptMaid).NotNull().WithMessage("CountBathroomExceptMaid: Obrigatório");
            RuleFor(x => x.CountBedrooms).NotNull().WithMessage("CountBedrooms: Obrigatório");
            RuleFor(x => x.CountParkingSpot).NotNull().WithMessage("CountParkingSpot: Obrigatório");
            RuleFor(x => x.Ddd).NotNull().WithMessage("Ddd: Obrigatório");
            RuleFor(x => x.Elevator).NotNull().WithMessage("Elevator: Obrigatório");
            RuleFor(x => x.Email).NotNull().WithMessage("Email: Obrigatório");
            RuleFor(x => x.FloorPosition).NotNull().Must(CheckFloorPosition).WithMessage("FloorPosition: Obrigatório");
            RuleFor(x => x.FloorPosition).NotNull().Must(CheckFloorPosition).WithMessage("FloorPosition: Formato inválid. Ex: Terreo, 01 andar, 20 andar, 60 andar");
            RuleFor(x => x.MaidRoom).NotNull().WithMessage("MaidRoom: Obrigatório");
            RuleFor(x => x.Neighborhood).NotNull().WithMessage("MaidRoom: Obrigatório");
            RuleFor(x => x.Number).NotNull().WithMessage("Number: Obrigatório");
            RuleFor(x => x.PhoneNumber).NotNull().WithMessage("PhoneNumber: Obrigatório");
            RuleFor(x => x.PropertyForeiro).NotNull().WithMessage("PropertyForeiro: Obrigatório");
            RuleFor(x => x.PurchaserName).NotNull().WithMessage("PurchaserName: Obrigatório");
            RuleFor(x => x.PurchaserOwnerSettings).NotNull().WithMessage("PurchaserOwnerSettings: Obrigatório");
            RuleFor(x => x.RecreationArea).NotNull().WithMessage("RecreationArea: Obrigatório");
            RuleFor(x => x.TransmittedName).NotNull().WithMessage("TransmittedName: Obrigatório");
            RuleFor(x => x.TransmittedOwnerSettings).NotNull().WithMessage("TransmittedOwnerSettings: Obrigatório");
            RuleFor(x => x.Uf).NotNull().WithMessage("Uf: Obrigatório");
            RuleFor(x => x.Uf).NotNull().Must(CheckUf).WithMessage("Uf: Valor inválido");
        }

        private bool CheckFloorPosition(string floor)
        {
            if (floor == "Terreo")
                return true;

            for (int i = 1; i < 61; i++)
            {
                var nameFloor = $"{i.ToString("00")} andar";
                if (floor == nameFloor)
                    return true;
            }

            return false;
        }

        private bool CheckUf(string uf)
        {
            List<string> Ufs = new List<string>();

            Ufs.Add("AC");
            Ufs.Add("AL");
            Ufs.Add("AM");
            Ufs.Add("AP");
            Ufs.Add("BA");
            Ufs.Add("CE");
            Ufs.Add("DF");
            Ufs.Add("ES");
            Ufs.Add("FN");
            Ufs.Add("GO");
            Ufs.Add("MA");
            Ufs.Add("MG");
            Ufs.Add("MS");
            Ufs.Add("MT");
            Ufs.Add("PA");
            Ufs.Add("PB");
            Ufs.Add("PE");
            Ufs.Add("PI");
            Ufs.Add("PR");
            Ufs.Add("RJ");
            Ufs.Add("RN");
            Ufs.Add("RO");
            Ufs.Add("RR");
            Ufs.Add("RS");
            Ufs.Add("SC");
            Ufs.Add("SE");
            Ufs.Add("SP");
            Ufs.Add("TO");

            return Ufs.Any(x => x == uf);
        }
    }
}
