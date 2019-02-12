using System;

namespace Pregledi.API
{
	public class AppSettings
	{
		public string ConnectionString { get; set; }
		public string EVENTSTORE_USERNAME { get; set; }
		public string EVENTSTORE_PASSWORD { get; set; }
		public string EVENTSTORE_HOST { get; set; }
		public int EVENTSTORE_TCP_PORT { get; set; }
		public int EVENTSTORE_HTTP_PORT { get; set; }
		public string SERVICEBUS_USERNAME { get; set; }
		public string SERVICEBUS_PASSWORD { get; set; }
		public string SERVICEBUS_HOST { get; set; }
		public string IDENTITY_HOST { get; set; }
		public string API_NAME { get; set; }
		public int EventStoreMaxQueueSize { get; set; }
		public int EventStoreReadBatchSize { get; set; }
		public string MYSQL_VERSION { get; set; }
	}
}
