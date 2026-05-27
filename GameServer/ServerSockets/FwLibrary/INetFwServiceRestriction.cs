// Decompiled with JetBrains decompiler
// Type: NetFwTypeLib.INetFwServiceRestriction
// Assembly: Interop.NetFwTypeLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBB67B90-D917-4D96-BB2B-F437E1DE660F
// Assembly location: C:\Users\Administrator\source\repos\ConsoleApp1\ConsoleApp1\bin\Debug\Interop.NetFwTypeLib.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib
{
  [Guid("8267BBE3-F890-491C-B7B6-2DB1EF0E5D2B")]
  [TypeLibType(4160)]
  [ComImport]
  public interface INetFwServiceRestriction
  {
    [DispId(1)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RestrictService(
      [MarshalAs(UnmanagedType.BStr), In] string serviceName,
      [MarshalAs(UnmanagedType.BStr), In] string appName,
      [In] bool RestrictService,
      [In] bool serviceSidRestricted);

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool ServiceRestricted([MarshalAs(UnmanagedType.BStr), In] string serviceName, [MarshalAs(UnmanagedType.BStr), In] string appName);

    [DispId(3)]
    INetFwRules Rules { [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
  }
}
