using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using diplom_api;
using NLog;
using NLog.Web;
using Microsoft.Data.SqlClient;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("My API Start!");

try
{
	var builder = WebApplication.CreateBuilder(args);

	builder.Logging.ClearProviders();
	builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
	builder.Host.UseNLog();



	builder.Services.AddAuthorization();
	builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(options =>
		{
			options.RequireHttpsMetadata = false;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				// указывает, будет ли валидироваться издатель при валидации токена
				ValidateIssuer = true,
				// строка, представляющая издателя
				ValidIssuer = AuthOptions.ISSUER,
				// будет ли валидироваться потребитель токена
				ValidateAudience = true,
				// установка потребителя токена
				ValidAudience = AuthOptions.AUDIENCE,
				// будет ли валидироваться время существования
				ValidateLifetime = true,
				// установка ключа безопасности
				IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
				// валидация ключа безопасности
				ValidateIssuerSigningKey = true,
				ClockSkew = TimeSpan.FromMinutes(AuthOptions.LIFETIME)
			};

		});
	// Add services to the container.

	builder.Services.AddControllers();
	builder.Services.AddDbContext<diplom_api.Models.kursach_diplomContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen(
		s =>
		{
			s.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Chat API",
				Description = "Chat API Swagger Surface",
				Contact = new OpenApiContact
				{
					Name = "João Victor Ignacio",
					Email = "ignaciojvig@gmail.com",
					Url = new Uri("https://www.linkedin.com/in/ignaciojv/")
				},
				License = new OpenApiLicense
				{
					Name = "MIT",
					Url = new Uri("https://github.com/ignaciojvig/ChatAPI/blob/master/LICENSE")
				}

			});

			s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});

			s.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
				});
		});

	builder.Services.AddLogging(loggingBuilder =>
	{
		loggingBuilder.AddConsole().AddFilter(DbLoggerCategory.Database.Command.Name, Microsoft.Extensions.Logging.LogLevel.Information);
		loggingBuilder.AddDebug();
	});

	var app = builder.Build();

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
}
catch(Exception exception)
{
	logger.Error(exception, "My API Stop!");
	throw;
}
finally
{
	NLog.LogManager.Shutdown();
}