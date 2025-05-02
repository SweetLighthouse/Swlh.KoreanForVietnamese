using System.Text.Json.Serialization;

namespace Swlh.WebApp.Application.Dtos.NavItems;

public class NavItem
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty; // required


    // either href or dropdown must be present, or both
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    [JsonPropertyName("dropdown")]
    public List<NavLink>? Dropdown { get; set; }

    public bool IsValid() =>
        !string.IsNullOrWhiteSpace(Text) && (Href != null || (Dropdown?.Count == 0));
}
