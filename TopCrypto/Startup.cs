using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TestAuth.Data;
using TopCrypto.DataLayer.Data.Models;
using TopCrypto.DataLayer.Services.Broker;
using TopCrypto.DataLayer.Services.Broker.Interfaces;
using TopCrypto.DataLayer.Services.CoinGraph;
using TopCrypto.DataLayer.Services.CoinGraph.Interfaces;
using TopCrypto.DataLayer.Services.CoinIds;
using TopCrypto.DataLayer.Services.CoinIds.Interfaces;
using TopCrypto.DataLayer.Services.CoinInfo;
using TopCrypto.DataLayer.Services.CoinInfo.Interfaces;
using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.DataLayer.Services.CoinMarket;
using TopCrypto.DataLayer.Services.CoinMarket.Interfaces;
using TopCrypto.DataLayer.Services.CustomCache;
using TopCrypto.DataLayer.Services.DBLogging;
using TopCrypto.DataLayer.Services.DBLogging.Interfaces;
using TopCrypto.DataLayer.Services.FiatCurrency;
using TopCrypto.DataLayer.Services.FiatCurrency.Interfaces;
using TopCrypto.DataLayer.Services.FiatCurrency.Models;
using TopCrypto.DataLayer.Services.Helpers;
using TopCrypto.DataLayer.Services.Language;
using TopCrypto.DataLayer.Services.Language.Interfaces;
using TopCrypto.DataLayer.Services.News;
using TopCrypto.DataLayer.Services.News.Interfaces;
using TopCrypto.DataLayer.Services.Search;
using TopCrypto.DataLayer.Services.Search.Interfaces;
using TopCrypto.DataLayer.Services.Settings;
using TopCrypto.DataLayer.Services.Settings.Interfaces;
using TopCrypto.Services;
using TopCrypto.ServicesLayer;
using TopCrypto.ServicesLayer.CoinGraph;
using TopCrypto.ServicesLayer.CoinGraph.Interfaces;
using TopCrypto.ServicesLayer.CoinIds;
using TopCrypto.ServicesLayer.CoinIds.Interfaces;
using TopCrypto.ServicesLayer.CoinInfo;
using TopCrypto.ServicesLayer.CoinInfo.Interfaces;
using TopCrypto.ServicesLayer.CoinInfo.Models;
using TopCrypto.ServicesLayer.CoinMarket;
using TopCrypto.ServicesLayer.CoinMarket.Interfaces;
using TopCrypto.ServicesLayer.CoinMarketAndIds;
using TopCrypto.ServicesLayer.CoinMarketAndIds.Interfaces;
using TopCrypto.ServicesLayer.DBLogging;
using TopCrypto.ServicesLayer.DBLogging.Interfaces;
using TopCrypto.ServicesLayer.FiatCurrency;
using TopCrypto.ServicesLayer.FiatCurrency.Interfaces;
using TopCrypto.ServicesLayer.FiatCurrency.Models;
using TopCrypto.ServicesLayer.Interfaces;
using TopCrypto.ServicesLayer.Language;
using TopCrypto.ServicesLayer.Language.Interfaces;
using TopCrypto.ServicesLayer.News;
using TopCrypto.ServicesLayer.News.Interfaces;
using TopCrypto.ServicesLayer.Search;
using TopCrypto.ServicesLayer.Search.Interfaces;
using TopCrypto.ServicesLayer.Settings;
using TopCrypto.ServicesLayer.Settings.Interfaces;
using TopCrypto.ServicesLayer.WebSockets;
using TopCrypto.ServicesLayer.WebSockets.Interfaces;

using Serilog;
using Serilog.Events;

