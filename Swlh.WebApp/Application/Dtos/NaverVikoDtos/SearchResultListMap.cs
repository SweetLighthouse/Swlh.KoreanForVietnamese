namespace Swlh.WebApp.Application.Dtos.NaverVikoDtos;

public class SearchResultListMap
{
    public SearchResultListMapType<WordItemType>? WORD { get; set; }
    public SearchResultListMapType<WordItemType>? OPEN { get; set; }
    public SearchResultListMapType<WordItemType>? IDIOM { get; set; }
    public SearchResultListMapType<WordItemType>? MEANING { get; set; }
    public SearchResultListMapType<ExampleItemType>? EXAMPLE { get; set; }
    public SearchResultListMapType<WordItemType>? VLIVE { get; set; }
}
