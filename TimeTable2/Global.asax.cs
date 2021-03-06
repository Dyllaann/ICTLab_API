﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.ApplicationInsights.Extensibility;

namespace TimeTable2
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var applicationInsights = WebConfigurationManager.AppSettings["ApplicationInsights"];
            if (!string.IsNullOrEmpty(applicationInsights))
            {
                TelemetryConfiguration.Active.InstrumentationKey = applicationInsights;
            }
            log4net.Config.XmlConfigurator.Configure();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
