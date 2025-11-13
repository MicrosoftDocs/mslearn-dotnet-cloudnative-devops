using Store.Components;
using Store.Services;

var builder = WebApplication.CreateBuilder(args);

var productEndpoint = builder.Configuration["ProductEndpoint"]
    ?? throw new InvalidOperationException("ProductEndpoint is not set");

builder.Services.AddSingleton<ProductService>();
builder.Services.AddHttpClient<ProductService>(c => c.BaseAddress = new Uri(productEndpoint));

builder.Services.AddServiceDiscovery();
builder.Services.AddHttpForwarderWithServiceDiscovery();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapForwarder("/api/product/{name}.{ext:regex(^png|jpg$)}", productEndpoint, "/images/{name}.{ext}");

// Add supported cultures for request localization
var supportedCultures = new[] { "en-US" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.Run();
