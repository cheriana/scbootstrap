namespace ScBootstrap.Logic.Pipelines
{
    using System.Linq;
    using System.Web.Mvc;
    using Sitecore.Configuration;
    using Sitecore.Pipelines;
    using Sitecore.Pipelines.HttpRequest;
    using StackExchange.Profiling;
    using StackExchange.Profiling.MVCHelpers;

    public static class MiniProfilerSettings
    {
        public static bool EnableMiniProfiler = Settings.GetBoolSetting("EnableMiniProfiler", false);
    }

    public class MiniProfilerConfig
    {
        public void Process(PipelineArgs args)
        {
            if (!MiniProfilerSettings.EnableMiniProfiler) return;

            // Ignore default paths
            var ignored = MiniProfiler.Settings.IgnoredPaths.ToList();
            ignored.Add("WebResource.axd");
            ignored.Add("/sitecore/");
            ignored.Add("/~/media/");
            MiniProfiler.Settings.IgnoredPaths = ignored.ToArray();

            // Setup profiler for Controllers via a Global ActionFilter
            GlobalFilters.Filters.Add(new ProfilingActionFilter());

            // Intercept ViewEngines to profile all partial views and regular views.
            var copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            copy.ForEach(i => ViewEngines.Engines.Add(new ProfilingViewEngine(i)));
        }
    }

    public class MiniProfilerBeginRequest : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (!MiniProfilerSettings.EnableMiniProfiler) return;
            MiniProfiler.Start();
        }
    }

    public class MiniProfilerEndRequest : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (!MiniProfilerSettings.EnableMiniProfiler) return;
            MiniProfiler.Stop();
        }
    }
}
