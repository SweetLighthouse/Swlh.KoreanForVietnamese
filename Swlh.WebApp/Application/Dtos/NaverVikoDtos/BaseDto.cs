namespace Swlh.WebApp.Application.Dtos.NaverVikoDtos;

public class BaseDto
{
    public SearchResultMap? searchResultMap { get; set; }
    public string? mode { get; set; }
    public bool? exactMatcheEntryUrl { get; set; }
    public List<string?>? collectionRanking { get; set; }
    public bool? LAIMLog { get; set; }
    public string? query { get; set; }
    public string? range { get; set; }
    public SearchMaybek? searchMaybek { get; set; }
}
