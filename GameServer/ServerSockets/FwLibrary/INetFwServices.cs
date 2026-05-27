// Decompiled with JetBrains decompiler
// Type: NetFwTypeLib.INetFwServices
// Assembly: Interop.NetFwTypeLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBB67B90-D917-4D96-BB2B-F437E1DE660F
// Assembly location: C:\Users\Administrator\source\repos\ConsoleApp1\ConsoleApp1\bin\Debug\Interop.NetFwTypeLib.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace NetFwTypeLib
{
  [Guid("79649BB4-903E-421B-94C9-79848E79F6EE")]
  [TypeLibType(4160)]
  [ComImport]
  public interface INetFwServices : IEnumerable
  {
    [DispId(1)]
    int Count { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    INetFwService Item([In] NET_FW_SERVICE_TYPE_ svcType);


    new IEnumerator GetEnumerator();
  }
}
