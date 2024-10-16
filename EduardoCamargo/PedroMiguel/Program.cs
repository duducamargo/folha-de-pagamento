using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using PedroMiguel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

// //POST : CADASTRAR FUNCIONARIO
app.MapPost("/api/funcionario/cadastrar", ([FromBody] Funcionario funcionario, [FromServices] AppDataContext ctx) =>
{

    if (funcionario == null)
    {
        return Results.NotFound("Funcionário inválido...");
    }

    if (ctx.Funcionarios.Any((ctxFuncionario) => funcionario.Cpf == ctxFuncionario.Cpf))
    {
        return Results.NotFound("Funcionário já existe...");
    }

    // aux[0] = funcionario.Cpf[0];

    ctx.Funcionarios.Add(funcionario);
    ctx.SaveChanges();

    var funcionarios = ctx.Funcionarios.ToList();

    return Results.Ok(funcionarios);
});

//GET : LISTAR FUNCIONARIOS
app.MapGet("/api/funcionario/listar", ([FromServices] AppDataContext ctx) =>
{
    var funcionarios = ctx.Funcionarios.ToList();

    if (funcionarios == null)
    {
        return Results.NotFound("Nenhum funcionario cadastrado...");
    }

    return Results.Ok(funcionarios);
});

//POST : CADASTRAR FOLHA DE PAGAMENTO
app.MapPost("/api/folha/cadastrar", ([FromBody] Folha folha, [FromServices] AppDataContext ctx) =>
{

    if (folha == null)
    {
        return Results.NotFound("Folha de Pagamento inválida...");
    }

    Funcionario? funcionario = ctx.Funcionarios.FirstOrDefault((funcionario) => funcionario.Id == folha.FuncionarioId);

    if (funcionario == null)
    {
        return Results.NotFound("Usuario não existe");
    }

    if (folha.Mes < 1 || folha.Mes > 12)
    {
        return Results.NotFound("Mês inválido...");
    }

    if (ctx.Folhas.Any((x) => x.Mes == folha.Mes && x.FuncionarioId == folha.FuncionarioId))
    {
        return Results.NotFound("Pagamento do Mês já realizado!");
    }

    folha.Funcionario = ctx.Funcionarios.FirstOrDefault((funcionario) => funcionario.Id == folha.FuncionarioId);

    folha.SalarioBruto = folha.Valor * folha.Quantidade;

    if (folha.SalarioBruto <= 1903.98)
    {
        folha.ImpostoIrrf = 0;
    }

    else if (folha.SalarioBruto > 1903.98 && folha.SalarioBruto <= 2826.65)
    {
        folha.ImpostoIrrf = (folha.SalarioBruto * 0.075);
    }

    else if (folha.SalarioBruto > 2826.65 && folha.SalarioBruto <= 3751.05)
    {
        folha.ImpostoIrrf = (folha.SalarioBruto * 0.15);
    }

    else if (folha.SalarioBruto > 3751.05 && folha.SalarioBruto <= 4664.68)
    {
        folha.ImpostoIrrf = (folha.SalarioBruto * 0.225);
    }

    else
    {
        folha.ImpostoIrrf = (folha.SalarioBruto * 0.275);
    }

    if (folha.SalarioBruto <= 1693.72)
    {
        folha.ImpostoInss = (folha.SalarioBruto * 0.08);
    }

    else if (folha.SalarioBruto > 1693.72 && folha.SalarioBruto <= 2822.90)
    {
        folha.ImpostoInss = (folha.SalarioBruto * 0.09);
    }

    else if (folha.SalarioBruto > 2822.90 && folha.SalarioBruto <= 5645.80)
    {
        folha.ImpostoInss = (folha.SalarioBruto * 0.11);
    }

    else
    {
        folha.ImpostoInss = 621.03;
    }

    folha.ImpostoFgts = (folha.SalarioBruto * 0.08);

    folha.SalarioLiquido = folha.SalarioBruto - folha.ImpostoInss - folha.ImpostoIrrf;

    ctx.Folhas.Add(folha);
    ctx.SaveChanges();

    var folhas = ctx.Folhas.ToList();

    return Results.Ok(folhas);
});

//GET : LISTAR FOLHAS DE PAGAMENTO
app.MapGet("/api/folha/listar", ([FromServices] AppDataContext ctx) =>
{
    var folhas = ctx.Folhas.ToList();

    folhas.ForEach((folha) => folha.Funcionario = ctx.Funcionarios.FirstOrDefault((funcionario) => funcionario.Id == folha.FuncionarioId));

    if (folhas == null)
    {
        return Results.NotFound("Nenhum funcionario cadastrado...");
    }

    return Results.Ok(folhas);
});

app.MapGet("/api/folha/buscar/{cpf}/{mes}/{ano}", ([FromRoute] string cpf, int mes, int ano, [FromServices] AppDataContext ctx) =>
{
    var funcionario = ctx.Funcionarios.FirstOrDefault(funcionario => funcionario.Cpf == cpf);

    if (funcionario == null)
    {
        return Results.NotFound("Nenhum funcionário encontrado com esse CPF.");
    }

    var folha = ctx.Folhas.FirstOrDefault(f => f.Funcionario.Cpf == cpf && f.Mes == mes && f.Ano == ano);

    if (folha == null)
    {
        return Results.NotFound("Nenhuma folha de pagamento encontrada para o funcionário no período especificado.");
    }
    folha.Funcionario = funcionario;

    return Results.Ok(folha);
});


app.Run();
