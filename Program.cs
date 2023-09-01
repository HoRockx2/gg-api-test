using Newtonsoft.Json.Linq;


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// https://data.gg.go.kr/portal/data/service/selectServicePage.do?page=1&rows=10&sortColumn=&sortDirection=&infId=APXKY127QXG0TVVR1Y6E28683342&infSeq=2&order=&loc=&searchWord=%EB%89%B4%EC%8A%A4

const string url = @"https://openapi.gg.go.kr/GgNewsDataPortal";
const string KEY = "fc9a27c97426452ab7f3fb689df6d2c1";

var _query = new Dictionary<string, string>
{
    { "KEY", KEY },
    { "Type", "json" },
    { "pIndex", "1" },
    { "pSize", "10" },
    {"REGIST_DTM", "2023-09-01"}
};

// make query string by _query
var query = string.Join("&", _query.Select(x => $"{x.Key}={x.Value}"));

// combine url and query
var urlWithQuery = $"{url}?{query}";

// send get request to url with HttpClient
var client = new HttpClient();
var response = await client.GetAsync(urlWithQuery);

// parser response to json
var responseString = await response.Content.ReadAsStringAsync();
var json = JObject.Parse(responseString);

// print keys in json
foreach (var item in json)
{
    Console.WriteLine(item.Key);
}

// get [ggnewsdataportal][head][list_total_count] value
var totalCount = json["GgNewsDataPortal"][0]["head"][0]["list_total_count"];

Console.WriteLine($"total count: {totalCount}");

// get JArray from [ggnewsdataportal][row] from json
var rows = json["GgNewsDataPortal"][1]["row"] as JArray;

// parse JArray to List<News>
var newsList = rows.ToObject<List<News>>();

// print newsList
foreach (var news in newsList)
{
    Console.WriteLine($"{news.INST_NM} {news.REGIST_DTM} {news.TITLE} {news.LINK_URL}");
}