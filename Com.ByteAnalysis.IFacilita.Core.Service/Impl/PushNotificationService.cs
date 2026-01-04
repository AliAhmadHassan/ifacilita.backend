using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PushNotificationService : IPushNotificationService
    {
        SortedList<int, List<string>> caching = new SortedList<int, List<string>>();

        Repository.IUserPushNotificationRepository repository;
        Repository.IUserRepository userRepository;
        Entity.IApplicationSettings applicationSettings;

        public PushNotificationService(Repository.IUserPushNotificationRepository repository,
            Repository.IUserRepository userRepository,
            Entity.IApplicationSettings applicationSettings)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.applicationSettings = applicationSettings;
        }


        public void AddToken(int iduser, string token)
        {
            if (caching.ContainsKey(iduser) && caching[iduser].Contains(token))
                return;

            if (!caching.ContainsKey(iduser))
                caching.Add(iduser, new List<string>());

            IEnumerable<Entity.UserPushNotification> pushNotifications = this.repository.GetByIdUser(iduser);
            Entity.UserPushNotification userPushNotification = pushNotifications.Where(c => c.Token.Equals(token)).FirstOrDefault();

            if (userPushNotification != null)
            {
                caching[iduser].Add(userPushNotification.Token);
                return;
            }

            userPushNotification = new Entity.UserPushNotification();
            userPushNotification.IdUser = iduser;
            userPushNotification.Token = token;

            using (var client = new HttpClient())
            {
                Entity.User user = this.userRepository.FindById(iduser);
                if (user == null)
                    return;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (user.PushNotification == null || user.PushNotification == "")
                {

                    HttpResponseMessage respToken = client.PostAsync($"{this.applicationSettings.URLPushNotification}user", new StringContent(
                            JsonConvert.SerializeObject(new
                            {
                                name = user.Name,
                                lastName = user.LastName,
                                tokens = new[]
                               {
                               token
                               }

                            }), Encoding.UTF8, "application/json")).Result;

                    string conteudo2 =
                        respToken.Content.ReadAsStringAsync().Result;

                    Console.WriteLine(conteudo2);

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        user.PushNotification = conteudo2.Replace("\"", "");

                        this.userRepository.Update(user);
                        this.repository.Insert(userPushNotification);
                    }
                }
                else
                {
                    HttpResponseMessage respToken = client.PutAsync($"{this.applicationSettings.URLPushNotification}user/{user.PushNotification}/{token}/add-token", new StringContent(
                        JsonConvert.SerializeObject(new{}), Encoding.UTF8, "application/json")).Result;

                    string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(conteudo2);

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        this.repository.Insert(userPushNotification);
                    }
                }
            }
        }

        public void SendMessage(int iduser, string title, string body, string icon, string clickAction)
        {
            using (var client = new HttpClient())
            {
                Entity.User user = this.userRepository.FindById(iduser);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (user.PushNotification == null || user.PushNotification == "")
                {

                    HttpResponseMessage respToken = client.PostAsync($"{this.applicationSettings.URLPushNotification}ToSend/{user.PushNotification}", new StringContent(
                            JsonConvert.SerializeObject(new
                            {
                                title,
                                body,
                                icon,
                                clickAction
                            }), Encoding.UTF8, "application/json")).Result;

                    string conteudo2 =
                        respToken.Content.ReadAsStringAsync().Result;

                    Console.WriteLine(conteudo2);

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                    }
                }
            }
        }
    }
}
