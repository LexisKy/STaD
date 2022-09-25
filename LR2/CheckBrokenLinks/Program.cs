using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net;


namespace CheckBrokenLinks
{
    public class LinkResearch
    {
        private readonly Uri _baseLink;
        public List<string> _visitedLinks = new List<string>();
        public List<LinkResponseCode> _validLink = new List<LinkResponseCode>();
        public List<LinkResponseCode> _invalidLink = new List<LinkResponseCode>();
        public DateTime _checkTime;

        public LinkResearch(string baseLink)
        {
            _baseLink = new Uri(baseLink);
        }

        public struct LinkResponseCode
        {
            public string Url { get; set; }
            public HttpStatusCode Code { get; set; }
        }

        public struct WebSiteResponse
        {
            public HttpStatusCode Code { get; set; }
            public string Html { get; set; }
        }

        private WebSiteResponse LoadPage(string url)
        {
            WebSiteResponse webSiteResponse = new WebSiteResponse();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                var streamResponse = response.GetResponseStream();
                StreamReader stream = new StreamReader(streamResponse);
                string result = stream.ReadToEnd();
                webSiteResponse.Html = result;
                webSiteResponse.Code = response.StatusCode;
            }
            catch (WebException exception)
            {
                webSiteResponse.Code = ((HttpWebResponse)exception.Response).StatusCode;
            }

            return webSiteResponse;
        }

        private List<string> GetAllUrlsFromPage(string html)
        {
            List<string> urlsList = new List<string>();
            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(html);

            HtmlNodeCollection urlsCollection = doc.DocumentNode.SelectNodes("//a[@href]");
            foreach(HtmlNode link in urlsCollection)
            {
                string attribute = link.GetAttributeValue("href", "");
                string url = "";

                if (Uri.IsWellFormedUriString(attribute, UriKind.Absolute))
                {
                    url = new Uri(attribute, UriKind.Absolute).AbsoluteUri;
                }
                else if (Uri.IsWellFormedUriString(attribute, UriKind.Relative))
                {
                    Uri relative = new Uri(attribute, UriKind.Relative);
                    url = new Uri(_baseLink, relative).AbsoluteUri;
                }

                if (!urlsList.Contains(url) && (url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("ftp://")))
                {
                    urlsList.Add(url);
                }
            }
            return urlsList;
        }

        private bool IsCorrectDomain(string urlItem)
        {
            Uri url = new Uri(urlItem);
            return url.Host == _baseLink.Host;
        }
        private void FindAndSortUrls(string url)
        {
            WebSiteResponse response = LoadPage(url);

            _visitedLinks.Add(url);

            if(response.Code == HttpStatusCode.OK)
            {
                LinkResponseCode linkResponse = new LinkResponseCode {Code = response.Code, Url = url };
                _validLink.Add(linkResponse);
                List<string> linkList = new List<string>();
                if(IsCorrectDomain(url))
                {
                    linkList = GetAllUrlsFromPage(response.Html);
                }
                foreach(string item in linkList)
                {
                    if(!_visitedLinks.Contains(item))
                    {
                        FindAndSortUrls(item);
                    }
                }
            }
            else
            {
                LinkResponseCode invalidLinkResponseCode = new LinkResponseCode { Code = response.Code, Url = url };
                _invalidLink.Add(invalidLinkResponseCode);
            }
        }

        public void UrlsResearchProcess()
        {
            _invalidLink.Clear();
            _validLink.Clear();
            FindAndSortUrls(_baseLink.AbsoluteUri);
            _visitedLinks.Clear();
            _checkTime = DateTime.Now;
        }

        private void WriteDataToFile(string filePath, List<LinkResponseCode> data)
        {
            StreamWriter fileData = new StreamWriter("../../../" + filePath, false);
            foreach (var item in data)
            {
                fileData.WriteLine(item.Url + " " + (int)item.Code);
            }
            fileData.WriteLine($"Kоличество ссылок: {_validLink.Count}");
            fileData.WriteLine($"Дата проверки: {_checkTime}");
            fileData.Close();
        }

        public void WriteDataToFiles(string validUrlsFilePath, string invalidUrlsFilePath)
        {
            WriteDataToFile(validUrlsFilePath, _validLink);
            WriteDataToFile(invalidUrlsFilePath, _invalidLink);
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            LinkResearch linkResearch = new LinkResearch("http://links.qatl.ru/");
            linkResearch.UrlsResearchProcess();
            linkResearch.WriteDataToFiles("valid.txt", "invalid.txt");
        }
    }
}