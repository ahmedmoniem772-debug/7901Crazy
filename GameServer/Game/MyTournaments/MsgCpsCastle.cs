using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusX.Client;
namespace VirusX.Game.MsgTournaments
{
  public  class MsgCpsCastle
    {
      public  ProcesType Process { get; set; }

      public DateTime StartTimer = new DateTime();

      public MsgCpsCastle()
      {
          Process = ProcesType.Dead;
      }
      public void Start()
      {
          if (Process == ProcesType.Dead)
          {
              StartTimer= DateTime.Now;
              Process = ProcesType.Alive;
          }
      }
      public void Finish()
      {
         
          if (Process == ProcesType.Alive)
          {
              MsgSchedules.SendSysMesage("CP Castle has finished.", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.yellow);
              
              Process = ProcesType.Dead;
                var AllOut = Pool.GamePoll.Values.Where(p => p.Player.Map == 10133 || p.Player.Map == 10134).ToArray();
                foreach (var Client in AllOut)
                    Client.Teleport(438, 418, 1002);
          }
      }
      public void CheckUp()
      {
         if (Process == ProcesType.Alive)
          {
              DateTime Now= DateTime.Now;
              if (Now > StartTimer.AddMinutes(5))
                  Finish();
           
          }
      }
    }
}
