using System.ComponentModel;
using System.Diagnostics;

namespace DT3;

interface IUrlTester {
    public void GetUrlSync();
    public Task GetUrlAsync(string url);
    public int Size { get; }
    public int Time { get; }
    public string Url { get; set; }
}

interface INotifyUrlTesting : IUrlTester, INotifyPropertyChanged { }

public class UrlTesterModel : INotifyUrlTesting {
    
    private int _size;
    private int _time;
    private string _url;
    private readonly DT2.UrlTester _urlTester;
    public event PropertyChangedEventHandler? PropertyChanged;


    public UrlTesterModel() {
        _urlTester = new DT2.UrlTester();
        IDictionary<string, Stopwatch> sw = new Dictionary<string, Stopwatch>();
        _urlTester.PageStart += url => { sw.Add(url, Stopwatch.StartNew()); };
        _urlTester.PageLoaded += (url, size) => {
            sw[url].Stop();
            int time = (int)sw[url].Elapsed.TotalMilliseconds;
            Size = size;
            Time = time;
            sw.Remove(url);
        };
        PropertyChanged += OnPropertyChanged_URL;
    }

    private void OnPropertyChanged_URL(object? s, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Url") _urlTester.GetUrlSync(Url);
    }

    public void GetUrlSync() => _urlTester.GetUrlSync(_url);

    public async Task GetUrlAsync(string u) => await _urlTester.GetUrlAsync(u);

    public int Size
    {
        get => _size;
        set { _size = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Size")); }
    }

    public int Time
    {
        get => _time;
        set { _time = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time")); }
    }

    public string Url
    {
        get => _url;
        set { _url = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Url")); }
    }

}