using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using html_scrap.Models;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace html_scrap.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public List<string> _hrefs = new List<string>();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        string url = "https://www.w3schools.com/";
        var response = CallURL(url).Result;
        var linkList = ParseHTML(response);

        ViewBag.URLs = linkList;

        return View(linkList);
    }


    private static async Task<string> CallURL(string fullURL)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetStringAsync(fullURL);
        return response;
    }

    private List<string> ParseHTML(string html)
    {
        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);


        //var programmerLinks = htmlDoc.DocumentNode.Descendants("li")
        //.Where(node => !node.GetAttributeValue("class", "").Contains("tocsection"))
        //.ToList();

        var extractedURLs = htmlDoc.DocumentNode.SelectNodes("//a").ToList();


        List<string> _Link = new List<string>();


        for (int i = 0; i < extractedURLs.Count; i++)
        {
            _Link.Add(extractedURLs[i].FirstChild.OuterHtml);
                    //.FirstChild.Attributes[0].Value);
        }


        //foreach (var link in extractedURLs)
        //{
            //if (link.FirstChild.Attributes.Count > 0)
            //{
            //_Link.Add("https://www.w3schools.com" + link.FirstChild.Attributes[0].Value);

            //}
        //}

        return _Link;

    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

