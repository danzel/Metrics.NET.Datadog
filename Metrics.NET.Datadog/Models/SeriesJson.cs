using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.NET.Datadog.Models
{
	public class SeriesJson
	{
		public MetricJson[] Series { get; set; }
	}
}
