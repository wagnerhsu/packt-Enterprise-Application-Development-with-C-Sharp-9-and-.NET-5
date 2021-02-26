using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Configuration.Xml;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestConfiguration.CustomConfiguration;


namespace TestConfiguration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig = config.Build();

                    //AKV Configuration provider
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var keyVaultClient = new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(
                            azureServiceTokenProvider.KeyVaultTokenCallback));

                    config.AddAzureKeyVault(
                      $"https://{builtConfig["KeyVault:Name"]}.vault.azure.net/",
                      builtConfig["KeyVault:AppClientId"],
                      builtConfig["KeyVault:AppClientSecret"]);

                    //JSON Configuration provider
                    config.AddJsonFile("AdditionalConfig.json",
                    optional: true,
                    reloadOnChange: true);

                    //XML Configuration provider
                    config.AddXmlFile("AdditionalXMLConfig.xml",
                    optional: true,
                    reloadOnChange: true);

                    //Custom SQL Configuration provider
                    config.AddSql("Connection string", "Query");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }



}
