// Decompiled with JetBrains decompiler
// Type: NetFwTypeLib.INetFwAuthorizedApplications
// Assembly: Interop.NetFwTypeLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBB67B90-D917-4D96-BB2B-F437E1DE660F
// Assembly location: C:\Users\Administrator\source\repos\ConsoleApp1\ConsoleApp1\bin\Debug\Interop.NetFwTypeLib.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace NetFwTypeLib
{
  [Guid("644EFD52-CCF9-486C-97A2-39F352570B30")]
  [TypeLibType(4160)]
  [ComImport]
  public interface INetFwAuthorizedApplications : IEnumerable
  {
    [DispId(1)]
    int Count { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Add([MarshalAs(UnmanagedType.Interface), In] INetFwAuthorizedApplication app);

    [DispId(3)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Remove([MarshalAs(UnmanagedType.BStr), In] string imageFileName);

    [DispId(4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    INetFwAuthorizedApplication Item([MarshalAs(UnmanagedType.BStr), In] string imageFileName);

    new IEnumerator GetEnumerator();
  }
}
