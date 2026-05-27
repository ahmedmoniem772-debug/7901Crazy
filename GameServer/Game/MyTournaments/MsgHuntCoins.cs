using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConquerOnline.Game.MsgTournaments
{
  public  class MsgHuntCoins
    {

        public int GoldRemain = 0;
        public int SilverRemain = 0;
        public int CopperRemain = 0;


      public  ProcesType Process { get; set; }

      public DateTime StartTimer = new DateTime();

      public MsgHuntCoins()
      {
          Process = ProcesType.Dead;
      }
      public void Start()
      {
          if (Process == ProcesType.Dead)
          {
              MsgSchedules.SendInvitation("HuntCoins", "Change Prize From Npc Shop HuntCoins", 374, 451, 1002, 0, 60);
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
              MsgSchedules.SendSysMesage("The Quest HuntCoins has finished.", MsgServer.MsgMessage.ChatMode.System, MsgServer.MsgMessage.MsgColor.yellow);
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
