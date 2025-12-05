using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "default", options =>
    {
        options.PermitLimit = 20;
        options.Window = TimeSpan.FromSeconds(10);
        options.QueueLimit = 0;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Seguridad adicional
app.UseXContentTypeOptions();
app.UseXfo(options => options.Deny());
app.UseReferrerPolicy(opts => opts.NoReferrer());
// app.UseCsp(...); // si quieres agrego CSP

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
