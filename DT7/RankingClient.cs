namespace DT7;

class RankingClien
{
    static void Main(string[] args)
    {
        var client = new RankingServiceClient();
        var rankingListTask = client.RankingListAsync();
        var rankingList = rankingListTask.Result;
        rankingList.ForEach(r => Console.WriteLine($"Name: {r.Name}\nTime: {r.Time}\n"));
        Console.ReadLine();
    }
}
