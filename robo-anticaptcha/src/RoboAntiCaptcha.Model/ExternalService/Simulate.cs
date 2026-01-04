using RoboAntiCaptchaModel.Enums;
using System;

namespace RoboAntiCaptchaModel.ExternalService
{
    public class Simulate
    {
        public long Iptu { get; set; }

        public decimal Value { get; set; }

        public TransactionNature TransactionNature;

        public string Pal { get; set; }

        public string TransferredPart { get; set; }

        public decimal CalculationBasis { get; set; }

        public decimal Taxation { get; set; }

        public string Utilization { get; set; }

        public string Address { get; set; }

        public DateTime Due { get; set; }
    }
}
