using System;
using System.Collections.Generic;
using Metrics.MetricData;
using Metrics.NET.Datadog.Models;
using Metrics.Reporters;

namespace Metrics.NET.Datadog
{
	class DatadogReport : BaseReport
	{
		private readonly HttpSender _httpSender;
		private readonly string _host;
		private readonly string _app;

		private readonly List<MetricJson> _metrics = new List<MetricJson>();
		private double _timestamp;

		public DatadogReport(HttpSender httpSender, string host, string app)
		{
			_httpSender = httpSender;
			_host = host;
			_app = app;
		}

		protected override void StartReport(string contextName)
		{
			_metrics.Clear();
			_timestamp = (DateTimeOffset.UtcNow - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds;
		}

		protected override void EndReport(string contextName)
		{
			_httpSender.Send(_metrics);
		}

		protected override void ReportGauge(string name, double value, Unit unit, MetricTags tags)
		{
			Report(name, value, tags);
		}

		protected override void ReportCounter(string name, CounterValue value, Unit unit, MetricTags tags)
		{
			Report(name, value.Count, tags);
		}

		protected override void ReportMeter(string name, MeterValue value, Unit unit, TimeUnit rateUnit, MetricTags tags)
		{
			Report(name, "count", value.Count, tags);
			Report(name, "15m", value.FifteenMinuteRate, tags);
			Report(name, "5m", value.FiveMinuteRate, tags);
			Report(name, "1m", value.OneMinuteRate, tags);
			Report(name, "avg", value.MeanRate, tags);
		}

		protected override void ReportHistogram(string name, HistogramValue value, Unit unit, MetricTags tags)
		{
			Report(name, "count", value.Count, tags);
			Report(name, "max", value.Max, tags);
			Report(name, "avg", value.Mean, tags);
			Report(name, "median", value.Median, tags);
			Report(name, "min", value.Min, tags);
			Report(name, "stdDev", value.StdDev, tags);

			Report(name, "75percentile", value.Percentile75, tags);
			Report(name, "95percentile", value.Percentile95, tags);
			Report(name, "98percentile", value.Percentile98, tags);
			Report(name, "99percentile", value.Percentile99, tags);
			Report(name, "999percentile", value.Percentile999, tags);
		}

		protected override void ReportTimer(string name, TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
		{
			ReportMeter(name + ".rate", value.Rate, unit, rateUnit, tags);

			ReportHistogram(name, value.Histogram, unit, tags);
		}

		protected override void ReportHealth(HealthStatus status)
		{
			//TODO?
		}

		private void Report(string name, string subname, double value, MetricTags tags)
		{
			Report(name + "." + subname, value, tags);
		}

		private void Report(string name, double value, MetricTags tags)
		{
			_metrics.Add(new MetricJson
			{
				Host = _host,
				Metric = FixName(name),
				Points = new[] { new[] { _timestamp, value } },
				Tags = tags.Tags
			});
		}

		private string FixName(string name)
		{
			var closingSquare = name.LastIndexOf("] ", StringComparison.Ordinal);

			if (closingSquare >= 0)
			{
				//[DESKTOP-RANDOMBITS.Metrics_NET_Datadog_TestHarness_vshost] mymeter
				//[DESKTOP-RANDOMBITS.Metrics_NET_Datadog_TestHarness_vshost - System] mymeter
				//[DESKTOP-RANDOMBITS.Metrics_NET_Datadog_TestHarness_vshost - Application] mymeter
				//

				var dash = name.LastIndexOf(" - ", closingSquare, StringComparison.Ordinal);
				if (dash >= 0)
					name = name.Substring(dash + 3, closingSquare - dash - 2) + "." + name.Substring(closingSquare + 2);
				else
					name = name.Substring(closingSquare + 2);
			}

			return _app + "." + name;
		}
	}
}
