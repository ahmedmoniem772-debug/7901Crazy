// Decompiled with JetBrains decompiler
// Type: NetFwTypeLib.INetFwRules
// Assembly: Interop.NetFwTypeLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBB67B90-D917-4D96-BB2B-F437E1DE660F
// Assembly location: C:\Users\Administrator\source\repos\ConsoleApp1\ConsoleApp1\bin\Debug\Interop.NetFwTypeLib.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace NetFwTypeLib
{
  [Guid("9C4C6277-5027-441E-AFAE-CA1F542DA009")]
  [TypeLibType(4160)]
  [ComImport]
  public interface INetFwRules : IEnumerable
  {
    [DispId(1)]
    int Count { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Add([MarshalAs(UnmanagedType.Interface), In] INetFwRule rule);

    [DispId(3)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Remove([MarshalAs(UnmanagedType.BStr), In] string Name);

    [DispId(4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    INetFwRule Item([MarshalAs(UnmanagedType.BStr), In] string Name);


    new IEnumerator GetEnumerator();
  }
}
