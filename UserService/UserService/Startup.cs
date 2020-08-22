using System;
using System.Reflection;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

namespace UserService
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
            var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "my-sql-server";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
            var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "UserApi";
            var user = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "sa";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "P@ssword";
            var connectionString = $"Server={server},{port};Database={database};User={user};Password={password};";
            Console.WriteLine("Start Up, Connection string is : " + connectionString);
            services.AddDbContext<Models.UserContext>(options =>
                    //options.UseSqlServer("Server=my-sql-server,1433;Database=UserApi;User=sa;Password=P@ssword;",
                    options.UseSqlServer(connectionString,
                    sqlServerOptionsAction: sqlOptions => sqlOptions.EnableRetryOnFailure())
                );
            //services.AddDiscoveryClient(Configuration);
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                string serviceName = Assembly.GetEntryAssembly().GetName().Name;

                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                ISampler sampler = new ConstSampler(sample: true);

                var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    .WithSender(new UdpSender("jagerservice", 6831, 0))
                    .Build();

                ITracer tracer = new Tracer.Builder(serviceName)
                    .WithLoggerFactory(loggerFactory)
                    .WithSampler(sampler)
                    .WithReporter(reporter)
                    .Build();

                GlobalTracer.Register(tracer);

                return tracer;
            });
            services.AddOpenTracing();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            //app.UseDiscoveryClient();
            Models.PrepDb.PrepPopulation(app);
        }
    }
}
