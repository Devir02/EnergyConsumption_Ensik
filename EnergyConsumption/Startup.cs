using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EnergyConsumption.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using EnergyConsumption.Repository.Interfaces;
using DataAccess.EFCore.Repositories;
using DataAccess.EFCore.UnitOfWorks;
using LoggerManager;
namespace EnergyConsumption
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
      


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<EnergyConsumptionContext>(options =>
           options.UseSqlServer(
               Configuration.GetConnectionString("connStr"),
               b => b.MigrationsAssembly(typeof(EnergyConsumptionContext).Assembly.FullName)));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IReadingRepository, ReadingRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILoggerManager, Logger>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
