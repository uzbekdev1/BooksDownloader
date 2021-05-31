using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using HtmlAgilityPack;

namespace BooksDownloader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
            Console.OutputEncoding = Encoding.UTF8;

            var client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            var page = new HtmlWeb
            {
                OverrideEncoding = Encoding.UTF8
            };

            for (var i = 1; i <= 1000000; i++)
                try
                {
                    var previewUrl = $"http://sd.blackball.lv/books/{i}";
                    var doc = page.Load(previewUrl);
                    var title = doc.DocumentNode.SelectSingleNode("//h2[@class='item-title']").InnerHtml.Trim();
                    var filePath = Path.Combine(Environment.CurrentDirectory, "tmp", $"{title}.pdf");

                    if (File.Exists(filePath)) File.Delete(filePath);

                    client.DownloadFile($"http://sd.blackball.lv/DownloadFile.ashx?file={i}", filePath);

                    Console.WriteLine(title);
                }
                catch (Exception)
                {
                    // ignored
                }

            Console.WriteLine("Done!");
        }
    }
}