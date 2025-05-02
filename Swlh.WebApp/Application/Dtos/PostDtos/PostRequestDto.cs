using System.ComponentModel.DataAnnotations;

namespace Swlh.WebApp.Application.Dtos.PostDtos;

public class PostRequestDto
{
    public Guid? Id { get; set; }
    [MaxLength(1023)] public string? Title { get; set; } = string.Empty;
    [MaxLength(1023)] public string? Description { get; set; } = string.Empty;
    public IFormFile? Thumbnail { get; set; } = null;
    [MaxLength(65535)] public string? Body { get; set; } = string.Empty;
    public List<string>? TagNames { get; set; } = [];
    public bool? IsDisabled { get; set; }
}
