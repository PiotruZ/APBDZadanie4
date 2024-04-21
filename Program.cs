using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Zadanie4;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Initialize sample data
var animals = new List<Animal>
{
    new Animal { Id = 1, Name = "Rex", Category = "Dog", Weight = 30.5, FurColor = "Black" },
    new Animal { Id = 2, Name = "Milo", Category = "Cat", Weight = 6.2, FurColor = "White" },
    new Animal { Id = 3, Name = "Charlie", Category = "Parrot", Weight = 0.85, FurColor = "Green" }
};

var visits = new List<Visit>
{
    new Visit { Id = 1, AnimalId = 1, DateOfVisit = DateTime.Now.AddDays(-10), Description = "Routine check-up", Price = 45.00 },
    new Visit { Id = 2, AnimalId = 2, DateOfVisit = DateTime.Now.AddDays(-3), Description = "Vaccination", Price = 30.00 },
    new Visit { Id = 3, AnimalId = 1, DateOfVisit = DateTime.Now, Description = "Emergency treatment for injury", Price = 100.00 }
};

// API endpoints
app.MapGet("/api/animals", () => Results.Ok(animals))
    .WithName("GetAllAnimals")
    .WithOpenApi()
    .Produces<List<Animal>>(StatusCodes.Status200OK);

app.MapGet("/api/animals/{id}", (int id) =>
{
    var animal = animals.FirstOrDefault(a => a.Id == id);
    if (animal == null) return Results.NotFound("Animal not found");
    return Results.Ok(animal);
})
.WithName("GetAnimalById")
.WithOpenApi()
.Produces<Animal>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.MapPost("/api/animals", (Animal animal) =>
{
    animals.Add(animal);
    return Results.Created($"/api/animals/{animal.Id}", animal);
})
.WithName("CreateAnimal")
.WithOpenApi()
.Produces<Animal>(StatusCodes.Status201Created);

app.MapPut("/api/animals/{id}", (int id, Animal updatedAnimal) =>
{
    var animal = animals.FirstOrDefault(a => a.Id == id);
    if (animal == null) return Results.NotFound("Animal not found");
    animal.Name = updatedAnimal.Name;
    animal.Category = updatedAnimal.Category;
    animal.Weight = updatedAnimal.Weight;
    animal.FurColor = updatedAnimal.FurColor;
    return Results.NoContent();
})
.WithName("UpdateAnimal")
.WithOpenApi()
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.MapDelete("/api/animals/{id}", (int id) =>
{
    var animal = animals.FirstOrDefault(a => a.Id == id);
    if (animal == null) return Results.NotFound("Animal not found");
    animals.Remove(animal);
    return Results.NoContent();
})
.WithName("DeleteAnimal")
.WithOpenApi()
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.MapGet("/api/animals/{animalId}/visits", (int animalId) =>
    Results.Ok(visits.Where(v => v.AnimalId == animalId)))
.WithName("GetVisitsForAnimal")
.WithOpenApi()
.Produces<List<Visit>>(StatusCodes.Status200OK);

app.MapPost("/api/animals/{animalId}/visits", (int animalId, Visit visit) =>
{
    visit.AnimalId = animalId;
    visits.Add(visit);
    return Results.Created($"/api/animals/{animalId}/visits/{visit.Id}", visit);
})
.WithName("CreateVisit")
.WithOpenApi()
.Produces<Visit>(StatusCodes.Status201Created);

app.Run();