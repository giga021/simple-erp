using Microsoft.Extensions.Logging;
using System;

namespace Knjizenje.Service.Infrastructure
{
	public class EventStoreLogger : EventStore.ClientAPI.ILogger
	{
		private readonly Microsoft.Extensions.Logging.ILogger logger;

		public EventStoreLogger(ILoggerFactory loggerFactory)
		{
			this.logger = loggerFactory.CreateLogger("EventStore");
		}

		public void Debug(string format, params object[] args)
		{
			logger.LogDebug(format, args);
		}

		public void Debug(Exception ex, string format, params object[] args)
		{
			logger.LogDebug(ex, format, args);
		}

		public void Error(string format, params object[] args)
		{
			logger.LogError(format, args);
		}

		public void Error(Exception ex, string format, params object[] args)
		{
			logger.LogError(ex, format, args);
		}

		public void Info(string format, params object[] args)
		{
			logger.LogInformation(format, args);
		}

		public void Info(Exception ex, string format, params object[] args)
		{
			logger.LogInformation(ex, format, args);
		}
	}
}
