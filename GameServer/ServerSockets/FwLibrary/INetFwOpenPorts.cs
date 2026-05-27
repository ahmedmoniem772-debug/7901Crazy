// Decompiled with JetBrains decompiler
// Type: NetFwTypeLib.INetFwOpenPorts
// Assembly: Interop.NetFwTypeLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBB67B90-D917-4D96-BB2B-F437E1DE660F
// Assembly location: C:\Users\Administrator\source\repos\ConsoleApp1\ConsoleApp1\bin\Debug\Interop.NetFwTypeLib.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace NetFwTypeLib
{
  [Guid("C0E9D7FA-E07E-430A-B19A-090CE82D92E2")]
  [TypeLibType(4160)]
  [ComImport]
  public interface INetFwOpenPorts : IEnumerable
  {
    [DispId(1)]
    int Count { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Add([MarshalAs(UnmanagedType.Interface), In] INetFwOpenPort Port);

    [DispId(3)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Remove([In] int portNumber, [In] NET_FW_IP_PROTOCOL_ ipProtocol);

    [DispId(4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    INetFwOpenPort Item([In] int portNumber, [In] NET_FW_IP_PROTOCOL_ ipProtocol);

   
    new IEnumerator GetEnumerator();
  }
}
