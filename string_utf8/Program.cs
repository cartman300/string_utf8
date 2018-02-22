using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InteropUtils {
	class Program {
		static IDisposable Shite() {
			return default(IDisposable);
		}

		static void Main(string[] args) {
			Test.Init();

			// Stores string as UTF8
			StringUTF8 UTF8 = "На берегу пустынных волн Стоял он, дум великих полн, И вдаль глядел.";

			// Stores string as ASCII
			StringASCII ASCII = "Hello ASCII World!";

			// Converts from ASCII to UTF8 because all the characters map
			StringUTF8 UTF8_2 = ASCII;

			// Does not work
			StringASCII ASCII_2 = UTF8_2;

			// Get a "const char*" equivalent of supplied string
			StringPtr Ptr = UTF8;

			// Ditto
			Ptr = ASCII;

			// Does not convert anything, is equivalent of a static cast (works both ways)
			StringASCII ASCII_3 = (StringPtr)UTF8;

			// Call native P/Invoke method with an UTF8 and ASCII string
			// All native functions should have the signature "MethodName(StringPtr String)"
			Test.PrintString(UTF8);
			Test.PrintString(ASCII);

			// Call native P/Invoke method but explicitly encode compatible strings to UTF8
			Test.PrintString((StringUTF8)"Hello UTF8 World!");
			Test.PrintString((StringUTF8)ASCII);

			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}

	static class Test {
		[DllImport(nameof(Test))]
		public static extern void Init();

		[DllImport(nameof(Test))]
		public static extern StringPtr PrintString(StringPtr Str);
	}
}
