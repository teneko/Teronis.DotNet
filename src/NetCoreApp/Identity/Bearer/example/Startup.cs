using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Teronis.Identity.Bearer.Authentication;
using Teronis.Identity.Entities;
using Teronis.AspNetCore.Authorization.Extensions;
using Teronis.Mvc.ApplicationModels;
using Teronis.Mvc.Case;
using Teronis.Mvc.JsonProblemDetails;
using Microsoft.AspNetCore.Mvc;
using NSwag.Generation.Processors.Security;
using NSwag;
using Teronis.Identity.Bearer.NSwag;
using Teronis.Identity.AccountManaging.Controllers;
using Teronis.Identity.Bearer.SignInManaging.Controllers;
using Teronis.Identity.Bearer.Stores;

namespace Teronis.Identity.Bearer
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
            services.AddApiVersioning(options => {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services
                .AddControllers()
                .AddAccountControllers()
                .AddBearerSignInControllers();

            services.CreateJsonProblemDetailsBuilder()
                .AddJsonProblemDetails(options => options.AddDefaultMappers())
                .ConfigureApiBehaviourOptions()
                .ConfigureApiVersioningOptions();

            services.AddDbContext<BearerIdentityDbContext>(options => {
                options.UseSqlite("Data Source=bearerIdentity.db;");

                // Uncomment when using EntityFrameworkCore.InMemory.
                //options.ConfigureWarnings(warnings =>
                //    warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("a security key a security key a security key a security key"));

            services.AddIdentity<UserEntity, RoleEntity>()
                .AddEntityFrameworkStores<BearerIdentityDbContext>()
                .AddAccountManager<BearerIdentityDbContext>()
                .AddBearerTokenStore<BearerIdentityDbContext>()
                .AddBearerSignInManager<BearerIdentityDbContext>(options => {
                    options.IncludeErrorDetails = true;

                    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    options.CreateDefaultedTokenDescriptor = () => new BearerTokenDescriptor(signingCredentials);
                });

            services.AddAuthentication()
                .AddIdentityBasic<UserEntity>()
                .AddIdentityJwtRefreshToken(new JwtBearerAuthenticationOptions(securityKey))
                .AddJwtAccessToken(new JwtBearerAuthenticationOptions(securityKey) {
                    IncludeErrorDetails = true
                });

            services.AddAuthorization(options => {
                options.AddPolicy(
                    new AuthorizationPolicyBuilder(AuthenticationDefaults.AccessTokenBearerScheme)
                    .RequireAuthenticatedUser()
                    .RequireRole(TeronisIdentityBearerExampleDefaults.AdministratorRoleName)
                    .Build(),
                    AccountControllerDefaults.CanCreateRolePolicy,
                    AccountControllerDefaults.CanCreateUserPolicy);
            });

            services.AddOpenApiDocument(document => {
                document.DocumentName = "v1";
                document.Version = "v1";
                //document.ApiGroupNames = new[] { "", "1.0" };
                document.Title = "A bearer token project example.";

                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("Basic"));

                document.AddSecurity("Basic", new OpenApiSecurityScheme() {
                    Description = "Basic",
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "basic"
                });

                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("Bearer"));

                document.AddSecurity("Bearer", new OpenApiSecurityScheme() {
                    Description = "Bearer",
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                //document.OperationProcessors.Add(new OpenApiExamplesAppender());
                document.OperationProcessors.Add(new FlattenOperationsProcessor());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Outputs exceptions as RFC 7807 defined problem details.
            app.UseProblemDetails();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.Map("/", (context) => context.Response.WriteAsync("Hello Teronis.Identity"));
            });
        }
    }
}
