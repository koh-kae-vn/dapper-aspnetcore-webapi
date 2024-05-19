using DapperASPNetCore.Context;
using DapperASPNetCore.Contracts;
using DapperASPNetCore.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DapperASPNetCore
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<DapperContext>();
			services.AddSingleton<CREDITContext>();

			services.AddScoped<ICompanyRepository, CompanyRepository>();
			services.AddScoped<ICreditRepository, CREDITRepository>();

			services.AddControllers();
			services.AddCors(options => {
				options.AddPolicy("mypolicy", builder => builder
				 .WithOrigins("http://localhost:23949/")
				 .SetIsOriginAllowed((host) => true)
				 .AllowAnyMethod()
				 .AllowAnyHeader());
			});
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dynamic services", Version = "v1" });
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dynamic services V1");
			});

			var option = new RewriteOptions();
			option.AddRedirect("^$", "swagger");
			app.UseRewriter(option);

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("mypolicy");

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
