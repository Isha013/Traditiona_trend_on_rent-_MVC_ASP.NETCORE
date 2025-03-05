var builder = WebApplication.CreateBuilder(args);

// ? Add Session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ? Add Controllers with Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ? Enable Session Middleware
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseWebSockets();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
