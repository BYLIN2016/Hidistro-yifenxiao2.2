using System;
namespace HtmlAgilityPack
{
	internal class HtmlCmdLine
	{
		internal static bool Help;
		static HtmlCmdLine()
		{
			HtmlCmdLine.Help = false;
			HtmlCmdLine.ParseArgs();
		}
		internal static string GetOption(string name, string def)
		{
			string result = def;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				HtmlCmdLine.GetStringArg(commandLineArgs[i], name, ref result);
			}
			return result;
		}
		internal static string GetOption(int index, string def)
		{
			string result = def;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int num = 0;
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				if (HtmlCmdLine.GetStringArg(commandLineArgs[i], ref result))
				{
					if (index == num)
					{
						return result;
					}
					result = def;
					num++;
				}
			}
			return result;
		}
		internal static bool GetOption(string name, bool def)
		{
			bool result = def;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				HtmlCmdLine.GetBoolArg(commandLineArgs[i], name, ref result);
			}
			return result;
		}
		internal static int GetOption(string name, int def)
		{
			int result = def;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				HtmlCmdLine.GetIntArg(commandLineArgs[i], name, ref result);
			}
			return result;
		}
		private static void GetBoolArg(string Arg, string Name, ref bool ArgValue)
		{
			if (Arg.Length < Name.Length + 1)
			{
				return;
			}
			if ('/' != Arg[0] && '-' != Arg[0])
			{
				return;
			}
			if (Arg.Substring(1, Name.Length).ToLower() == Name.ToLower())
			{
				ArgValue = true;
			}
		}
		private static void GetIntArg(string Arg, string Name, ref int ArgValue)
		{
			if (Arg.Length < Name.Length + 3)
			{
				return;
			}
			if ('/' != Arg[0] && '-' != Arg[0])
			{
				return;
			}
			if (Arg.Substring(1, Name.Length).ToLower() == Name.ToLower())
			{
				try
				{
					ArgValue = Convert.ToInt32(Arg.Substring(Name.Length + 2, Arg.Length - Name.Length - 2));
				}
				catch
				{
				}
			}
		}
		private static bool GetStringArg(string Arg, ref string ArgValue)
		{
			if ('/' == Arg[0] || '-' == Arg[0])
			{
				return false;
			}
			ArgValue = Arg;
			return true;
		}
		private static void GetStringArg(string Arg, string Name, ref string ArgValue)
		{
			if (Arg.Length < Name.Length + 3)
			{
				return;
			}
			if ('/' != Arg[0] && '-' != Arg[0])
			{
				return;
			}
			if (Arg.Substring(1, Name.Length).ToLower() == Name.ToLower())
			{
				ArgValue = Arg.Substring(Name.Length + 2, Arg.Length - Name.Length - 2);
			}
		}
		private static void ParseArgs()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				HtmlCmdLine.GetBoolArg(commandLineArgs[i], "?", ref HtmlCmdLine.Help);
				HtmlCmdLine.GetBoolArg(commandLineArgs[i], "h", ref HtmlCmdLine.Help);
				HtmlCmdLine.GetBoolArg(commandLineArgs[i], "help", ref HtmlCmdLine.Help);
			}
		}
	}
}
