using BlazorApp.Components;
using BlazorApp.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем компоненты Blazor Server для интерактивной формы проверки года.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// В production включаем обработку ошибок и HSTS, чтобы безопаснее обслуживать сайт по HTTPS.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Перенаправляем запросы к отсутствующим страницам на русскоязычную страницу 404.
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

// Подключаем REST API до маршрутов интерфейса, чтобы запрос проверки обрабатывался как JSON.
app.MapLeapYearEndpoints();

// Раздаем статические файлы и запускаем корневое приложение Blazor.
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
