using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class string_utf8 {

	public string_utf8(string Str) {

	}

	public static implicit operator string_utf8(string Str) {
		return new string_utf8(Str);
	}
}
