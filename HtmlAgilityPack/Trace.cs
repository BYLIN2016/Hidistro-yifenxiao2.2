using System;
namespace HtmlAgilityPack
{
	internal class Trace
	{
		internal static Trace _current;
		internal static Trace Current
		{
			get
			{
				if (Trace._current == null)
				{
					Trace._current = new Trace();
				}
				return Trace._current;
			}
		}
		public static void WriteLine(string message, string category)
		{
			Trace.Current.WriteLineIntern(message, category);
		}
		private void WriteLineIntern(string message, string category)
		{
		}
	}
}
