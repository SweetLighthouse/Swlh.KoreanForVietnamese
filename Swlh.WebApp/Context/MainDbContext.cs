using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Swlh.Domain.Enums;
using Swlh.WebApp.Domain.Entities;

namespace Swlh.WebApp.Context;

public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
{
    public DbSet<CustomFile> CustomFiles { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet <Keyword> Keywords { get; set; }
    public DbSet<Example> Examples { get; set; }
    public DbSet<CommentOnKeyword> CommentOnKeywords {  get; set; }
    public DbSet<CommentOnKeywordReaction> CommentOnKeywordReactions { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<CommentOnPost> CommentOnPosts { get; set; }
    public DbSet<CommentOnPostReaction> CommentOnPostReactions { get; set; }
    public DbSet<PostReaction> PostReactions { get; set; }
    public DbSet<Report> Reports { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomFile>()
            .HasKey(file => file.Id);

        modelBuilder.Entity<Account>()
            .Property(a => a.Role)
            .HasDefaultValue(Role.User);

        modelBuilder.Entity<Account>()
            .HasKey(acc => acc.Id);

        modelBuilder.Entity<Account>()
            .Property(a => a.IsDisabled)
            .HasDefaultValue(false);


        modelBuilder.Entity<CommentOnKeyword>()
            .HasKey(comment => comment.Id);

        modelBuilder.Entity<CommentOnKeyword>()
            .Property(comment => comment.IsDisabled)
            .HasDefaultValue(false);


        modelBuilder.Entity<CommentOnKeyword>()
            .HasOne(comment => comment.Account)
            .WithMany(acc => acc.CommentOnWords)
            .HasForeignKey(comment => comment.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CommentOnKeyword>()
            .HasOne(comment => comment.Keyword)
            .WithMany(keyword => keyword.Comments)
            .HasForeignKey(comment => comment.Key)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<CommentOnKeywordReaction>()
            .HasKey(record => new { record.AccountId, record.CommentId });

        modelBuilder.Entity<Account>()
            .HasMany(acc => acc.CommentOnWordsReacted)
            .WithMany(comment => comment.ReactedAccounts)
            .UsingEntity<CommentOnKeywordReaction>(

                j => j.HasOne(j => j.Comment)
                      .WithMany(comment => comment.Reactions)
                      .HasForeignKey(j => j.CommentId)
                      .OnDelete(DeleteBehavior.Cascade),

                j => j.HasOne(j => j.Account)
                      .WithMany(acc => acc.CommentOnWordReactions)
                      .HasForeignKey(j => j.AccountId)
                      .OnDelete(DeleteBehavior.Cascade)
            );

        modelBuilder.Entity<Keyword>()
            .HasKey(key => key.Key);

        modelBuilder.Entity<Keyword>()
            .Property(key => key.SearchCount)
            .HasDefaultValue(0);

        modelBuilder.Entity<Example>()
            .HasKey(example => new { example.Korean, example.Vietnamese });


        /// post thing
        modelBuilder.Entity<Post>()
            .HasKey(post => post.Id);

        modelBuilder.Entity<Post>()
            .HasOne(post => post.Account)
            .WithMany(account => account.Posts)
            .HasForeignKey(post => post.AccountId)
            .OnDelete(DeleteBehavior.SetNull); // account is deleteed but all the post is still there.


        modelBuilder.Entity<PostTag>()
            .HasKey(postTag => new { postTag.PostId, postTag.TagId });

        modelBuilder.Entity<Post>()
            .HasMany(post => post.Tags)
            .WithMany(tag => tag.Posts)
            .UsingEntity<PostTag>(
                j => j.HasOne(j => j.Tag)
                      .WithMany(tag => tag.PostTags)
                      .HasForeignKey(j => j.TagId)
                      .OnDelete(DeleteBehavior.Cascade), // on tag delete, remove post tag
                j => j.HasOne(j => j.Post)
                      .WithMany(post => post.PostTags)
                      .HasForeignKey(j => j.PostId)
                      .OnDelete(DeleteBehavior.Cascade) // on post delete, remove post tag
            );

        //modelBuilder.Entity<Tag>()
        //    .HasOne(postTag => postTag.Post)
        //    .WithMany(post => post.Tags)
        //    .HasForeignKey(postTag => postTag.PostId)
        //    .OnDelete(DeleteBehavior.Cascade); // post is deleted => all tags is deleted




        modelBuilder.Entity<CommentOnPost>()
            .HasKey(comment => comment.Id);

        modelBuilder.Entity<CommentOnPost>()
            .HasOne(comment => comment.Account)
            .WithMany(account => account.CommentOnPosts)
            .HasForeignKey(comment => comment.AccountId)
            .OnDelete(DeleteBehavior.SetNull); // account is deleted, but the comment in the post is still there

        modelBuilder.Entity<CommentOnPost>()
            .HasOne(comment => comment.Post)
            .WithMany(post => post.Comments)
            .HasForeignKey(comment => comment.PostId)
            .OnDelete(DeleteBehavior.Cascade); // post deleted => all its comments are gone.


        modelBuilder.Entity<PostReaction>()
            .HasKey(postReaction => new { postReaction.PostId, postReaction.AccountId });

        modelBuilder.Entity<Post>()
            .HasMany(post => post.ReactedAccounts)
            .WithMany(account => account.PostsReacted)
            .UsingEntity<PostReaction>(

                j => j.HasOne(j => j.Account)
                      .WithMany(acc => acc.PostReactions)
                      .HasForeignKey(j => j.AccountId)
                      .OnDelete(DeleteBehavior.Cascade), // on account delete, remove reaction

                j => j.HasOne(j => j.Post)
                      .WithMany(post => post.Reactions)
                      .HasForeignKey(j => j.PostId)
                      .OnDelete(DeleteBehavior.Cascade) // on post delete, remove reaction
            );


        modelBuilder.Entity<CommentOnPostReaction>()
            .HasKey(reaction => new { reaction.CommentId, reaction.AccountId });

        modelBuilder.Entity<CommentOnPost>()
            .HasMany(comment => comment.ReactedAccounts)
            .WithMany(account => account.CommentOnPostsReacted)
            .UsingEntity<CommentOnPostReaction>(

                j => j.HasOne(j => j.Account)
                      .WithMany(acc => acc.CommentOnPostReactions)
                      .HasForeignKey(j => j.AccountId)
                      .OnDelete(DeleteBehavior.Cascade), // delete reaction on account delete

                j => j.HasOne(j => j.Comment)
                      .WithMany(comment => comment.Reactions)
                      .HasForeignKey(j => j.CommentId)
                      .OnDelete(DeleteBehavior.Cascade) // delete reaction on comment delete
            );

        modelBuilder.Entity<Report>()
            .HasKey(report => report.Id);

        modelBuilder.Entity<Report>()
            .Property(report => report.IsSolved)
            .HasDefaultValue(false);

        modelBuilder.Entity<Report>()
            .HasOne(report => report.Account)
            .WithMany(account => account.ReportsCreated)
            .HasForeignKey(report => report.AccountId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimeStamp();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        AddTimeStamp();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void AddTimeStamp()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
