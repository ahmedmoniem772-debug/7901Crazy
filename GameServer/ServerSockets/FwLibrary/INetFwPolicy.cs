// Decompiled with JetBrains decompiler
// Type: NetFwTypeLib.INetFwPolicy
// Assembly: Interop.NetFwTypeLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBB67B90-D917-4D96-BB2B-F437E1DE660F
// Assembly location: C:\Users\Administrator\source\repos\ConsoleApp1\ConsoleApp1\bin\Debug\Interop.NetFwTypeLib.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib
{
  [Guid("D46D2478-9AC9-4008-9DC7-5563CE5536CC")]
  [TypeLibType(4160)]
  [ComImport]
  public interface INetFwPolicy
  {
    [DispId(1)]
    INetFwProfile CurrentProfile { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    INetFwProfile GetProfileByType([In] NET_FW_PROFILE_TYPE_ profileType);
  }
}
