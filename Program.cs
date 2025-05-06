using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Pizz�ria.Models;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<PizzaEhodDB>(connectionString);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Pizz�ria",
        Description = "Faire les pizzas que vous aimez",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pizz�ria API V1");
});

app.MapGet("/", () => "Ecole Sup�rieure Polytechnique DIC3 2025");
app.MapGet("/pizzas", async (PizzaEhodDB db) => await db.Pizzas.ToListAsync());
app.MapPost("/pizza", async(PizzaEhodDB db, PizzaEhod pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.IdEhod}", pizza);
});
app.MapGet("/pizza/{id}", async (PizzaEhodDB db, int id) => await db.Pizzas.FindAsync(id));
app.MapPut("/pizza/{id}", async (PizzaEhodDB db, PizzaEhod updatepizza, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();
    pizza.NomEhod = updatepizza.NomEhod;
    pizza.DescriptionEhod = updatepizza.DescriptionEhod;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/pizza/{id}", async (PizzaEhodDB db, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null)
    {
        return Results.NotFound();
    }
    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});
app.Run();
