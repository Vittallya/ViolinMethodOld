using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DAL.EF_Core;
using Microsoft.EntityFrameworkCore;
using LiteDB;
using Main.Options.LiteDb;
using DAL.Repositories;
using AutoMapper;
using DAL.Models;
using Main.ViewModels;

namespace Main
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
            services.AddTransient(x => new Mapper(new MapperConfiguration(x =>
            {
                x.CreateMap<Note, NoteModel>();
                x.CreateMap<NoteModel, Note>();

                x.CreateMap<PageInfo, PageInfoModel>();
                x.CreateMap<PageInfoModel, PageInfo>();

                x.CreateMap<Priem, PriemModel>();
                x.CreateMap<PriemModel, Priem>();

                x.CreateMap<PriemGroup, PriemGroupModel>();
                x.CreateMap<PriemGroupModel, PriemGroup>();

            })));

            services.AddDbContext<DbMainContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("Local"));
            });

            var liteDbOpts = Configuration.GetSection(LiteDbConfigOptions.Position).Get<LiteDbConfigOptions>();
            services.AddSingleton<ILiteDatabase>(x => new LiteDatabase(liteDbOpts.DbLocation, new LiteDbMapper()));

            services.AddSingleton<IStore<ILiteDatabase>>(x =>
            {
                return new LiteDbStore(x.GetRequiredService<ILiteDatabase>());
            });

            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.SignIn.RequireConfirmedAccount = false;
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;

            }).AddEntityFrameworkStores<DbMainContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "NotesAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
            });
            services.AddRazorPages();


            services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminArea", p => { p.RequireRole("admin"); });
            });

            services.AddRazorPages();

            services.AddControllersWithViews(x =>
            {
                x.Conventions.Add(new AdminAreaConvention("Admin", "AdminArea"));
            }).
            SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).
            AddSessionStateTempDataProvider();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
