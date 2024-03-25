using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using DT2;

namespace DT3 {

    public class UrlTesterModel /* TO BE DONE */ {
        public event PropertyChangedEventHandler PropertyChanged;

        int size;
        int time;
        string url;
        DT2.UrlTester urlTester;

        public UrlTesterModel() {
            urlTester = new DT2.UrlTester();
            IDictionary<string, Stopwatch> sw = new Dictionary<string, Stopwatch>();
            urlTester.PageStart += url => { sw.Add(url, Stopwatch.StartNew()); };
            urlTester.PageLoaded += (url, size) => {
                sw[url].Stop();
                int time = (int)sw[url].Elapsed.TotalMilliseconds;
                Size = size;
                Time = time;
                sw.Remove(url);
            };
        }

        public int Size {
            get { return size; }
            set /* TO BE DONE */
        }

        public int Time {
            get { return time; }
            set /* TO BE DONE */
        }

        public string Url {
            get { return url; }
            set /* TO BE DONE */
        }
    }

}