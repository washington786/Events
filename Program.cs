using Events.Extensions;

var builder = WebApplication.CreateBuilder(args);

// inject custom service helper
new ApplicationHelperServices(builder.Services, builder.Configuration).AddApplicationServices();

// swagger ui support services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("dev");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("prod");
}

app.UseSession();

app.UseHttpsRedirection();

app.Run();
