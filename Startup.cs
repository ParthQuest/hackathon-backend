using HackathonAPI.Business;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(HackathonAPI.Startup))]
namespace HackathonAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;
            builder.Services.AddTransient<IDMSService, DMSService>();

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = null;
            });

            builder.Services.AddTransient<QueryFactory>(e =>
            {
                var connection = new MySqlConnection(configuration.GetConnectionString("MySqlConnectionString"));
                var compiler = new MySqlCompiler();
                return new QueryFactory(connection, compiler);
            });
        }
    }
}
