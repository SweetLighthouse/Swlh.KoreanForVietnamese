namespace Swlh.WebApp.Application.Dtos.NaverVikoDtos;

public class SearchMaybek
{
    public string? query { get; set; }
    public string? revert { get; set; }
    // orKEquery
    public string? forceQuery { get; set; }
    // errataQuery
    public bool? compatibilityJamo { get; set; }
}
