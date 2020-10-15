using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using BusinessLogic;
using DataAccess.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogic.Interfaces;
using DataAccess.InterfaceRepos;

namespace WebApp
{
    public class Startup
    {
        private const string AllowOrigin = "allowOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<ILoanRepos<Loan>>(s =>
            {
                return RepositoryFactory<Loan>.CreateLoanRepos().WithLoan(() =>
                {
                    var conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
                    conn.Open();
                    return conn;
                });
            });
            services.AddTransient<IUserRepos<Person>>(s =>
            {
                return RepositoryFactory<Person>.CreateUserRepos().WithUser(() =>
                {
                    var conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
                    conn.Open();
                    return conn;
                });
            });
            services.AddTransient<ICardRepos<Card>>(s =>
            {
                return RepositoryFactory<Card>.CreateCardRepos().WithCard(() =>
                {
                    var conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
                    conn.Open();
                    return conn;
                });
            });
            services.AddTransient<IBookRepos<Book>>(s =>
            {
                return RepositoryFactory<Book>.CreateBookRepos().WithBook(() =>
                {
                    var conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
                    conn.Open();
                    return conn;
                });
            });
            services.AddTransient<IGuestLibraryRepos<Person>>(s =>
            {
                return RepositoryFactory<Person>.CreateGuestRepos().With(() =>
                {
                    var conn = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
                    conn.Open();
                    return conn;
                });
            });

            services.AddTransient<ILoanLogic, LoanLogic>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors(c =>
            {
                c.AddPolicy(AllowOrigin, options => options.WithOrigins("https://localhost:4200", "http://localhost:44360")
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors(AllowOrigin);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
