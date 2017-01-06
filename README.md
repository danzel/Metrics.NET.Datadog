#Metrics.NET.Datadog
[![Build status](https://ci.appveyor.com/api/projects/status/f2vv2lt322g4dvvv?svg=true)](https://ci.appveyor.com/project/danzel/metrics-net-datadog)

https://www.nuget.org/packages/Metrics.NET.Datadog

## What is the Datadog Reporter for Metrics.NET
The [Metrics.NET](https://github.com/Recognos/Metrics.NET) library provides a way of instrumenting applications with custom metrics (timers, histograms, counters etc) that can be reported in various ways and can provide insights on what is happening inside a running application.

This assembly provides a mechanism to report the metrics gathered by Metrics.NET to Datadog via their Web API.

##Configuring the DatadogReporter

**You only need to configure the DatadogReporter once per application start.**

```c#
Metric.Config
	.WithReporting(config => config
		.WithDatadog("PutYourApiKeyHere", Environment.MachineName, "PutYourAppNameHere", TimeSpan.FromSeconds(5))
	)
	.WithAllCounters();
```

###Your Datadog API Token
Log in to Datadog, Integrations -> APIs.
You can create and manage your API keys at the top of this list.
