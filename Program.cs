using CarBuilder.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;


List<Paint> paints = new List<Paint>()
{
    new Paint()
    {
        Id = 1,
        Price = 200.25M,
        Color = "Silver"
    },
    new Paint()
    {
        Id = 2,
        Price = 235.6M,
        Color = "Midnight Blue"
    },
    new Paint()
    {
        Id = 3,
        Price = 145.78M,
        Color = "Firebrick Red"
    },
    new Paint()
    {
        Id = 4,
        Price = 564.78M,
        Color = "Spring Green"
    }
};

List<Interior> interiors = new List<Interior>()
{
    new Interior()
    {
        Id = 1,
        Price = 548.65M,
        Material = "Beige Fabric"
    },
    new Interior()
    {
        Id = 2,
        Price = 12.45M,
        Material = "Charcoal Fabric"
    },
    new Interior()
    {
        Id = 3,
        Price = 789.45M,
        Material = "White Leather"
    },
    new Interior()
    {
        Id = 4,
        Price = 3216.56M,
        Material = "Black Leather"
    }
};

List<Technology> technologies = new List<Technology>()
{
    new Technology()
    {
        Id = 1,
        Price = 123.45M,
        Package = "Basic Package (basic sound system)"
    },
    new Technology()
    {
        Id = 2,
        Price = 965.86M,
        Package = "Navigation Package (includes integrated navigation controls)"
    },
    new Technology()
    {
        Id = 3,
        Price = 458.75M,
        Package = "Visibility Package (includes side and reat cameras)"
    },
    new Technology()
    {
        Id = 4,
        Price = 568.23M,
        Package = "Ultra Package (includes navigation and visibility packages)"
    }
};

List<Wheels> wheels = new List<Wheels>(){
    new Wheels()
    {
        Id = 1,
        Price = 123.85M,
        Style = "17-inch Pair Radial"
    },
    new Wheels()
    {
        Id = 2,
        Price = 785.63M,
        Style = "17-inch Pair Radial Black"
    },
    new Wheels()
    {
        Id = 3,
        Price = 895.22M,
        Style = "18-inch Pair Spoke Silver"
    },
    new Wheels()
    {
        Id = 4,
        Price = 786.36M,
        Style = "18-inch Pair Spoke Black"
    },
};

List<Order> orders = new List<Order>()
{

};
var builder = WebApplication.CreateBuilder(args);
// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options =>
                {
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                    options.AllowAnyHeader();
                });
}

app.UseHttpsRedirection();

app.MapGet("/interiors", () =>
{
    return interiors;
});

app.MapGet("/interiors/{id}", (int id) => 
{
    Interior interior = interiors.FirstOrDefault(i => i.Id == id);
    if (interior == null)
    {
        return Results.NotFound();
    }
    interior.Orders = orders.Where(o => o.InteriorId == id).ToList();
    return Results.Ok(interior);
});

app.MapGet("/paints", () =>
{
    return paints;
});

app.MapGet("/paints/{id}", (int id) => 
{
    Paint paint = paints.FirstOrDefault(p => p.Id == id);
    if (paint == null)
    {
        return Results.NotFound();
    }
    paint.Orders = orders.Where(o => o.PaintId == id).ToList();
    return Results.Ok(paint);
});

app.MapGet("/technologies", () => 
{
    return technologies;
});

app.MapGet("/technologies/{id}", (int id) => 
{
    Technology technology = technologies.FirstOrDefault(t => t.Id == id);
    if (technology == null)
    {
        return Results.NotFound();
    }
    technology.Orders = orders.Where(o => o.TechnologyId == id).ToList();
    return Results.Ok(technology);
});

app.MapGet("/wheels", () =>
{
    return wheels;
});

app.MapGet("/wheels/{id}", (int id) => 
{
    Wheels wheel = wheels.FirstOrDefault(w => w.Id == id);
    if (wheel == null)
    {
        return Results.NotFound();
    }
    wheel.Orders = orders.Where(o => o.WheelId == id).ToList();
    return Results.Ok(wheel);
});

app.MapGet("/orders", () => 
{
    foreach (Order order in orders)
    {
        order.Paint = paints.FirstOrDefault(p => p.Id == order.PaintId);
        order.Wheels = wheels.FirstOrDefault(w => w.Id == order.WheelId);
        order.Interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);
        order.Technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    }
    
    var unfulfilledOrders = orders.Where(o => !o.Fulfilled).ToList();

    return unfulfilledOrders;
});

app.MapPost("/orders", (Order order) => 
{
    order.Id = orders.Count > 0 ?orders.Max(o => o.Id) + 1 : 1;
    order.Timestamp = DateTime.Now;
    orders.Add(order);
    return order;
});

app.MapGet("/orders/{id}", (int id) => 
{
    Order order = orders.FirstOrDefault(o => o.Id == id);
    if (order == null)
    {
        return Results.NotFound();
    }
    order.Interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);
    order.Paint = paints.FirstOrDefault(p => p.Id == order.PaintId);
    order.Technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    order.Wheels = wheels.FirstOrDefault(w => w.Id == order.WheelId);
    return Results.Ok(order);
});

app.MapPost("/orders/{id}/fulfill", (int id) =>
{
    Order orderToFulfill = orders.FirstOrDefault(o => o.Id == id);
    orderToFulfill.Fulfilled = true;
});

app.Run();
