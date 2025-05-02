using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swlh.WebApp.Application.Dtos.NaverVikoDtos;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;
using System.Net;
using System.Text.RegularExpressions;

namespace Swlh.WebApp.Controllers;

public class SearchController(MainDbContext context, IHttpClientFactory httpClientFactory) : Controller
{
    private readonly HttpClient naverClient = httpClientFactory.CreateClient("NaverDict");
    private readonly HttpClient krClient = httpClientFactory.CreateClient("KrDict");

    public async Task<IActionResult> Index(string? query)
    {
        if (string.IsNullOrWhiteSpace(query)) return View();
        query = query.Trim();

        var keywordEntry = await context.Keywords.FindAsync(query);
        var searchResult = new SearchMaybek();

        if (keywordEntry != null)
        {
            keywordEntry.SearchCount += 1;
            context.Entry(keywordEntry).State = EntityState.Modified;
        }
        else
        {
            context.Keywords.Add(new Keyword { Key = query, SearchCount = 1 });

            try
            {
                string searchstring = Uri.EscapeDataString(query);
                var response = await naverClient.GetFromJsonAsync<BaseDto>($"?range=word&page=1&query={searchstring}");

                searchResult = response?.searchMaybek;
                //if (searchResult?.forceQuery != null || searchResult?.revert != null) return View(model: searchResult);

                // but what if the word is not a misspell but the check found no word? simply wrong word.
                //bool found = response?.searchResultMap?.SearchResultListMap?.WORD?.total != 0;
                //if (!found) return View();
            }
            catch
            {
                // cant connect
                return View(model: searchResult);
            }


            List<Word>? words = await GetWordsFromKr(query);
            foreach (var word in words ?? [])
            {
                if (context.Words.Any(w => w.WordKr == word.WordKr && w.WordVn == word.WordVn)) continue;
                context.Words.Add(word);
            }

            List<Example>? examples = await GetExamplesFromNaver(query);
            foreach (var example in examples ?? [])
            {
                if (context.Examples.Any(e => e.Korean == example.Korean && e.Vietnamese == example.Vietnamese)) continue;
                context.Examples.Add(example);
            }
        }

        await context.SaveChangesAsync();
        return View(model: searchResult);  
    }


    private async Task<List<Word>?> GetWordsFromKr(string query)
    {
        try
        {
            string searchstring = Uri.EscapeDataString(query);

            HttpResponseMessage body = await krClient.GetAsync($"?blockCount=100&mainSearchWord={searchstring}");

            List<Word> words = new();

            HtmlDocument doc = new();
            doc.LoadHtml(body.Content.ReadAsStringAsync().Result);
            HtmlNode main = doc.DocumentNode.SelectSingleNode("//div[@class='sub_left']");
            HtmlNodeCollection results = main.SelectNodes("./div[contains(@class, 'search_result')]/dl");

            // maybe there is a mismatch: for example, search string: facebook. naver say its valid, but kr says no.
            if (results == null || results.Count == 0) return words;

            foreach (var result in results)
            {
                var wordKrRaw = result.SelectSingleNode("./dt/a/span[contains(@class, 'word_type1_17')]");
                var wordKr = wordKrRaw?.ChildNodes.FirstOrDefault(n => n.NodeType == HtmlNodeType.Text)?.InnerText.Trim();


                var type = @result.SelectSingleNode("./dt/span[contains(@class, 'word_att_type1')]/span[contains(@class, 'manyLang2')]")?.InnerHtml.Trim();
                var pronunciation = result.SelectSingleNode("./dt/span[not(@*)]")?.InnerHtml.Trim().Trim('(', ')');

                var ddList = result.SelectNodes("./dd");

                for (int i = 0; i < ddList.Count; i += 3)
                {
                    var wordVn = ddList[i]?.ChildNodes.FirstOrDefault(n => n.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(n.InnerText))?.InnerText.Trim().Replace("\r", "").Replace("\n", "");
                    wordVn = Regex.Replace(wordVn ?? "", @"^\d*\.\s*", "");
                    var meaningKr = ddList[i + 1].InnerText.Trim();
                    var meaningVn = ddList[i + 2].InnerText.Trim();

                    if (string.IsNullOrWhiteSpace(wordVn) || string.IsNullOrWhiteSpace(wordKr) || string.IsNullOrWhiteSpace(meaningVn) || string.IsNullOrWhiteSpace(meaningKr))
                    {
                        continue;
                    }

                    words.Add(new Word
                    {
                        WordKr = wordKr,
                        WordVn = wordVn,
                        Type = type ?? "",
                        Pronunciation = pronunciation ?? "",
                        MeaningKr = meaningKr,
                        MeaningVn = meaningVn,
                    });
                }
            }

            // remove duplicates
            words = words.DistinctBy(w => new { w.WordKr, w.WordVn }).ToList();

            return words;
        }
        catch
        {
            return null;
        }
    }


    private async Task<List<Example>?> GetExamplesFromNaver(string query)
    {
        try
        {
            var examples = new List<Example>();

            string searchstring = Uri.EscapeDataString(query);
            int searchCount = 1;
            while (examples.Count < 10 && searchCount <= 30)
            {
                var response = await naverClient.GetFromJsonAsync<BaseDto>($"?range=example&page={searchCount}&query={searchstring}");

                if (response?.searchResultMap?.SearchResultListMap?.EXAMPLE?.items?.Count == 0) break; // no more examples to crawl

                var items = response?.searchResultMap?.SearchResultListMap?.EXAMPLE?.items;

                foreach (var item in items ?? [])
                {
                    var vietnamese = WebUtility.UrlDecode(item?.exampleEncode)?.Trim();
                    var korean = WebUtility.UrlDecode(item?.translationEncode)?.Trim();

                    if (string.IsNullOrWhiteSpace(korean) ||
                        string.IsNullOrWhiteSpace(vietnamese) ||
                        korean.Split(' ').Length < 3 ||
                        vietnamese.Split(' ').Length < 3) continue;

                    examples.Add(new Example
                    {
                        Korean = korean,
                        Vietnamese = vietnamese.ToLower()
                    });
                }

                searchCount++;
            }

            // remove duplicates
            examples = examples.DistinctBy(e => new { e.Korean, e.Vietnamese }).ToList();

            return examples;
        }
        catch
        {
            return null;
        }
    }
}
