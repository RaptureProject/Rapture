// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rapture.Services.VersionCheck.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // Resolves an issue where Kestrel tries to write to streams synchronously
    // this is likely a bug in ASP.NET Core 6
    options.AllowSynchronousIO = true;
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Version = "v1",
        Title = "Rapture.Services.VersionCheck",
        Description = "The version check service for Final Fantasy XIV.",
        TermsOfService = new("https://github.com/AndrewBabbitt97/Rapture/blob/main/LICENSE.txt"),
        License = new()
        {
            Name = "MIT License",
            Url = new("https://github.com/AndrewBabbitt97/Rapture/blob/main/LICENSE.txt")
        }
    });

    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddSingleton<VersionRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rapture.Services.VersionCheck v1"));

app.UseHttpLogging();

app.MapControllers();

app.Run();
