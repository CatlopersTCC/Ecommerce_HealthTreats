
using WebEcommerce.Repository;
using WebEcommerce.Repository.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Adicionando o servi�o para manipular a Sess�o
builder.Services.AddHttpContextAccessor();

//Adicionando as interfaces como servi�o
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();


//Adicionando servi�os para armazenar sess�es no navegador (corrigir problema de TEMPDATA, dados tempor�reos)
builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        //Definindo um tempo para a dura��o
        options.IdleTimeout = TimeSpan.FromSeconds(60);
        options.Cookie.HttpOnly = true;

        //Mostrar para o navegdor que o cookie � essencial
        options.Cookie.IsEssential = true;
    });
    builder.Services.AddMvc().AddSessionStateTempDataProvider();



builder.Services.AddScoped<WebEcommerce.Libraries.Section.Section>();
builder.Services.AddScoped<WebEcommerce.Libraries.Login.LoginCliente>();

builder.Services.AddAuthentication("AdminAuth")
    .AddCookie("AdminAuth", options =>
    {
        options.LoginPath = "/Admin/Login"; // Caminho para a p�gina de login
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
