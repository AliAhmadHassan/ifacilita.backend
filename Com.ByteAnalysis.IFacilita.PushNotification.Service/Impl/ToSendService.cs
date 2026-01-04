using Com.ByteAnalysis.IFacilita.PushNotification.Model;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Service.Impl
{
    public class ToSendService : IToSendService
    {

        Repository.IToSendRepository repository;
        Repository.IUserRepository userRepository;
        IApplicationSettings applicationSettings;

        public ToSendService(Repository.IToSendRepository repository,
            IApplicationSettings applicationSettings,
            Repository.IUserRepository userRepository)
        {
            this.repository = repository;
            this.applicationSettings = applicationSettings;
            this.userRepository = userRepository;
        }


        public void CreateOrUpdate(string iduser, Model.Notification notification)
        {
            Model.User user = this.userRepository.Get(iduser);

            foreach (var token in user.Tokens)
            {
                Message message = new Message()
                {
                    Notification = notification,
                    To = token
                };

                ToSend toSend = new ToSend()
                {
                    Message = message,
                    User = user,
                    Created = DateTime.Now
                };

                try
                {
                    if (!Send(toSend))
                        this.repository.CreateOrUpdate(toSend);

                } 
                catch(Exception error)
                {
                    this.repository.CreateOrUpdate(toSend);
                }
            }
        }

        public ToSend Get(string id)
        {
            return this.repository.Get(id);
        }

        public IEnumerable<ToSend> Get()
        {
            return this.repository.Get();
        }

        public void Remove(string Id)
        {
            this.repository.Remove(Id);
        }

        public bool Send()
        {
            bool isSuccess = true;
            IEnumerable<Model.ToSend> toSends = Get();
            foreach (var toSend in toSends)
            {
                if (!Send(toSend))
                    isSuccess = false;
            }
            return isSuccess;
        }

        public bool Send(Model.ToSend toSend)
        {
            bool isSuccess = true;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string key = $"key={this.applicationSettings.PushNotificationToken}";

                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", key);

                HttpResponseMessage respToken = client.PostAsync("https://fcm.googleapis.com/fcm/send", new StringContent(
                        JsonConvert.SerializeObject(new {
                            notification = new{
                                title = toSend.Message.Notification.Title,
                                body = toSend.Message.Notification.Body,
                                icon = toSend.Message.Notification.Icon,
                                click_action = toSend.Message.Notification.ClickAction
                            },
                            to = toSend.Message.To

                        }), Encoding.UTF8, "application/json")).Result;

                string conteudo2 =
                    respToken.Content.ReadAsStringAsync().Result;

                Console.WriteLine(conteudo2);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    PushNotificationReturned returned = JsonConvert.DeserializeObject<PushNotificationReturned>(conteudo2);
                    Model.User user = toSend.User;

                    if (user.Sendeds == null) 
                        user.Sendeds = new List<Sended>();

                    user.Sendeds.Add(new Sended()
                    {
                        Created = toSend.Created,
                        IdToSend = toSend.Id,
                        DateOfSended = DateTime.Now,
                        Message = toSend.Message,
                        PushNotificationReturned = returned
                    });

                    this.userRepository.CreateOrUpdate(user);
                }
                else
                {

                }
            }


            return isSuccess;
        }


    }
}
