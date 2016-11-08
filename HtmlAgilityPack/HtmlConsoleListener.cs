using System;
using System.Diagnostics;
namespace HtmlAgilityPack
{
	internal class HtmlConsoleListener : TraceListener
	{
		public override void Write(string Message)
		{
			this.Write(Message, "");
		}
		public override void Write(string Message, string Category)
		{
			Console.Write("T:" + Category + ": " + Message);
		}
		public override void WriteLine(string Message)
		{
			this.Write(Message + "\n");
		}
		public override void WriteLine(string Message, string Category)
		{
			this.Write(Message + "\n", Category);
		}
	}
}
