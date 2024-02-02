using Demo_I18n;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Microsoft.extensions.Localization

builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(o =>
{
    List<CultureInfo> cultureInfos = new List<CultureInfo>() {
        new CultureInfo("en-US"),
        new CultureInfo("fr-BE")
    };
    o.SetDefaultCulture("fr-BE");
    o.SupportedCultures = cultureInfos;
    o.SupportedUICultures = cultureInfos;
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//await builder.Build().RunAsync();

WebAssemblyHost host = builder.Build();

CultureInfo culture;
IJSRuntime js = host.Services.GetRequiredService<IJSRuntime>();
string currentCultre = await js.InvokeAsync<string>("localStorage.getItem", "blazorCulture");
if(currentCultre != null)
     culture = new CultureInfo(currentCultre);

else
{
    culture = new CultureInfo("fr-BE");
    await js.InvokeVoidAsync("localStorage.setItem", "blazorCulture", "fr-BE");
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();