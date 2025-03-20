using Amazon.DynamoDBv2;
using SmartInventory.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure AWS services
builder.Services.AddAWSService<IAmazonDynamoDB>();

// Register services
builder.Services.AddScoped<IInventoryService, InventoryService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJsApp",
        builder =>
        {
            builder.WithOrigins(builder.Configuration["AllowedOrigins"]!)
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowNextJsApp");
app.UseAuthorization();
app.MapControllers();

app.Run(); 