using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class SendInvite: BasicEntity
    {
        public Int32 Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? EMailSended { get; set; }
        public DateTime? SMSSended { get; set; }
        public DateTime? WhatsappSended { get; set; }
        public DateTime? PushSended { get; set; }
        public Int64? RealEstateRegisteredNumber { get; set; }
        public Int32? IdUser { get; set; }
        public Int32? IdSendInviteTextType { get; set; }
        public RealEstate RealEstate { get; set; }
        public User User { get; set; }
        public SendInviteTextType SendInviteTextType { get; set; }
    }
}