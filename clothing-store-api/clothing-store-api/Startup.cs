using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.EntityFrameworkCore;
using clothing_store_api.Data;
using System.Text;

using clothing_store_api.Services;
using clothing_store_api.Types;

using clothing_store_api.Middlewares;

namespace clothing_store_api
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
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidIssuer = Configuration.GetValue<string>("Jwt:Issuer"),
                ValidAudience = Configuration.GetValue<string>("Jwt:Issuer"),
                IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:Key")
                            ))
            };

            services.AddScoped<TokenValidationParameters>(provider => validationParams);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = validationParams;
                });


            services.AddDbContext<SystemContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("SystemConnection")));
            services.AddDbContext<CustomerContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("CustomerConnection")));
            services.AddDbContext<ManagerContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("ManagerConnection")));
            services.AddDbContext<InitContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("InitializeConnection")));


            services.AddScoped<IJwtGenerator, JwtGenerator>();

            services.AddScoped<AuthService>();
            services.AddScoped<ImagesService>();
            services.AddScoped<ProductsService>();
            services.AddScoped<CustomerService>();

            services.AddScoped<Auth>();
            services.AddScoped<Validate>();

            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:3000", "https://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            /*app.UseCors(
                options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            );*/
            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            logger.LogInformation("Application running on: " + Configuration.GetValue<string>("Host"));
        }
    }
}
