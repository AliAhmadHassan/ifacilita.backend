using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        IDatabaseSettings databaseSettings;

        public UserRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_user", new { id });
            }
        }

        public User Insert(User entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    social_security_number = entity.SocialSecurityNumber,
                    name = entity.Name,
                    last_name = entity.LastName,
                    password = entity.Password,
                    e_mail = entity.EMail,
                    ddi = entity.DDI,
                    ddd = entity.DDD,
                    mobile_number = entity.MobileNumber,
                    identity_card = entity.IdentityCard,
                    date_of_birth = entity.DateOfBirth,
                    social_login_authorization_code = entity.SocialLoginAuthorizationCode,
                    iddefailt_transaction = entity.iddefailtTransaction,
                    iduser_profile = entity.IdUserProfile,
                    idaddress = entity.IdAddress,
                    broker_registration_number = entity.BrokerRegistrationNumber,
                    iduser_spouse_type = entity.IdUserSpouseType,
                    user_spouse_social_security_number = entity.UserSpouseSocialSecurityNumber,
                    iduser_bank_data = entity.IdUserBankData,
                    nationality = entity.Nationality,
                    push_notification = entity.PushNotification,
                    father_name = entity.FatherName,
                    mother_name = entity.MotherName,
                    payment_clientid = entity.PaymentClientId,
                    payment_clientexternalid = entity.PaymentExtenalClientId
                };

                entity.Id = conn.ExecuteScalar<int>("spi_user", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public User Update(User entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    social_security_number = entity.SocialSecurityNumber,
                    name = entity.Name,
                    last_name = entity.LastName,
                    password = entity.Password,
                    e_mail = entity.EMail,
                    ddi = entity.DDI,
                    ddd = entity.DDD,
                    mobile_number = entity.MobileNumber,
                    identity_card = entity.IdentityCard,
                    date_of_birth = entity.DateOfBirth,
                    social_login_authorization_code = entity.SocialLoginAuthorizationCode,
                    iddefailt_transaction = entity.iddefailtTransaction,
                    iduser_profile = entity.IdUserProfile,
                    idaddress = entity.IdAddress,
                    broker_registration_number = entity.BrokerRegistrationNumber,
                    iduser_spouse_type = entity.IdUserSpouseType,
                    user_spouse_social_security_number = entity.UserSpouseSocialSecurityNumber,
                    iduser_bank_data = entity.IdUserBankData,
                    nationality = entity.Nationality,
                    push_notification = entity.PushNotification,
                    father_name = entity.FatherName,
                    mother_name = entity.MotherName,
                    payment_clientid = entity.PaymentClientId,
                    payment_clientexternalid = entity.PaymentExtenalClientId
                };

                conn.Execute("spu_user", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<User> FindAll()
        {
            IEnumerable<User> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<User>("sps_user");
            }
            return entities;
        }

        public User FindById(int id)
        {

            User entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<User, UserProfile, Address, Broker, UserSpouseType, UserSpouse, UserBankData, User>("sps_user_by_pk", param: new { id }, map: (user, userProfile, address, broker, userSpouseType, userSpouse, userBankData)=>{
                       user.UserProfile = userProfile;
                       user.Address = address;
                       user.Broker = broker;
                       user.UserSpouseType = userSpouseType;
                       user.UserSpouse = userSpouse;
                       user.UserBankData = userBankData;
                       return user;
                    }, 
                    splitOn: "user_profile.id, address.id, broker.registration_number, user_spouse_type.id, user_spouse.social_security_number, user_bank_data.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
		public List<User> FindByIdUserProfile (Int32 IdUserProfile){throw new NotImplementedException();}

        public User FindByEMail(string email)
        {
            User entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<User>("sps_user_by_email", new { email }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public User FindBySocialLoginAuthorizationCode(string authorizationCode)
        {
            User entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<User>("sps_user_by_social_login_authorization_code", new { social_login_authorization_code = authorizationCode }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public User FindByMobile(short ddd, long mobile_number)
        {
            User entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<User>("sps_user_by_ddd_mobile_number", new { ddd, mobile_number }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public User FindBySocialSecurityNumber(long socialSecurityNumber)
        {
            User entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<User>("sps_user_by_social_security_number", new { social_security_number = socialSecurityNumber }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }
    }
}