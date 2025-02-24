using Microsoft.EntityFrameworkCore;
using Lojinha.Controllers;
using Microsoft.AspNetCore.Mvc;
using Lojinha.Aplicacao;
using Lojinha.Persistencia;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<LojinhaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUsuarioService, UsuarioService>();
builder.Services.AddTransient<IPedidoService, PedidoService>();
builder.Services.AddTransient<IItemsPedidosService, ItemsPedidosService>();
builder.Services.AddTransient<IProdutosService, ProdutosService>();
builder.Services.AddTransient<IEstoqueService, EstoqueService>();
builder.Services.AddTransient<IFornecedoresService, FornecedoresService>();
builder.Services.AddTransient<IMovimentacaoEstoqueService, MovimentacaoEstoqueService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();