using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProEventos.Application.Contratos;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;
using System;
using ProEventos.Application.Services;
using ProEventos.Persistence.Repository;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using ProEventos.Persistence;
using System.Text.Json.Serialization;
using ProEventos.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Collections.Generic;

namespace ProEventos.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método é chamado pelo ASP.NET Core durante a inicialização da aplicação.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configura o Entity Framework Core para usar o SQLite com a string de conexão "Default".
            services.AddDbContext<ProEventosContext>(
                context => context.UseSqlite(Configuration.GetConnectionString("Default"))
            );

            // Configura o Identity Framework para autenticação e autorização.
            services.AddIdentityCore<User>(options =>
            {
                // Define requisitos de senha.
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddRoleValidator<RoleValidator<Role>>()
            .AddEntityFrameworkStores<ProEventosContext>()
            .AddDefaultTokenProviders();

            // Configura a autenticação com JSON Web Tokens (JWT).
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Configura os controladores (controllers) da aplicação.
            services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // Configura o AutoMapper, um mapeador de objetos.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Registra os serviços da aplicação no contêiner de injeção de dependência.
            services.AddScoped<IEventoService, EventoService>();
            services.AddScoped<ILoteService, LoteService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEventoPersistence, EventoPersistence>();
            services.AddScoped<IGeralPersitence, GeralPersitence>();
            services.AddScoped<ILotePersistence, LotePersistence>();
            services.AddScoped<IUserPersistence, UserPersistence>();

            // Configuração do CORS para permitir requisições de qualquer origem.
            services.AddCors();

            // Configuração do Swagger para documentação da API.
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ProEventos.API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{

                    Description = @"JWT Authorization header usando Bearer.
                                    Entre com 'Bearer ' [espaço] então coloque seu token.
                                    Exemplo 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<String>()
                    }
                });
            });
        }

        // Este método é chamado pelo ASP.NET Core para configurar o pipeline de requisições HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Verifica se a aplicação está em modo de desenvolvimento.
            if (env.IsDevelopment())
            {
                // Configura o tratamento de exceções em ambiente de desenvolvimento.
                app.UseDeveloperExceptionPage();
                // Configura o Swagger para documentação da API.
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProEventos.API v1"));
            }

            // Configura redirecionamento HTTPS.
            app.UseHttpsRedirection();

            // Configuração de roteamento.
            app.UseRouting();

            // Configuração de Autenticação.
            app.UseAuthentication();

            // Configuração de autorização.
            app.UseAuthorization();

            
            // Configuração do CORS para permitir solicitações de qualquer origem.
            app.UseCors(cors => cors.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
            );

            // Configuração de arquivos estáticos.
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                RequestPath = new PathString("/Resources")
            });

            // Configuração de endpoints da aplicação.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}