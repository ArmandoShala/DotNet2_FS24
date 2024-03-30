using DT2;
using System.Diagnostics;
using System.Windows;

namespace DT3;

public partial class MainWindow
{
    public UrlTesterModel urlTesterModel = new UrlTesterModel();
    int bytes;
    int TimeInMs;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = urlTesterModel;
    }

    private void GetUrlButton_Click(object sender, RoutedEventArgs e)
    {
    }

    private void GetUrlButton_Click(object sender, RoutedEventArgs e)
    {
        //urlTesterModel.Url = UrlText.Text;
        var urlTester = new UrlTester();
        var stopWatch = new Stopwatch();

        urlTester.PageStart += url => stopWatch.Start();
        urlTester.PageLoaded += (url, bytes) =>
        {
            stopWatch.Stop();

            Dispatcher.Invoke(() =>
            {
                LoadedBytes.Text = $"{bytes} bytes";
                LoadingTime.Text = $"{stopWatch.ElapsedMilliseconds} ms";
                LoadingProgressBar.Value = stopWatch.ElapsedMilliseconds;
            });
        };

        urlTester.GetUrlAsync(UrlText.Text);
    }

    private void GetUrlButton_Click(object sender, RoutedEventArgs e)
    {
        urlTesterModel.Url = UrlText.Text;
        var urlTester = new UrlTester();

        var stopWatch = new Stopwatch();
        urlTester.PageStart += url => stopWatch.Start();
        urlTester.PageLoaded += (url, bytes) =>
        {
            stopWatch.Stop();
            Dispatcher.Invoke(() =>
            {
                bytes = bytes;
                TimeInMs = (int) stopWatch.ElapsedMilliseconds;
            });
        };

        urlTester.GetUrlAsync(UrlText.Text);
    }
}