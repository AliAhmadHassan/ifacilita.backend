using Com.ByteAnalysis.IFacilita.Chat.Model;
using Com.ByteAnalysis.IFacilita.Chat.Service;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Chat.API.Extensions
{
    public class ChatHub : Hub
    {
        private IConnectionsService _connections;
        private readonly ITransactionService _transactionService;

        public ChatHub(IConnectionsService _connections, ITransactionService transactionService)
        {
            this._connections = _connections;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Override para inserir cada usuário no nosso repositório, lembrando que esse repositório está em memória
        /// </summary>
        /// <returns> Retorna lista de usuário no chat e usuário que acabou de logar </returns>
        public override Task OnConnectedAsync()
        {
            //var user = JsonConvert.DeserializeObject<User>(Context.GetHttpContext().Request.Query["user"]);
            //_connections.Add(Context.ConnectionId, user);
            ////Ao usar o método All eu estou enviando a mensagem para todos os usuários conectados no meu Hub
            //Clients.All.SendAsync("chat", _connections.GetAllUser(), user);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Método responsável por encaminhar as mensagens pelo hub
        /// </summary>
        /// <param name="ChatMessage">Este parâmetro é nosso objeto representando a mensagem e os usuários envolvidos</param>
        /// <returns></returns>
        public async Task SendMessage(Model.Message message)
        {
            //Ao usar o método Client(_connections.GetUserId(chat.destination)) eu estou enviando a mensagem apenas para o usuário destino, não realizando broadcast
            var userTo = _connections.GetConnectionId(message.UserTo.UserId);
            var userFrom = _connections.GetConnectionId(message.UserFrom.UserId);

            var transaction = _transactionService.Get(message.TransactionId.ToString());

            message.MessageDate = DateTime.Now;

            if (transaction == null)
            {
                transaction = new Transaction() { Messages = new List<Message>(), Users = new List<User>() };
                transaction.Id = message.TransactionId.ToString().PadLeft(24, '0');

                transaction.Users.Add(new User() { Nome = message.UserTo.Nome, UserId = message.UserTo.UserId });
                transaction.Users.Add(new User() { Nome = message.UserFrom.Nome, UserId = message.UserFrom.UserId });

                transaction.Messages.Add(message);
                _transactionService.Create(transaction);
            }
            else
            {
                if (transaction.Messages == null)
                    transaction.Messages = new List<Message>();

                transaction.Messages.Add(message);
                _transactionService.Update(transaction.Id, transaction);
            }

            if (!string.IsNullOrEmpty(userTo))
                await Clients.Client(userTo).SendAsync("Receive", message);

            if (!string.IsNullOrEmpty(userFrom))
                await Clients.Client(userFrom).SendAsync("Receive", message);

            //await Clients.All.SendAsync("Receive", message);

        }

        public void Login(Model.User user)
        {
            _connections.Add(Context.ConnectionId, user);
            Clients.All.SendAsync("chat", _connections.GetAllUser(), user);
            Clients.Client(Context.ConnectionId).SendAsync("chat", _connections.GetAllUser(), user);
        }
    }
}
