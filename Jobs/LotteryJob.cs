using Microsoft.Extensions.Caching.Memory;
using Quartz;
using RoosterLottery_Server.Model;

namespace demo_RoosterLottery.Jobs
{
    [DisallowConcurrentExecution]
    public class LotteryJob : IJob
    {
        private readonly IMemoryCache memoryCache;

        public LotteryJob(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Random random = new Random();
            int winningNumber = random.Next(0, 10);
            List<Ticket> tickets = GetTicket();
            // Find winners
            List<Ticket> winners = tickets.Where(ticket => ticket.SelectedNumber == winningNumber).ToList();

            List<LotteryResult> lotteryResults;
            memoryCache.TryGetValue("LotteryResults", out lotteryResults);
            if (lotteryResults == null) lotteryResults = new List<LotteryResult>();
            LotteryResult newLotteryResult = new LotteryResult() { WinningNumber = winningNumber, CountUser = tickets.Count, Winners = winners.Select(winner => winner.PhoneNumber).ToList()};
            lotteryResults.Add(newLotteryResult);
            memoryCache.Set("LotteryResults", lotteryResults);
            return Task.CompletedTask;
        }

        private List<Ticket> GetTicket()
        {
            string dateTime = DateTime.Now.ToString("HHddMMyyyy");
            List<Ticket> tickets;
            memoryCache.TryGetValue(dateTime, out tickets);
            return tickets ?? new List<Ticket>();
        }
    }
}
