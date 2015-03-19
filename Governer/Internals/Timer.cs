using System;

namespace Governer.Internal
{
	public abstract class Timer
	{
		public abstract int MeasureInSeconds(Action action);
	}

	public class DateTimeTimer : Timer
	{
		#region implemented abstract members of Timer
		public override int MeasureInSeconds (Action action)
		{
			DateTime started = DateTime.Now;
			action ();
			return Convert.ToInt32 (DateTime.Now.Subtract (started).TotalSeconds);
		}
		#endregion
		
	}
}

