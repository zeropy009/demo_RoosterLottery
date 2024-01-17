namespace RoosterLottery_Server.Model
{
    public class LotteryResult
    {
        public int WinningNumber { get; set; }

        public int CountUser { get; set; }

        public List<string?> Winners { get; set;} = new List<string?>();
    }
}
