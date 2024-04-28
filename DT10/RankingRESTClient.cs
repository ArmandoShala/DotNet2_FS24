using System.Text.Json;

namespace DT10;

public class RankingRESTClient
{
    public static async Task Main()
    {
        var client = new HttpClient();
        var response = await client.GetAsync("https://localhost:7088/RankingRESTClient");
        var json = await response.Content.ReadAsStringAsync();
        var competitors = JsonSerializer.Deserialize<List<Competitor>>(json);
        competitors?.ToList().ForEach(c => Console.WriteLine(c));
    }
}
public abstract class Competitor
{
    protected Competitor(string name, string time)
    {
        Name = name;
        Time = time;
    }

    private string Name { get; set; }
    private string Time { get; set; }

    public override string ToString() => $"{Name} {Time}";
}
