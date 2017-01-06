using System;
using Metrics.Reports;

namespace Metrics.NET.Datadog
{
	public static class DatadogExtensions
	{
		public static MetricsReports WithDatadog(this MetricsReports reports, string apiKey, string host, string app, TimeSpan interval)
		{
			return reports.WithReport(new DatadogReport(new HttpSender(apiKey), host, app), interval);
		}
	}
}
