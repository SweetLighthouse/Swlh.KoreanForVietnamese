using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(Example))] public class Example
{
    [MaxLength(1023)] public string Korean { get; set; } = string.Empty;
    [MaxLength(1023)] public string Vietnamese { get; set; } = string.Empty;

}
