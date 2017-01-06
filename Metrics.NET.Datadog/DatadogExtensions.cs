using System;
using Metrics.Reports;

namespace Metrics.NET.Datadog
{
	public static class DatadogExtensions
	{
		/// <summary>
		/// Schedule a Report to be executed and sent to Datadog at a fixed <paramref name="interval"/>.
		/// </summary>
		/// <param name="reports"></param>
		/// <param name="apiKey">API Key from Datadog (Integrations - APIs - API Keys)</param>
		/// <param name="host">Host name to show in Datadog</param>
		/// <param name="app">Application name to show in Datadog</param>
		/// <param name="interval">Interval at which to run the report.</param>
		/// <returns></returns>
		public static MetricsReports WithDatadog(this MetricsReports reports, string apiKey, string host, string app, TimeSpan interval)
		{
			return reports.WithReport(new DatadogReport(new HttpSender(apiKey), host, app), interval);
		}
	}
}
