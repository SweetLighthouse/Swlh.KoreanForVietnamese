namespace Swlh.WebApp.Application.Dtos.NaverVikoDtos;

public class SearchResultListMapType<T> where T : class
{
    public string? query { get; set; }
    public string? queryRevert { get; set; }
    public List<T?>? items { get; set; }
    public int? total { get; set; }
    public string? sectionType { get; set; }
    // revert
    // orKEquery
    public string? forceQuery { get; set; }
    // errataQuery
    
}
