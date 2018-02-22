using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct StringPtr {
	public IntPtr Pointer;

	public int Strlen() {
		int Len = 0;
		while (Marshal.ReadByte(Pointer, Len) != 0)
			Len++;
		return Len;
	}

	public static implicit operator StringPtr(IntPtr Ptr) {
		return new StringPtr() { Pointer = Ptr };
	}

	public static implicit operator IntPtr(StringPtr Ptr) {
		return Ptr.Pointer;
	}

	public static implicit operator StringUTF8(StringPtr Ptr) {
		return new StringUTF8(Ptr.Pointer, Ptr.Strlen());
	}

	public static implicit operator StringASCII(StringPtr Ptr) {
		return new StringASCII(Ptr.Pointer, Ptr.Strlen());
	}
}

public unsafe class StringUTF8 {
	IntPtr Bytes;
	public int Length { get; private set; }

	public StringUTF8(string Str) {
		byte[] SafeBytes = Encoding.UTF8.GetBytes(Str);
		Bytes = Marshal.AllocHGlobal(SafeBytes.Length);
		Marshal.Copy(SafeBytes, 0, Bytes, SafeBytes.Length);
		Length = SafeBytes.Length;
	}

	public StringUTF8(IntPtr Bytes, int Length) {
		this.Bytes = Marshal.AllocHGlobal(Length);
		for (int i = 0; i < Length; i++)
			Marshal.WriteByte(this.Bytes, i, Marshal.ReadByte(Bytes, i));

		this.Length = Length;
	}

	~StringUTF8() {
		Marshal.FreeHGlobal(Bytes);
	}

	public override string ToString() {
		byte[] SafeBytes = new byte[Length];
		for (int i = 0; i < SafeBytes.Length; i++)
			SafeBytes[i] = Marshal.ReadByte(Bytes, i);

		return Encoding.UTF8.GetString(SafeBytes);
	}

	public static implicit operator StringUTF8(string Str) {
		return new StringUTF8(Str);
	}

	public static implicit operator string(StringUTF8 Str) {
		return Str.ToString();
	}

	public static implicit operator StringPtr(StringUTF8 Str) {
		return new StringPtr() { Pointer = Str.Bytes };
	}

	public static implicit operator IntPtr(StringUTF8 Str) {
		return (StringPtr)Str;
	}
}


public unsafe class StringASCII {
	IntPtr Bytes;
	public int Length { get; private set; }

	public StringASCII(string Str) {
		byte[] SafeBytes = Encoding.ASCII.GetBytes(Str);
		Bytes = Marshal.AllocHGlobal(SafeBytes.Length);
		Marshal.Copy(SafeBytes, 0, Bytes, SafeBytes.Length);
		Length = SafeBytes.Length;
	}

	public StringASCII(IntPtr Bytes, int Length) {
		this.Bytes = Marshal.AllocHGlobal(Length);
		for (int i = 0; i < Length; i++)
			Marshal.WriteByte(this.Bytes, i, Marshal.ReadByte(Bytes, i));

		this.Length = Length;
	}

	~StringASCII() {
		Marshal.FreeHGlobal(Bytes);
	}

	public override string ToString() {
		byte[] SafeBytes = new byte[Length];
		for (int i = 0; i < SafeBytes.Length; i++)
			SafeBytes[i] = Marshal.ReadByte(Bytes, i);

		return Encoding.ASCII.GetString(SafeBytes);
	}

	public static implicit operator StringASCII(string Str) {
		return new StringASCII(Str);
	}

	public static implicit operator string(StringASCII Str) {
		return Str.ToString();
	}

	public static implicit operator StringPtr(StringASCII Str) {
		return new StringPtr() { Pointer = Str.Bytes };
	}

	public static implicit operator IntPtr(StringASCII Str) {
		return (StringPtr)Str;
	}

	public static implicit operator StringUTF8(StringASCII Str) {
		return Str.ToString();
	}
}