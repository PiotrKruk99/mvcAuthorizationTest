using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";
                    options.AccessDeniedPath = "/illegal";
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(20);
                    options.Cookie.MaxAge = options.ExpireTimeSpan;
                    options.SlidingExpiration = true;
                    options.Events = new CookieAuthenticationEvents()
                    {
                        OnCheckSlidingExpiration = async context =>
                        {
                            var expireSpan = context.ShouldRenew ? options.ExpireTimeSpan : context.RemainingTime;
                            await Task.Run(() => context.Response.Cookies.Append("expireSpan", expireSpan.TotalSeconds.ToString()));
                        }
                    };
                });

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
