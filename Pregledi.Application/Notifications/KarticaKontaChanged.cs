using MediatR;

namespace Pregledi.Application.Notifications
{
	public class KarticaKontaChanged : INotification
	{
		public override bool Equals(object obj)
		{
			var changed = obj as KarticaKontaChanged;
			return changed != null;
		}

		public override int GetHashCode()
		{
			return default;
		}
	}
}
