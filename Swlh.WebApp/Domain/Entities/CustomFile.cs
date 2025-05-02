using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(CustomFile))] public class CustomFile : BaseEntity
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[] FileData { get; set; } = [];
    public string ContentType { get; set; } = string.Empty;
}