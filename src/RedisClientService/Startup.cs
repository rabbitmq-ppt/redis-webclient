using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RedisClientService.Configuration;
using RedisClientService.Infrastructure;
using StackExchange.Redis;

namespace RedisClientService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddMvc();
            services.AddOptions();
            services.Configure<RedisCsConfig>(options => _configuration.GetSection("RedisClient").Bind(options));
            services.AddTransient<IRedisCsConfig>(sp => sp.GetRequiredService<IOptions<RedisCsConfig>>().Value);
            services.AddTransient<IRedisCache, RedisCache>();
            services.AddSwaggerGen();

            services.AddSingleton(
                sp => ConnectionMultiplexer.Connect(_configuration.GetValue<string>("RedisClient:ConnectionString"))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=redis}/{action=index}/{id?}");
            });
        }
    }
}
