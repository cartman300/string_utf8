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

			StringUTF8 Str1 = "На берегу пустынных волн Стоял он, дум великих полн, И вдаль глядел.";



			using (StringUTF8.Disposable Str2 = Test.PrintString(Str1)) {


				Console.WriteLine("Returned: {0}", Str2);
			}

			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}

	static class Test {
		[DllImport(nameof(Test))]
		public static extern void Init();

		[DllImport(nameof(Test))]
		public static extern StringUTF8 PrintString(StringUTF8 Str);
	}
}
