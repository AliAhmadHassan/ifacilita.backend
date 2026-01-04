using RoboAntiCaptchaModel.Enums;
using System;

namespace RoboAntiCaptchaModel.ExternalService
{
    public class Protocol
    {
        public int ProtocolNumber { get; set; }

        public string Iptu { get; set; }

        public string PurchaserDocument { get; set; }

        public string PurchaserInformed { get; set; }

        public string TransmittedDocument { get; set; }

        public string TransmittedInformed { get; set; }

        public decimal PercentageTransferred { get; set; }

        public decimal ValueItbi { get; set; }

        public DateTime Due { get; set; }

        public string Address { get; set; }

        public TransactionNature TransactionNature { get; set; }

        public decimal DeclaredValue { get; set; }

    }
}
