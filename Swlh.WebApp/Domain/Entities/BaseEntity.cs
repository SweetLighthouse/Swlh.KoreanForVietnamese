namespace Swlh.WebApp.Domain.Entities;

public class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int AccessedCount { get; set; }
}
