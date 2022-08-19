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

            // дл€ хранени€ сессии в пам€ти и дл€ использовани€ кэшировани€ (IMemoryCache)
            services.AddMemoryCache();

            // ƒобавление в проект сессию
            services.AddSession(options =>
            {
                options.Cookie.Name = ".MyAppV.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.IsEssential = true;
            });


            //ƒобавление service
            services.AddTransient<CalcService>(); 

            services.AddTransient<DayService>();
            services.AddTransient<MonthsService>();

            services.AddSingleton<Visitors>();
            services.AddSingleton<WriterAndReadService>();

            services.AddTransient<NotesService>();
            
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, Services.MailService>();
            services.AddControllers();

            // –егистраци€ глобальных фильтров. Ёти фильтры будут срабатывать дл€ каждого контроллера и метода действи€ в приложении.
            services.AddControllersWithViews()
                .AddMvcOptions(
                options =>
                {
                    options.Filters.Add<CountUsersAttribute>(); 
                    options.Filters.Add<LogAction>(); 
                    
                });

            // AddAuthentication конфигурирует и добавл€ет в DI сервисы, необходимые дл€ аутентификации
            // API аутентификации в ASP.NET Core поддерживает использование множества схем аутентификации
            //   примеру, в одном приложении может присутствовать возможность аутентификации с помощью куки,
            // с помощью интеграции с Google и Facebook
            // ¬ метод AddAuthentication передаетс€ название схемы по-умолчанию, далее происходит
            // конфигураци€ схем аутентификации, используемых в приложении
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/Account/SignInAccess";     // ссылка на страницу логина, приложение перенаправит
                   options.AccessDeniedPath = "/Account/Denied";    // пользовател€ по этому адресу, если он попытаетс€ запросить ресурс,
                                                                    // дл€ которого у него нет прав

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

            //дл€ работы с сесс€ми
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            // ƒобавление в пайплайн обработки запроса middleware аутентификации и авторизации
            // Ќа этом этапе будет произведена авторизаци€ по указанным схемам (в нашем случае - Cookies)

            // ѕри аутентификации, приложение проверит наличие Cookie аутентификации, 
            // попытаетс€ извлечь из него данные о пользователе и поместить их в специальный объект 
            // в контексте обработки запроса. (HttpContext.User)
            app.UseAuthentication();

            // Ќа этапе авторизации приложение проверит, есть ли у пользоваетл€ доступ к запрашиваемому ресурсу
            //   примеру, если неаутентифицированный пользователь обратитс€ к методу действи€, декорированному 
            // аттрибутом Authorize, запрос не пройдет авторизацию и приложение перенаправит пользовател€ на страницу дл€ логина
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
