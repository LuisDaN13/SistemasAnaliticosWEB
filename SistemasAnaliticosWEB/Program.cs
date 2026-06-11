using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "default", options =>
    {
        options.PermitLimit = 20;
        options.Window = TimeSpan.FromSeconds(10);
        options.QueueLimit = 0;
    });

    options.RejectionStatusCode = 429;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// MIDDLEWARE DE SEGURIDAD
app.Use(async (context, next) =>
{
    // Bloqueo de bots
    var ua = context.Request.Headers["User-Agent"].ToString().ToLower();
    string[] blocked = { "mirai", "jaws", "lol", "mfscan" };
    if (blocked.Any(b => ua.Contains(b)))
    {
        context.Response.StatusCode = 403;
        return;
    }

    var headers = context.Response.Headers;

    var csp =
        "default-src 'self'; " +
        "script-src 'self' https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://www.youtube.com https://s.ytimg.com; " +
        "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://fonts.googleapis.com; " +
        "style-src-elem 'self' 'unsafe-inline' https://cdn.jsdelivr.net https://cdnjs.cloudflare.com https://fonts.googleapis.com; " +
        "font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com https://cdn.jsdelivr.net; " +
        "img-src 'self' data: blob:; " +
        "frame-src 'self' https://www.youtube-nocookie.com https://maps.google.com https://www.google.com; " +
        "frame-ancestors 'none'; " +
        "base-uri 'self'; " +
        "form-action 'self'; " +
        (app.Environment.IsDevelopment()
            ? "connect-src 'self' wss://localhost:* https://cdn.jsdelivr.net;"  
            : "connect-src 'self';");

    headers["Content-Security-Policy"] = csp;
    headers["X-Content-Type-Options"] = "nosniff";      
    headers["Referrer-Policy"] = "no-referrer";
    headers["X-Frame-Options"] = "DENY";
    headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
    headers["Cross-Origin-Resource-Policy"] = "same-origin"; 

    await next();

    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        context.Request.Path = "/Home/Error";
        await next();
    }
});

// Headers de seguridad HTTP
app.UseXContentTypeOptions();
app.UseXfo(options => options.Deny());
app.UseReferrerPolicy(opts => opts.NoReferrer());

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
