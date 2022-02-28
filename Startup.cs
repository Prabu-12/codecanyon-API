using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuizplusApi.Models;
using QuizplusApi.Services;
using QuizplusApi.ViewModels.Email;

namespace QuizplusApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string AllowSpecificOrigins = "_allowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Sql Server Connection String
            /* services.AddDbContextPool<AppDbContext>(opt => opt.UseSqlServer
            (Configuration.GetConnectionString("ApiConnStringMssql"))); */

            //Mysql Connection String
            services.AddDbContextPool<AppDbContext>(opt=>opt.UseMySql
            (Configuration.GetConnectionString("ApiConnStringMysql"),
            ServerVersion.AutoDetect(Configuration.GetConnectionString("ApiConnStringMysql"))));

            //Sqlite Connection String
            /* services.AddDbContextPool<AppDbContext>(opt=>opt.UseSqlite
            (Configuration.GetConnectionString("ApiConnStringSqlite"))); */

            //PostgreSql Connection String
            /* services.AddDbContextPool<AppDbContext>(opt=>opt.UseNpgsql
            (Configuration.GetConnectionString("ApiConnStringPostgreSql"))); */

            //Oracle Connection String
            /* services.AddDbContextPool<AppDbContext>(opt=>opt.UseOracle
            (Configuration.GetConnectionString("ApiConnStringOracle"))); */

            services.AddScoped(typeof(ISqlRepository<>), typeof(SqlRepository<>));

            services.AddCors(options=>
            {
                options.AddPolicy(name:AllowSpecificOrigins,builder=>
                    {
                        builder.WithOrigins("http://localhost:8080")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddControllers();
            
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService,Services.MailService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options=>{
                options.RequireHttpsMetadata=false;
                options.SaveToken=true;
                options.TokenValidationParameters=new TokenValidationParameters
                    {
                        ValidateIssuer=true,
                        ValidateAudience=true,
                        ValidateLifetime=true,
                        ValidateIssuerSigningKey=true,
                        ValidIssuer=Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
            });
            IdentityModelEventSource.ShowPII = true;

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Quizplus API v1",
                    Version = "v1",
                    Description = "API to communicate with Quizplus Client"
                });               
                options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme()
                {
                        Name = "Authorization",  
                        Type = SecuritySchemeType.ApiKey,  
                        Scheme = "Bearer",  
                        BearerFormat = "JWT",  
                        In = ParameterLocation.Header,  
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement  
                {  
                    {  
                          new OpenApiSecurityScheme  
                            {  
                                Reference = new OpenApiReference  
                                {  
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"  
                                }  
                            },  
                            new string[] {}  
  
                    }  
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(AllowSpecificOrigins);

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "QuizplusApi v1");
                options.RoutePrefix=string.Empty;
            });
        }
    }
}
