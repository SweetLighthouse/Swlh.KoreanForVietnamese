using Swlh.WebApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(Report))] public class Report : BaseEntity
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public Account Account { get; set; } = null!;
    [MaxLength(1023)] public string Link { get; set; } = string.Empty;
    [MaxLength(65535)] public string Description { get; set; } = string.Empty;
    public ReportStatus State { get; set; }

    [MaxLength(65535)] public string? Answer { get; set; }
}
