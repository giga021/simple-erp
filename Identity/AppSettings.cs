using System;

namespace Identity
{
	public class AppSettings
	{
		public string ConnectionStringUsers { get; set; }
		public string ConnectionStringClients { get; set; }
		public string ConnectionStringGrants { get; set; }
		public string MYSQL_VERSION { get; set; }
	}
}
