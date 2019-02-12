using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Pregledi.Persistence.EventSourcing
{
	public static class EventStoreInitializer
	{
		public static async Task InitializeAsync(ProjectionsManager projectionsManager, UserCredentials credentials)
		{
			var projections = await projectionsManager.ListAllAsync();
			var systemProjections = projections.Where(x => x.Name.StartsWith("$"));

			foreach (var item in systemProjections)
			{
				if (item.Status == "Stopped")
					await projectionsManager.EnableAsync(item.Name, credentials);
			}

			string naloziProjection = "Nalozi";
			if (!projections.Any(x => x.Name == naloziProjection))
			{
				string projectionFile = GetProjectionFile("Nalozi.js");
				await projectionsManager.CreateContinuousAsync(naloziProjection, projectionFile, credentials);
				await projectionsManager.EnableAsync(naloziProjection, credentials);
			}
		}

		private static string GetProjectionFile(string projectionFile)
		{
			string projectionsDir = GetProjectionsDirectory();
			string path = Path.Combine(projectionsDir, projectionFile);
			string[] lines = File.ReadAllLines(path);
			return string.Join(Environment.NewLine, lines);
		}

		private static string GetProjectionsDirectory()
		{
			return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
				@"EventSourcing/Projections");
		}
	}
}
