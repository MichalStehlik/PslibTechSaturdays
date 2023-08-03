using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Constants;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging to file
var sLog = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs\\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(sLog);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<RazorViewToStringRenderer>();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
    options => {
        options.Password.RequireUppercase = builder.Configuration["Password:Uppercase"] == "true";
        options.Password.RequireLowercase = builder.Configuration["Password:Lowercase"] == "true";
        options.SignIn.RequireConfirmedAccount = builder.Configuration["Password:ConfirmedAccount"] == "true";
        options.Password.RequireDigit = builder.Configuration["Password:Digit"] == "true";
        options.Password.RequireNonAlphanumeric = builder.Configuration["Password:NonAlphaNumeric"] == "true";
        options.Password.RequiredLength = Convert.ToInt32(builder.Configuration["Password:Length"]);
    }
    )
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Security.ADMIN_POLICY, policy => { policy.RequireClaim(Security.ADMIN_CLAIM, "1"); });
    options.AddPolicy(Security.LECTOR_POLICY, policy => { policy.RequireAssertion(x => x.User.HasClaim(Security.ADMIN_CLAIM, "1") || x.User.HasClaim(Security.LECTOR_CLAIM, "1")); });
});
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
