var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "review",
    pattern: "{controller=Home}/{action=Review}/{id?}");

app.MapControllerRoute(
    name: "add",
    pattern: "{controller=Home}/{action=Add}/{id?}");


app.MapControllerRoute(
    name: "verification",
    pattern: "{controller=Home}/{action=Verification}/{id?}");

app.MapControllerRoute(
    name: "documentview",
    pattern: "{controller=Home}/{action=DocumentView}/{id?}");

app.Run();
