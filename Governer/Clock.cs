using System;

namespace Governer
{
	public class Clock
	{
		public Clock()
		{
			this.OffSet = TimeSpan.Zero;
		}

		public TimeSpan OffSet {get; set;}

		public DateTime UtcNow 
		{
			get 
			{
				return DateTime.UtcNow.Add (this.OffSet);
			}	
		}
	}
}