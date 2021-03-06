﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.AsyncDecoderImpl;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Service
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
            services.AddMvc();

            String[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Length != 2)
            {
                Console.Error.WriteLine("Usage: dotnet Service.dll --decoder-config=<async_decoder_config.json>\n" +
                                        "<async_decoder_config.json> is the location that stores the " +
                                        "configuration for `AsyncDecoder`.");
            }

            AsyncDecoderConfig config = AsyncDecoderConfig.FromFile(Regex.Match(arguments[1], "--decoder-config=(.*)").Groups[1].Value);
            services.AddSingleton<IDecoder, AsyncDecoder>(decoder => new AsyncDecoder(config));

            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
	    Console.WriteLine("Using config: " + AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(LogManager.GetRepository(Assembly.GetCallingAssembly()), logCfg);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
