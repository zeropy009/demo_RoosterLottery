using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RoosterLottery_Server.Model;

namespace demo_RoosterLottery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoosterLotteryController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;

        public RoosterLotteryController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet(Name = "GetLotteryResults")]
        public IEnumerable<LotteryResult> Get()
        {
            List<LotteryResult> lotteryResults = new List<LotteryResult>();
            memoryCache.TryGetValue("LotteryResults", out lotteryResults);
            return lotteryResults;
        }

        [HttpPost(Name = "PostTicket")]
        public IActionResult Post(Ticket ticket)
        {
            string dateTime = DateTime.Now.AddHours(1).ToString("HHddMMyyyy");
            List<Ticket> tickets = new List<Ticket>();
            memoryCache.TryGetValue(dateTime, out tickets);
            tickets.Add(ticket);
            memoryCache.Set(dateTime, tickets);
            return Ok();
        }
    }
}