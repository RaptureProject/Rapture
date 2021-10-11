// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Version = "v1",
        Title = "Rapture.Services.PatchTracker",
        Description = "The patch tracker service for Final Fantasy XIV.",
        TermsOfService = new("https://github.com/AndrewBabbitt97/Rapture/blob/main/LICENSE.txt"),
        License = new()
        {
            Name = "MIT License",
            Url = new("https://github.com/AndrewBabbitt97/Rapture/blob/main/LICENSE.txt")
        }
    });

    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
        HttpLoggingFields.ResponsePropertiesAndHeaders |
        HttpLoggingFields.RequestQuery;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rapture.Services.PatchTracker v1"));

app.UseHttpLogging();

app.MapControllers();

app.Run();
