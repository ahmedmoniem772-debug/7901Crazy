using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirusX.Game.MsgTournaments
{
  public  class MsgSquidWardOctopus
    {

        public int GoldRemain = 0;
        public int SilverRemain = 0;
        public int CopperRemain = 0;


      public  ProcesType Process { get; set; }

      public DateTime StartTimer = new DateTime();

      public MsgSquidWardOctopus()
      {
          Process = ProcesType.Dead;
      }
      public void Start()
      {
          if (Process == ProcesType.Dead)
          {
              MsgSchedules.SendInvitation("SquidWardOctopus", " Prize Random Cps", 338, 283, 1002, 0, 60);
              StartTimer= DateTime.Now;
              GoldRemain = 100;
              SilverRemain = 200;
              CopperRemain = 300;

              Process = ProcesType.Alive;
          }
      }
      public void Finish()
      {
          if (Process == ProcesType.Alive)
          {
              MsgSchedules.SendSysMesage("The Quest SquidWardOctopus has finished.", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.yellow);
              Process = ProcesType.Dead;
          }
      }
      public void CheckUp()
      {
         if (Process == ProcesType.Alive)
          {
              DateTime Now= DateTime.Now;
              if (Now > StartTimer.AddMinutes(15))
                  Finish();
            
           
          }
      }
      
    }
}
