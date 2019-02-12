using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Knjizenje.Persistence.EventSourcing
{
	public static class EventStoreInitializer
	{
		public static async Task InitializeAsync(ProjectionsManager projectionsManager, UserCredentials credentials)
		{
			if (projectionsManager == null)
				throw new ArgumentNullException(nameof(projectionsManager));
			if (credentials == null)
				throw new ArgumentNullException(nameof(credentials));

			var projections = await projectionsManager.ListAllAsync();
			var systemProjections = projections.Where(x => x.Name.StartsWith("$"));

			foreach (var item in systemProjections)
			{
				if (item.Status == "Stopped")
					await projectionsManager.EnableAsync(item.Name, credentials);
			}

			if (!projections.Any(x => x.Name == Projections.ZaglavljaNaloga))
			{
				string projectionFile = GetProjectionFile("ZaglavljaNaloga.js");
				await projectionsManager.CreateContinuousAsync(Projections.ZaglavljaNaloga, projectionFile, credentials);
				await projectionsManager.EnableAsync(Projections.ZaglavljaNaloga, credentials);
			}
		}

		private static string GetProjectionFile(string projectionFile)
		{
			if (projectionFile == null)
				throw new ArgumentNullException(nameof(projectionFile));

			string projectionsDir = GetProjectionsDirectory();
			string path = Path.Combine(projectionsDir, projectionFile);
			string[] lines = File.ReadAllLines(path);
			return string.Join(Environment.NewLine, lines);
		}

		private static string GetProjectionsDirectory()
		{
			return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
					"EventSourcing/Projections");
		}
	}
}
