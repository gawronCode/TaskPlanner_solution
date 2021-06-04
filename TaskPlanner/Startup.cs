using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskPlanner.Data;
using TaskPlanner.Models.DbModels;
using TaskPlanner.Repositories.Implementations;
using TaskPlanner.Repositories.Interfaces;
using TaskPlanner.Utilities.Implementations;
using TaskPlanner.Utilities.Interfaces;

namespace TaskPlanner
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

            services.AddDbContext<TaskPlannerDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString")));


            // nuget Microsoft.AspNetCore.Authentication.JwtBearer
            services.AddAuthentication(q =>
            {
                q.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                q.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(q =>
            {
                q.RequireHttpsMetadata = false;
                q.SaveToken = true;
                q.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KeyHolder.GetKey())),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            
            services.AddScoped<IHashManager, HashManager>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IJwtAuthenticationManager, JwtAuthenticationManager>();

            services.AddControllers();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
