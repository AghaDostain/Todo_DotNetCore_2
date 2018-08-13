using System;
using System.Linq;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Todo.Data;
using Todo.Exceptions;
using Todo.Mappers.Profiles;
using Todo.Repositories;
using Todo.Services;
using Todo.WebAPI;
using Todo.WebAPI.Attributes;
using Todo.Models.Validations;
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
            services.AddTransient<IUserTaskManager, UserTaskManager>();
            services.AddTransient<IUserTaskRepository, UserTaskRepository>();
            services.AddTransient<DbContext, DataContext>();
            //services.AddTransient<IMapper, Mapper>();
            //Mapper.Initialize(m =>
            //{
            //    var profiles = typeof(UserTaskProfile).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x));
            //    foreach (var profile in profiles)
            //    {
            //        m.AddProfile(Activator.CreateInstance(profile) as Profile);
            //    }
            //});
            //Automapper profile
            //Mapper.Initialize(cfg => cfg.AddProfile<UserTaskProfile>());

            //services.Add(ServiceDescriptor.Transient(typeof(UserTaskModelValidator), typeof(UserTaskModelValidator)));
            services.AddMvc(opt=> opt.Filters.Add(typeof(ValidateModelAttribute)))
                .AddControllersAsServices()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserTaskModelValidator>());
            services.AddAutoMapper();
            //services.AddAutoMapper(null, AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IConfiguration>(Configuration);

            var connection = @"Server=.;Database=ToDo;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(connection));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            });
            services.Configure<Todo.Exceptions.ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new
                        {
                            Name = e.Key,
                            Message = e.Value.Errors.First().ErrorMessage
                        }).ToArray();
                    return new BadRequestObjectResult(errors);
                };
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo V1.0");
            });
            // Serialize all exceptions to JSON
            var jsonExceptionMiddleware = new ExceptionMiddleware( app.ApplicationServices.GetRequiredService<IHostingEnvironment>());
            app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandler = jsonExceptionMiddleware.Invoke });

            app.UseMvc();
        }

    }
}
