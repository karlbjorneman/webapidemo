using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webapidemo.Services;
using webapidemo.Model;
using MongoDB.Driver;
using webapidemo.Repositories;

namespace webapidemo
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
            var mongoDbConnectionstring = Configuration.GetValue<string>("mongodb:connectionstring");

            MongoClient mongoClient = new MongoClient(mongoDbConnectionstring);
            var database = mongoClient.GetDatabase("mongodbdemo");
            services.AddSingleton<IMongoDatabase>(database);

            services.AddScoped<IColumnRepository, ColumnRepository>();

            services.AddCors();
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;

                options.ClientErrorMapping[404].Link =
                    "https://httpstatuses.com/404";
            });
            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    var tokenValidatorParams = new TokenValidationParameters();
                    tokenValidatorParams.ValidateIssuerSigningKey = true;
                    tokenValidatorParams.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:secret"]));
                    tokenValidatorParams.ValidateIssuer = false;
                    tokenValidatorParams.ValidateAudience = false;
                    cfg.TokenValidationParameters = tokenValidatorParams;
                });

            services.AddScoped<IAuthService, AuthService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder =>
                builder
                //.WithOrigins("http://localhost:3000", "https://gustaftech-notesapp.azurewebsites.net")
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
