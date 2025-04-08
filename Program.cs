

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Session Service
builder.Services.AddDistributedMemoryCache();  // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true; // Security reason
    options.Cookie.IsEssential = true; // Required for essential session use
});

// ✅ Add Controllers with Views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ✅ Ensure Middleware Order is Correct
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.UseSession(); // ✅ MUST be added before `app.UseEndpoints()`

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Admin}/{action=CustomerBooking}/{id?}");
});



app.Run();
