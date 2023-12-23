using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MLauncher.src
{
    internal class MPQDetector
    {
        private readonly HttpClient _httpClient = new();

        private readonly List<string> _mpqsWeb = new();
        private readonly List<string> _mpqsDrive = new();

        private readonly string _url = "https://mythicforge.online/vc/mpq/";

        public MPQDetector()
        {
            _httpClient
                .DefaultRequestHeaders
                .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        async void Main()
        {
            var task1 = await DetectWebMpq();
            var task2 = DetectDriveMpq();
        }

        public async Task<List<string>> DetectWebMpq()
        {
            var response = await _httpClient.GetAsync(new Uri(_url));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var rawData = content.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                rawData.ToList().ForEach((string mpq) =>
                {
                    if (mpq.StartsWith("Patch") && mpq.EndsWith(".mpq"))
                    {
                        _mpqsWeb.Add(mpq);
                    }
                });
            }
            return _mpqsWeb;
        }

        public List<string> DetectDriveMpq()
        {
            var path = Path.Combine(Downloader._documentsPath, "/mythicforge/Data/");
            var files = Directory.GetFiles(path)
                .Where(name => name.StartsWith("Patch") && name.EndsWith(".mpq"))
                .ToList();
            
            _mpqsDrive.AddRange(files);
            return _mpqsDrive;
        }

        public bool CompareWebToDrive()
        {
            return _mpqsWeb.OrderBy(x => x).SequenceEqual(_mpqsDrive.OrderBy(x => x));
        }
    }
}