namespace TopCrypto
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
            services.AddLogging();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            #region Injection
            services.AddSingleton<InternalCache, InternalCache>();

            services.AddTransient<EndPointHelper>();

            services.AddTransient<ICoinMarketService, CoinMarketService>();
            services.AddTransient<ICoinMarketDataService, CoinMarketDataService>();

            services.AddTransient<ICoinMarketAndIds, CoinMarketAndIds>();

            services.AddTransient<ISearchDataService, SearchDataService>();
            services.AddTransient<ISearchService, SearchService>();

            services.AddTransient<JsonHelper, JsonHelper>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ICoinInfoPricesDataService, CoinInfoPricesDataService>();

            services.AddTransient<IBrokerDataService, BrokerDataService>();
            services.AddTransient<IBrokersService, BrokersService>();

            services.AddTransient<ILanguageDataService, LanguageDataService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<IMarketDataSocketManagerHelper, MarketDataSocketManagerHelper>();

            services.AddTransient<ICoinInfoService, CoinInfoService>();
            services.AddTransient<ICryptoCurencyPriceSaver, CoinInfoPricesService>();
            services.AddTransient<ICoinInfoBackroundHelper, CoinInfoBackroundHelper>();

            services.AddSingleton<AbstractComparisonService<CoinInfoComparisonDTO, CoinInfoDTO>,
                CoinInfoComparisonService>();
            services.AddTransient<ICoinInfoDataService, CoinInfoDataService>();

            services.AddTransient<ICoinIdsDataService, CoinIdsDataService>();
            services.AddTransient<ICoinIdsService, CoinIdsService>();

            services.AddTransient<ICoinInfoGraphDataService, CoinInfoGraphDataService>();
            services.AddTransient<ICoinInfoGraphService, CoinInfoGraphService>();

            services.AddTransient<IDBLoggingDataService, DBLoggingDataService>();
            services.AddTransient<IDBLoggingService, DBLoggingService>();


            services.AddTransient<IFiatCurrencyService, FiatCurrencyService>();
            services.AddSingleton<AbstractComparisonService<FiatCurrencyComparisonDTO, FiatCurrencyDTO>
                , FiatCurrencyComparisonService>();
            services.AddTransient<IFiatCurrencyDataService, FiatCurrencyDataService>();
            services.AddTransient<IFiatCurrencyBackroundHelper, FiatCurrencyBackroundHelper>();
            services.AddTransient<ISocketDataWrapperHelpers, SocketDataWrapperHelpers>();

            services.AddTransient<INewsDataService, NewsDataService>();
            services.AddTransient<INewsService, NewsService>();

            services.AddTransient<ISettingsDataService, SettingsDataService>();
            services.AddTransient<ISettingsService, SettingsService>();
            #endregion

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder,
                options => options.ResourcesPath = "Resources");

            #region sockets
            //https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/multi-container-microservice-net-applications/background-tasks-with-ihostedservice
            services.AddSingleton<ISocketManager, MarketDataSocketManager>();
            services.AddSingleton<IHostedService, CoinDataBackroundService>();
            services.AddSingleton<IHostedService, CoinGraphBackgroundService>();
            services.AddSingleton<IHostedService, FiatCurrencyBackgroundService>();
            services.AddSingleton<IHostedService, CoinIdsBackroundService>();
            services.AddSingleton<IHostedService, CoinMarketBackroundService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, 
            UserManager<ApplicationUser> _userManager)
        {
            AddDefaultUser(_userManager).Wait();

            #region Localization
            IList<CultureInfo> supportedCultures = GetSupportedCultures();
            var localizationOptions = GetRequestLocalizationOptions(supportedCultures);
            app.UseRequestLocalization(localizationOptions);
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true                    
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "Handled {RequestPath}";

                options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Warning;

                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                };
            });

            app.UseAuthentication();
            app.UseWebSockets();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "defult", template: "{controller}/{action}/{id?}");
            });
            app.MapWhen(context => context.Request.Path.Value.StartsWith("/admin", StringComparison.OrdinalIgnoreCase), builder =>
            {               
                builder.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute("spa-admin-fallback", new { controller = "AdminRoot", action = "Index" });
                });
            });
            app.MapWhen(context => !context.Request.Path.Value.StartsWith("/admin", StringComparison.OrdinalIgnoreCase), builder =>
            {
                app.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute(name: "spa-root-fallback", new { controller = "Root", action = "Index" });
                });
            });           
        }

        private async Task AddDefaultUser(UserManager<ApplicationUser> _userManager)
        {
            var admin = await _userManager.FindByEmailAsync("magnastrazh@gmail.com");

            if (admin == null)
            {
                var user = new ApplicationUser { UserName = "devTest", Email = "magnastrazh@gmail.com" };
                var result = await _userManager.CreateAsync(user, "devTest123%");
            }
        }

        private RequestLocalizationOptions GetRequestLocalizationOptions(IList<CultureInfo> supportedCultures)
        {
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };
            var provider = new CookieRequestCultureProvider();
            localizationOptions.RequestCultureProviders.Insert(0, provider);
            return localizationOptions;
        }

        private IList<CultureInfo> GetSupportedCultures()
        {
            IConfigurationSection myArraySection = Configuration.GetSection("supportedCultures");
            var culturesEnum = myArraySection.AsEnumerable().GetEnumerator();

            IList<CultureInfo> supportedCultures = new List<CultureInfo>();
            while (culturesEnum.MoveNext())
            {
                var val = culturesEnum.Current.Value;
                if (val == null || string.IsNullOrWhiteSpace(val)) continue;
                supportedCultures.Add(new CultureInfo(val));
            }

            return supportedCultures;
        }
    }
}
