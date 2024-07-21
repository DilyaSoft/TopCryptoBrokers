using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.Settings.Models;

namespace TopCryptoDev
{
    class Program
    {
        private static IConfiguration Config { get; set; }

        static Program()
        {
            Config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("appsettings.Development.json", true, true)
            .Build();
        }

        static async Task TestSettings()
        {
            var linksTitleSettings = Config
                .GetSection("linksTitleSettings")
                .Get<List<SettingFromConfigDTO>>();

            foreach (var item in linksTitleSettings)
            {
                Console.WriteLine(item);
            }
        }

        static void Main(string[] args)
        {
            TestSettings().Wait();

            Console.WriteLine("\n\nHello World!");
            Console.ReadKey();
        }
    }
}
