// Decompiled with JetBrains decompiler
// Type: NetFwTypeLib.INetFwProducts
// Assembly: Interop.NetFwTypeLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBB67B90-D917-4D96-BB2B-F437E1DE660F
// Assembly location: C:\Users\Administrator\source\repos\ConsoleApp1\ConsoleApp1\bin\Debug\Interop.NetFwTypeLib.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace NetFwTypeLib
{
  [Guid("39EB36E0-2097-40BD-8AF2-63A13B525362")]
  [TypeLibType(4160)]
  [ComImport]
  public interface INetFwProducts : IEnumerable
  {
    [DispId(1)]
    int Count { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.IUnknown)]
    object Register([MarshalAs(UnmanagedType.Interface), In] INetFwProduct product);

    [DispId(3)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    INetFwProduct Item([In] int index);


    new IEnumerator GetEnumerator();
  }
}
