using Elastic.Apm.AspNetCore;
using Elastic.Apm.NetCoreAll;
using Serilog;


var app = AppBuilder.createBuilder(args);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
