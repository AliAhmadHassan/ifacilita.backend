using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class SendInviteRepository : ISendInviteRepository
    {
        IDatabaseSettings databaseSettings;

        public SendInviteRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_send_invite", new { id });
            }
        }

        public SendInvite Insert(SendInvite entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    created = entity.Created,
                    e_mail_sended = entity.EMailSended,
                    s_m_s_sended = entity.SMSSended,
                    whatsapp_sended = entity.WhatsappSended,
                    push_sended = entity.PushSended,
                    real_estate_registered_number = entity.RealEstateRegisteredNumber,
                    iduser = entity.IdUser,
                    idsend_invite_text_type = entity.IdSendInviteTextType
                };

                entity.Id = conn.ExecuteScalar<int>("spi_send_invite", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public SendInvite Update(SendInvite entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    created = entity.Created,
                    e_mail_sended = entity.EMailSended,
                    s_m_s_sended = entity.SMSSended,
                    whatsapp_sended = entity.WhatsappSended,
                    push_sended = entity.PushSended,
                    real_estate_registered_number = entity.RealEstateRegisteredNumber,
                    iduser = entity.IdUser,
                    idsend_invite_text_type = entity.IdSendInviteTextType
                };

                conn.Execute("spu_send_invite", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<SendInvite> FindAll()
        {
            IEnumerable<SendInvite> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<SendInvite>("sps_send_invite");
            }
            return entities;
        }

        public SendInvite FindById(int id)
        {
            SendInvite entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<SendInvite, RealEstate, User, SendInviteTextType, SendInvite>("sps_send_invite_by_pk", param: new { id }, map: (sendInvite, realEstate, user, sendInviteTextType)=>{
                       sendInvite.RealEstate = realEstate;
                       sendInvite.User = user;
                       sendInvite.SendInviteTextType = sendInviteTextType;
                       return sendInvite;
                    }, 
                    splitOn: "real_estate.registered_number, user.id, send_invite_text_type.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
		public List<SendInvite> FindByRealEstateRegisteredNumber (Int64 RealEstateRegisteredNumber){throw new NotImplementedException();}

        public List<SendInvite> FindByIdUser(int IdUser)
        {
            throw new NotImplementedException();
        }
    }
}