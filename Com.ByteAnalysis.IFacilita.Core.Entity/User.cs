using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class User: BasicEntity
    {
        public Int32 Id { get; set; }
        public Int64? SocialSecurityNumber { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String Password { get; set; }
        public String EMail { get; set; }
        public Int16? DDI { get; set; }
        public Int16? DDD { get; set; }
        public Int64? MobileNumber { get; set; }
        public String IdentityCard { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public String SocialLoginAuthorizationCode { get; set; }
        public Int32? iddefailtTransaction { get; set; }
        public Int32? IdUserProfile { get; set; }
        public Int32? IdAddress { get; set; }
        public String BrokerRegistrationNumber { get; set; }
        public Int32? IdUserSpouseType { get; set; }
        public Int64? UserSpouseSocialSecurityNumber { get; set; }
        public Int32? IdUserBankData { get; set; }
        public string Nationality { get; set; }
        public UserProfile UserProfile { get; set; }
        public Address Address { get; set; }
        public Broker Broker { get; set; }
        public UserSpouseType UserSpouseType { get; set; }
        public UserSpouse UserSpouse { get; set; }
        public UserBankData UserBankData { get; set; }
        public string PushNotification { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string PaymentExtenalClientId { get; set; }
        public long? PaymentClientId { get; set; }
    }
}
