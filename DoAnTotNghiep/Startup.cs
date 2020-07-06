using AutoMapper;
using DoAnTotNghiep.MappingProfile;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebData.Implementation;
using WebData.Models;

namespace DoAnTotNghiep
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<quanlykhoahocContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedEmail = true).AddEntityFrameworkStores<quanlykhoahocContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = "541536780060115";// Configuration["Authentication:Facebook:541536780060115"];
                    facebookOptions.AppSecret = "00c6ebc65b45b3e62463faff6a580d63"; //Configuration["Authentication:Facebook:00c6ebc65b45b3e62463faff6a580d63"];
                })
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/Home/Error404";
                    options.LogoutPath = "/Home/Index";
                    options.LoginPath = "/Home/Index";
                });
            services.AddTransient<SendMailHelper>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICourseCategoryService, CourseCategoryService>();
            services.AddScoped<ICourseLessonService, CourseLessonService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IChapterService, ChapterService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<ILessonCommentService, LessonCommentService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWareHouseService, WareHouseService>();

            services.Configure<FormOptions>(opt =>
            {
                opt.MultipartBodyLengthLimit = int.MaxValue;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddDirectoryBrowser();
            services.AddSingleton(mapper);
            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc();
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
            }

            app.UseFileServer(enableDirectoryBrowsing: false);
            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}