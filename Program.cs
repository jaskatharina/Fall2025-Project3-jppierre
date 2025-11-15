using Fall2025_Project3_jppierre.Data;
using Fall2025_Project3_jppierre.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configConnectionString = builder.Configuration["Db:ConnectionString"] ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var conStrBuilder = new SqlConnectionStringBuilder(configConnectionString);
conStrBuilder.Password = builder.Configuration["Db:Password"] ?? throw new InvalidOperationException("Db:Password does not exist in the current Configuration");
conStrBuilder.UserID = builder.Configuration["Db:Username"] ?? throw new InvalidOperationException("Db:Username does not exist in the current Configuration");
var connectionString = conStrBuilder.ConnectionString;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register AI service
builder.Services.AddScoped<IAiService, OpenAiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
