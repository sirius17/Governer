using System;

namespace Governer.Internal
{
	public class SystemTimeSource : ITimeSource
	{
		#region ITimeSource implementation

		public DateTime GetUtcNow ()
		{
			return DateTime.UtcNow;
		}

		#endregion
	}
}

