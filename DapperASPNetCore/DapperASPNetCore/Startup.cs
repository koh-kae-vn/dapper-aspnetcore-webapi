using DapperASPNetCore.Context;
using DapperASPNetCore.Contracts;
using DapperASPNetCore.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
			services.AddScoped<ICompanyRepository, CompanyRepository>();
			services.AddControllers();
			services.AddCors(options => {
				options.AddPolicy("mypolicy", builder => builder
				 .WithOrigins("http://localhost:23949/")
				 .SetIsOriginAllowed((host) => true)
				 .AllowAnyMethod()
				 .AllowAnyHeader());
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

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
