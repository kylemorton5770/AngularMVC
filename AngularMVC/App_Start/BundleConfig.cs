﻿using System.Web;
using System.Web.Optimization;

namespace AngularMVC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                                  "~/Content/site.css"));

            //minify and compresses all js in controllers and adds to 
            //AngularMVC.js file, minifies all
            bundles.Add(new ScriptBundle("~/bundles/AngularMVC")
                   .IncludeDirectory("~/Scripts/Controllers", "*.js")
                   .Include("~/Scripts/AngularMVC.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
