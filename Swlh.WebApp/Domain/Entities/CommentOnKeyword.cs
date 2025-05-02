using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swlh.WebApp.Domain.Entities;

[Table(nameof(CommentOnKeyword))] public class CommentOnKeyword : BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid? AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public string Key { get; set; } = null!;
    public Keyword Keyword { get; set; } = null!;

    [MaxLength(1023)] public string Content { get; set; } = string.Empty;
    public bool IsDisabled { get; set; }

    public List<Account> ReactedAccounts { get; set; } = [];
    public List<CommentOnKeywordReaction> Reactions { get; set; } = [];
}
