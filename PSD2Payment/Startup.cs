using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PSD2Payment.Database;
using PSD2Payment.Database.Configuration;
using PSD2Payment.Repository;

namespace PSD2Payment
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<PISDBContext>(options =>
               ConnectionFactory.DatabaseConfiguration("psd2", options, Configuration)
            );
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IConnectionRepository, ConnectionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            MigrateDb(app);
        }

        public void MigrateDb(IApplicationBuilder app)
        {
            PISDBContext context = null;
            try
            {
                var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();
                context = serviceScope.ServiceProvider.GetService<PISDBContext>();
                context.Database.Migrate();

            }
            catch (InvalidOperationException e)
            {
                if (e.Source.Equals("Microsoft.EntityFrameworkCore.Relational"))
                {
                    context.Database.EnsureCreated();
                }
            }

        }

    }
}
