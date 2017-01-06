using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Metrics;

namespace Metrics.NET.Datadog.TestHarness
{
	class Program
	{
		static void Main(string[] args)
		{
			Metric.Config
				.WithReporting(config => config
					.WithDatadog("PutYourApiKeyHere", Environment.MachineName, "MetricsNetDatadogTest", TimeSpan.FromSeconds(5))
					.WithConsoleReport(TimeSpan.FromSeconds(5))
				)
				.WithAllCounters();

			var rand = new Random();

			var histogram = Metric.Histogram("histo", Unit.Bytes, tags: new MetricTags("tag1", "tag2"));
			var counter = Metric.Counter("counter", Unit.Calls);
			Metric.Gauge("gauge", () => rand.NextDouble(), Unit.Percent);
			var meter = Metric.Meter("meter", Unit.Commands);
			var timer = Metric.Timer("timer", Unit.Commands);

			while (true)
			{
				Console.WriteLine("...");
				histogram.Update(rand.Next(0, 10000));

				for (var i = 0; i < rand.Next(0, 100); i++)
					counter.Increment();

				for (var i = 0; i < rand.Next(0, 100); i++)
					meter.Mark();

				using (timer.NewContext())
					Thread.Sleep(TimeSpan.FromSeconds(rand.NextDouble()));

				Thread.Sleep(TimeSpan.FromSeconds(1));
			}
		}
	}
}
