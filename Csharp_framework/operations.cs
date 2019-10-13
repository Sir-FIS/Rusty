using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using static ShellCodeLauncher.decode;


namespace ShellCodeLauncher
{
	class Options
	{

		[Option('e', "decode method", Default = true,
		  HelpText = "how the data will be processed")]
		public string decoder { get; set; }

	}
	class operations

	{
		
		public static byte[] Create_shellcode(string[] args, string input)

		{
			var opts = new Options();

			Parser.Default.ParseArguments<Options>(args).WithParsed(parsed => opts = parsed);
			string decode_method = opts.decoder;
			var instance = new decode();



			switch (decode_method)
			{
				case "b64":			
					byte[] raw = Base64Decode(input);
					byte[] shellcode = new byte[raw.Length];
					System.Buffer.BlockCopy(raw, 0, shellcode, 0, raw.Length);
					return shellcode;

				case "encrypt":
					
					Console.Write("Enter password - ");
					string password = Console.ReadLine();
					shellcode = instance.Decrypt(password, input);
					return shellcode;
				default: 
					System.Console.WriteLine("No decode method given, running raw shellcode.");
					decode_method = null;
					shellcode = Encoding.ASCII.GetBytes(input); 
					return shellcode;

			}

		}
		}
	}

