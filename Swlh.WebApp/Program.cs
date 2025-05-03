using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Swlh.Domain.Enums;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // 500MB
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MainDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDistributedMemoryCache(); // Required for session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;  // Ensures that the session cookie is not accessible via JavaScript
    options.Cookie.IsEssential = true; // Makes the cookie essential for the app's functionality
});

builder.Services.AddHttpClient("NaverDict", client =>
{
    client.BaseAddress = new Uri("https://dict.naver.com/api3/viko/search");
    client.DefaultRequestHeaders.Add("Referer", "https://dict.naver.com/vikodict/");
});

builder.Services.AddHttpClient("KrDict", client =>
{
    client.BaseAddress = new Uri("https://krdict.korean.go.kr/vie/dicMarinerSearch/search");
});

var app = builder.Build();


// create default Admin account
using (var scope = app.Services.CreateScope())
{
    var username = builder.Configuration.GetValue<string>("DefaultAdminAccount.Username") ?? "admin";
    var password = builder.Configuration.GetValue<string>("DefaultAdminAccount.Password") ?? "admin";

    var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
    await context.Database.MigrateAsync();

    var admin = await context.Accounts.SingleOrDefaultAsync(acc => acc.Username == "admin");
    if (admin == null)
    {
        await context.Accounts.AddAsync(new Account
        {
            Username = "admin",
            Password = BCrypt.Net.BCrypt.HashPassword("admin"),
            Email = "admin@gmail.com",
            Role = Role.Admin
        });
    }
    else
    {
        bool needsUpdate = false;

        if (admin.Role != Role.Admin)
        {
            admin.Role = Role.Admin;
            needsUpdate = true;
        }

        if (!BCrypt.Net.BCrypt.Verify("admin", admin.Password))
        {
            admin.Password = BCrypt.Net.BCrypt.HashPassword("admin");
            needsUpdate = true;
        }

        if (needsUpdate)
            context.Accounts.Update(admin);
    }

    await context.SaveChangesAsync();
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();


// check for locked accounts
app.Use(async (context, next) =>
{
    var session = context.Session;
    var userIdString = session.GetString("Id");
    var database = context.RequestServices.GetRequiredService<MainDbContext>();

    if (Guid.TryParse(userIdString, out var userId) &&
        database.Accounts.Any(acc => acc.Id == userId && acc.IsDisabled))
    {
        session.Clear();
        var factory = context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
        var tempData = factory.GetTempData(context);
        tempData["Msg"] = "Tài khoản đã bị khóa.";
        context.Response.Redirect("/");
        return;
    }
    await next();
});


app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
