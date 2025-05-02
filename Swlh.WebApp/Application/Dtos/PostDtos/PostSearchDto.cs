namespace Swlh.WebApp.Application.Dtos.PostDtos;

public class PostSearchDto
{
    public string? PartOfTitle { get; set; }
    public List<string> TagNames { get; set; } = [];
}
