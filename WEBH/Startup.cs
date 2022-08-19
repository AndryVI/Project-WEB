using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebH.Models;
using WebH.Services;
using WebH.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebH
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // ��� �������� ������ � ������ � ��� ������������� ����������� (IMemoryCache)
            services.AddMemoryCache();

            // ���������� � ������ ������
            services.AddSession(options =>
            {
                options.Cookie.Name = ".MyAppV.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.IsEssential = true;
            });


            //���������� service
            services.AddTransient<CalcService>(); 

            services.AddTransient<DayService>();
            services.AddTransient<MonthsService>();

            services.AddSingleton<Visitors>();
            services.AddSingleton<WriterAndReadService>();

            services.AddTransient<NotesService>();
            
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, Services.MailService>();
            services.AddControllers();

            // ����������� ���������� ��������. ��� ������� ����� ����������� ��� ������� ����������� � ������ �������� � ����������.
            services.AddControllersWithViews()
                .AddMvcOptions(
                options =>
                {
                    options.Filters.Add<CountUsersAttribute>(); 
                    options.Filters.Add<LogAction>(); 
                    
                });

            // AddAuthentication ������������� � ��������� � DI �������, ����������� ��� ��������������
            // API �������������� � ASP.NET Core ������������ ������������� ��������� ���� ��������������
            // � �������, � ����� ���������� ����� �������������� ����������� �������������� � ������� ����,
            // � ������� ���������� � Google � Facebook
            // � ����� AddAuthentication ���������� �������� ����� ��-���������, ����� ����������
            // ������������ ���� ��������������, ������������ � ����������
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/Account/SignInAccess";     // ������ �� �������� ������, ���������� ������������
                   options.AccessDeniedPath = "/Account/Denied";    // ������������ �� ����� ������, ���� �� ���������� ��������� ������,
                                                                    // ��� �������� � ���� ��� ����

               });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (context, next) =>
            //{
            //    var visitors = context.RequestServices.GetService<Visitors>();
            //    visitors.AddSession(context.Request.Cookies[".MyApp.Session"]);
            //    await next();
            //});

            //��� ������ � �������
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            // ���������� � �������� ��������� ������� middleware �������������� � �����������
            // �� ���� ����� ����� ����������� ����������� �� ��������� ������ (� ����� ������ - Cookies)

            // ��� ��������������, ���������� �������� ������� Cookie ��������������, 
            // ���������� ������� �� ���� ������ � ������������ � ��������� �� � ����������� ������ 
            // � ��������� ��������� �������. (HttpContext.User)
            app.UseAuthentication();

            // �� ����� ����������� ���������� ��������, ���� �� � ������������ ������ � �������������� �������
            // � �������, ���� ��������������������� ������������ ��������� � ������ ��������, ��������������� 
            // ���������� Authorize, ������ �� ������� ����������� � ���������� ������������ ������������ �� �������� ��� ������
            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapDefaultControllerRoute();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("main",
                    pattern: "{title?}",
                    defaults: new
                    {
                        controller = "Home",
                        action = "index"
                    });

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller}/{action}/{id?}");

                endpoints.MapControllerRoute(
                      name: "Calculate",
                      pattern: "{controller}/{action}/{a}/{b}"
                  );
            });
        }
    }
}
