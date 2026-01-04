using Com.ByteAnalysis.IFacilita.Chat.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Com.ByteAnalysis.IFacilita.Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        Service.ITransactionService transactionService;
        private readonly IHubContext<Extensions.ChatHub> _hubContext;

        public ChatController(Service.ITransactionService transactionService,
            IHubContext<Extensions.ChatHub> _hubContext)
        {
            this.transactionService = transactionService;
            this._hubContext = _hubContext;
        }

        [HttpGet]
        public IActionResult get()
        {
            return Ok(this.transactionService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(this.transactionService.Get(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Transaction transaction)
        {
            transaction = this.transactionService.Create(transaction);

            return Ok(new { id = transaction.Id });
        }

        //[HttpPost]
        //public IActionResult Post([FromBody] object user)
        //{
        //    //var user = JsonConvert.DeserializeObject<User>(Context.GetHttpContext().Request.Query["user"]);
        //    //_connections.Add(Context.ConnectionId, user);
        //    ////Ao usar o método All eu estou enviando a mensagem para todos os usuários conectados no meu Hub
        //    //Clients.All.SendAsync("chat", _connections.GetAllUser(), user);

        //    return Ok();
        //}

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Transaction transaction, string id)
        {
            if (transactionService.Get(id) == null)
                return NotFound();

            this.transactionService.Update(id, transaction);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromBody] string id)
        {
            if (transactionService.Get(id) == null)
                return NotFound();

            this.transactionService.Remove(id);

            return Ok();
        }
    }
}
