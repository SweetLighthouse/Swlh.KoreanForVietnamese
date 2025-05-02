using System.Text.Json.Serialization;

namespace Swlh.WebApp.Application.Dtos.NavItems;

public class NavLink
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty; // required

    [JsonPropertyName("href")]
    public string Href { get; set; } = string.Empty; // required
}
