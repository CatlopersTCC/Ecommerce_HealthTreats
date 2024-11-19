
using WebEcommerce.Repository;
using WebEcommerce.Repository.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Adicionando o serviço para manipular a Sessão
builder.Services.AddHttpContextAccessor();

//Adicionando as interfaces como serviço
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();


//Adicionando serviços para armazenar sessões no navegador (corrigir problema de TEMPDATA, dados temporáreos)
builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        //Definindo um tempo para a duração
        options.IdleTimeout = TimeSpan.FromSeconds(60);
        options.Cookie.HttpOnly = true;

        //Mostrar para o navegdor que o cookie é essencial
        options.Cookie.IsEssential = true;
    });
    builder.Services.AddMvc().AddSessionStateTempDataProvider();



builder.Services.AddScoped<WebEcommerce.Libraries.Section.Section>();
builder.Services.AddScoped<WebEcommerce.Libraries.Login.LoginCliente>();

builder.Services.AddAuthentication("AdminAuth")
    .AddCookie("AdminAuth", options =>
    {
        options.LoginPath = "/Admin/Login"; // Caminho para a página de login
        options.AccessDeniedPath = "/Admin/AccessDenied"; // Caminho para acesso negado
    });

builder.Services.AddAuthorization();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseCookiePolicy();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
