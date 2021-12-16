using DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRApp;
using System;
using Microsoft.EntityFrameworkCore;
using Contracts.DAL;
using DAL.Repo;
using BLL.Contracts;
using Microsoft.OpenApi.Models;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Contracts.Authentification;
using TestChatR.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DTO;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BLL.Authenticators.TokenGenerators;
using BLL.Authenticators;
using System.Threading.Tasks;
using BLL.Authenticators.TokenValidators;
using DTO.Authentications;

namespace TestChatR
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
            Configuration.Bind("Authentication", authenticationConfiguration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                        ValidIssuer = authenticationConfiguration.Issuer,
                        ValidAudience = authenticationConfiguration.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            Console.WriteLine("JWT!!!");
                            //// если запрос направлен хабу
                            //var path = context.HttpContext.Request.Path;
                            //if (!string.IsNullOrEmpty(accessToken) &&
                            //    (path.StartsWithSegments("/chat")))
                            //{
                            //    // получаем токен из строки запроса
                            //    context.Token = accessToken;
                            //}
                            return Task.CompletedTask;
                        }
                        //OnChallenge = c =>
                        //{
                        //    c.HandleResponse();
                        //    return Task.CompletedTask;
                        //}
                    };


                });

            services.AddSignalR(hubOptions =>
            {
                hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
            });
            services.AddCors();
            services.AddSingleton(authenticationConfiguration);
            services.AddDbContext<ChatContext>(options =>
                options.UseSqlServer(connectionString));
           // services.AddScoped<Authenticator>();
            services.AddScoped<AccessTokenGenerator>();
            services.AddSingleton<RefreshTokenGenerator>();
            services.AddSingleton<TokenGenerator>();
            services.AddScoped<IRefreshTokenBase<RefreshTokenDto>, RefreshTokenRepository>();
            services.AddSingleton<RefreshTokenValidator>();
            services.AddIdentity<User, IdentityRole<int>>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 0;
            }).AddEntityFrameworkStores<ChatContext>();

            services.AddAutoMapper(new[] { typeof(MapperVM), typeof(MapperDAL) });
            services.AddScoped<DbContext, ChatContext>();
            //services.AddScoped<IdentityDbContext<User, IdentityRole<int>, int>, ChatContext>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAuthentificationService, AuthentificationService>();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebIpi v1"));
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(e => e.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSwagger();
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
