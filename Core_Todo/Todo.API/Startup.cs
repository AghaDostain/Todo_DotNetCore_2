using System.Linq;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Todo.Data;
using Todo.Repositories;
using Todo.Services;
using Todo.WebAPI;
using Todo.WebAPI.Attributes;
using Todo.Models.Validations;
using System.Net;
using Microsoft.AspNetCore.Routing;
using Todo.Common.Exceptions;
using System.Reflection;
using NetCore.AutoRegisterDi;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Todo.Entities;

namespace Core_Todo
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
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Lockers App",
                    Version = "v1"
                });
                c.CustomSchemaIds(x => x.FullName);
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer",new string[]{ } }
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Token for authorization",
                    Name = "Authorization",
                    In = "hearder",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(security);
            });
            var ServiceAssemblyToScan = Assembly.GetAssembly(typeof(UserTaskManager));
            services.RegisterAssemblyPublicNonGenericClasses(ServiceAssemblyToScan)
              .Where(c => c.Name.EndsWith("Manager"))
              .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            var repositoryAssemblyToScan = Assembly.GetAssembly(typeof(UserTaskRepository));
            services.RegisterAssemblyPublicNonGenericClasses(repositoryAssemblyToScan)
              .Where(c => c.Name.EndsWith("Repository"))
              .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            services.AddScoped<DbContext, DataContext>();

            services.AddMvc(opt=> opt.Filters.Add(typeof(ValidateModelAttribute)))
                .AddControllersAsServices()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserTaskModelValidator>())
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddAutoMapper();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Config JWT
            //Authenticate JWT
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = false;
                opt.Authority = "https://login.microsoftonline.com/a1d50521-9687-4e4d-a76d-ddd53ab0c668";
                opt.Audience = "http://abc.com";
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "http://abc.com",
                    ValidIssuer = "http://abc.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("MySuperSecureKey"))
                };
            });

            services.Configure<Todo.Exceptions.ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var error = actionContext.ModelState.Keys.SelectMany(key => actionContext.ModelState[key].Errors.Select(ex => new ValidationError(key, ex.ErrorMessage))).FirstOrDefault();
                    return new ContentActionResult<ValidationError>(HttpStatusCode.BadRequest, error, "BAD REQUEST", null);
                };
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var routeBuilder = new RouteBuilder(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo V1.0");
            });
            // Serialize all exceptions to JSON
            var jsonExceptionMiddleware = new ExceptionMiddleware( app.ApplicationServices.GetRequiredService<IHostingEnvironment>());
            app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandler = jsonExceptionMiddleware.Invoke });
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
