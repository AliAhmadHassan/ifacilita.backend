using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class LandpageService : ILandpageService
    {
        Entity.ISmtpSettings smtp;

        public LandpageService(Entity.ISmtpSettings smtp)
        {
            this.smtp = smtp;
        }
        public void contact(object entity)
        {
            System.Text.Json.JsonElement element = (System.Text.Json.JsonElement)entity;
            string name = element.GetProperty("name").GetString();
            string email = element.GetProperty("email").GetString();
            string message = element.GetProperty("message").GetString();

            string body = $@"<h1>Contato pela Landpage</h1>
Nome: {name}<br/>
Email: {email}<br/>
<hr/>
{message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>")}";

            this.smtp.SendEmail("ali.hassan@byteanalysis.com.br", "Landpage - Contato", body);
            this.smtp.SendEmail("eu@brunoparodi.com", "Landpage - Contato", body);
            this.smtp.SendEmail("jfdconsult@gmail.com", "Landpage - Contato", body);
        }
    }
}
