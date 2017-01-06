using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Metrics.Json;
using Metrics.NET.Datadog.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Metrics.NET.Datadog
{
	class HttpSender : IDisposable
	{
		private readonly string _apiKey;

		private HttpClient _httpClient = new HttpClient();
		private JsonSerializerSettings _settings;

		public HttpSender(string apiKey)
		{
			_apiKey = apiKey;

			_settings = new JsonSerializerSettings
			{
				ContractResolver = new LowercaseContractResolver()
			};
		}

		public void Send(IEnumerable<MetricJson> metrics)
		{
			var data = JsonConvert.SerializeObject(new SeriesJson {Series = metrics.ToArray()}, settings: _settings);

			var content = new ByteArrayContent(Encoding.UTF8.GetBytes(data));
			var task = _httpClient.PostAsync("https://app.datadoghq.com/api/v1/series?api_key=" + _apiKey , content);
			Task.WaitAll(task);

			//TODO: task.Result(?)
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		private class LowercaseContractResolver : DefaultContractResolver
		{
			protected override string ResolvePropertyName(string propertyName)
			{
				return propertyName.ToLower();
			}
		}
	}
}
