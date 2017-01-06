using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.NET.Datadog.Models
{
	/// <summary>
	/// For serializing http://docs.datadoghq.com/api/?lang=console#metrics
	/// </summary>
	public class MetricJson
	{
		public string Metric { get; set; }

		public double[][] Points { get; set; }

		public string Host { get; set; }

		public string[] Tags { get; set; }
	}
}
