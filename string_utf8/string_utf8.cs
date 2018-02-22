using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public unsafe struct StringUTF8 {
	[FieldOffset(0)]
	IntPtr Bytes;

	public class Disposable : IDisposable {
		IntPtr DataBuffer;
		int Length;
		bool Disposed = false;

		public Disposable(IntPtr DataBuffer, int Length) {
			this.DataBuffer = DataBuffer;
			this.Length = Length;
		}

		public void Dispose() {
			if (Disposed)
				return;
			Disposed = true;

			GCHandle SelfGCHandle = GCHandle.FromIntPtr(Marshal.ReadIntPtr(DataBuffer));
			SelfGCHandle.Free();

			Marshal.WriteIntPtr(DataBuffer, 0, IntPtr.Zero);
			Marshal.WriteIntPtr(DataBuffer, IntPtr.Size, IntPtr.Zero);
			Marshal.FreeHGlobal(DataBuffer);
		}

		public override string ToString() {
			return this;
		}

		public static implicit operator string(Disposable Disp) {
			return ReadFromIntPtr(Disp.DataBuffer + IntPtr.Size);
		}
	}

	public StringUTF8(string Str) {
		// Create the UTF8 encoded string
		byte[] UTF8Bytes = Encoding.UTF8.GetBytes(Str);

		// Allocate unmanaged data buffer and copy string into it
		int DataBufferSize = UTF8Bytes.Length + IntPtr.Size + 1;
		IntPtr DataBuffer = Marshal.AllocHGlobal(DataBufferSize);
		Bytes = DataBuffer + IntPtr.Size;
		Marshal.Copy(UTF8Bytes, 0, Bytes, UTF8Bytes.Length);
		Marshal.WriteByte(DataBuffer, DataBufferSize - 1, 0);

		// Allocate the disposable which holds a reference to itself
		Disposable Data = new Disposable(DataBuffer, DataBufferSize);
		Marshal.WriteIntPtr(DataBuffer, 0, GCHandle.ToIntPtr(GCHandle.Alloc(Data)));
	}

	public static implicit operator Disposable(StringUTF8 Str) {
		try {
			IntPtr MetadataPtr = Marshal.ReadIntPtr(Str.Bytes - IntPtr.Size);
			if (MetadataPtr == IntPtr.Zero)
				return null;

			GCHandle H = GCHandle.FromIntPtr(MetadataPtr);
			return H.Target as Disposable;
		} catch {
		}

		return null;
	}

	public static implicit operator StringUTF8(string Str) {
		return new StringUTF8(Str);
	}

	public static implicit operator IntPtr(StringUTF8 Str) {
		return Str.Bytes;
	}

	public static implicit operator string(StringUTF8 Str) {
		return ReadFromIntPtr(Str.Bytes);
	}

	public override string ToString() {
		return this;
	}

	public static string ReadFromIntPtr(IntPtr Ptr) {
		int Len = 0;
		while (Marshal.ReadByte(Ptr, Len) != 0)
			Len++;

		return Encoding.UTF8.GetString((byte*)Ptr, Len);
	}
}
