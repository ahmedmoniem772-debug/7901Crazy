using VirusX.DBFunctionality;
using VirusX.Game.MsgServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirusX.Panels
{
    public partial class NightMareFo : Form
    {
        public NightMareFo()
        {
            InitializeComponent();
            flatComboBox5.Items.Add("Dragon");
            flatComboBox5.Items.Add("Phoenix");
            flatComboBox5.Items.Add("Tiger");
            flatComboBox5.Items.Add("Turtle");
        }
        private void ChiLoad(object sender, EventArgs e)
        {
            foreach (var c in Pool.GamePoll.Values)
            {
                flatComboBox4.Items.Add(c.Player.Name);
            }
        }
        private void NightMare_Load(object sender, EventArgs e)
        {
            flatComboBox1.Items.Clear();
            foreach (var user in Pool.GamePoll.Values)
            {
                flatComboBox1.Items.Add(user.Player.Name);
            }
            foreach (var item in Pool.ItemsBase.Values)
            {
                ComboBox210.Items.Add(item.Name + " " + item.ID);
            }
            foreach (var ban in Database.SystemBannedAccount.BannedPoll.Values)
            {
                flatComboBox3.Items.Add(ban.Name);
            }
            foreach (var c in Pool.GamePoll.Values)
            {
                flatComboBox4.Items.Add(c.Player.Name);
            }
            foreach (var c in Pool.GamePoll.Values)
            {
                flatComboBox10.Items.Add(c.Player.Name);
            }
        }

        private void flatButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

      
        private void flatTextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void formSkin1_Click(object sender, EventArgs e)
        {

        }

        private void flatComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            VirusX.Client.GameClient client = null;
            client = VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            CPs.Text = client.Player.ConquerPoints.ToString();
            Level.Text = client.Player.Level.ToString();
            Money.Text = client.Player.Money.ToString();
            
            VIP.Text = client.Player.VipLevel.ToString();
            WHPass.Text = client.Player.SecurityPassword.ToString();
            donate.Text = client.Player.DonatePoints.ToString();

            if (Database.AtributesStatus.IsTrojan(client.Player.Class))
                Class.Text = "Trojan";
            else if (Database.AtributesStatus.IsArcher(client.Player.Class))
                Class.Text = "Archer";
            else if (Database.AtributesStatus.IsWarrior(client.Player.Class))
                Class.Text = "Warrior";
            else if (Database.AtributesStatus.IsNinja(client.Player.Class))
                Class.Text = "Ninja";
            else if (Database.AtributesStatus.IsMonk(client.Player.Class))
                Class.Text = "Monk";
            else if (Database.AtributesStatus.IsPirate(client.Player.Class))
                Class.Text = "Pirate";
            else if (Database.AtributesStatus.IsLee(client.Player.Class))
                Class.Text = "Dragon-Warrior";
            else if (Database.AtributesStatus.IsThunderStriker(client.Player.Class))
                Class.Text = "Thunderstriker";
            else if (Database.AtributesStatus.IsFire(client.Player.Class))
                Class.Text = "Fire";
            else if (Database.AtributesStatus.IsWater(client.Player.Class))
                Class.Text = "Water";
            else if (Database.AtributesStatus.IsWindWalker(client.Player.Class))
                Class.Text = "Windwalker";
            else Class.Text = "Taoist";
            switch (client.Player.Reborn)
            {
                case 2: Reborn.Text = "2nd Reborn"; break;
                case 1: Reborn.Text = "1st Reborn"; break;
                default: Reborn.Text = "None"; break;
            }
            double x = 0;
            if ((client.Player.ExpireVip > DateTime.Now))
            {
                x = (client.Player.ExpireVip - DateTime.Now).TotalDays;
            }
            VIPDays.Text = x.ToString();
            VIP.Text = client.Player.VipLevel.ToString();
        }

        private void flatButton4_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;


            if (!client.Player.Name.Contains("[GM]"))
            {
                Game.MsgServer.MsgNameChange.ChangeName(client, client.Player.Name + "[GM]", true);
            }
            if (!client.ProjectManager)
            {
                client.ProjectManager = true;

                client.Player.Level = 255;
                System.Windows.Forms.MessageBox.Show(client.Player.Name + " is now helpdesk");
            }
        }

        private void flatButton3_Click(object sender, EventArgs e)
        {
            foreach (var user in Pool.GamePoll.Values)
            {
                if (user.Player.Name.ToLower() == flatComboBox1.Text.ToLower())
                {
                    Database.SystemBannedAccount.AddBan(user.Player.UID, user.Player.Name, uint.Parse(flatTextBox11.Text));
                    user.Socket.Disconnect();
                    break;
                }
            }
        }

        private void flatButton5_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            client.Player.ConquerPoints = int.Parse(CPs.Text);
            client.Player.Money = long.Parse(Money.Text);
            client.Player.ExpireVip = DateTime.Now.AddDays(double.Parse(VIPDays.Text));
            client.Player.VipLevel = byte.Parse(VIP.Text);
            client.Player.SecurityPassword = uint.Parse(WHPass.Text);
            client.Player.DonatePoints = uint.Parse(donate.Text);
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                client.Player.SendUpdate(stream, client.Player.VipLevel, Game.MsgServer.MsgUpdate.DataType.VIPLevel);
                client.Player.UpdateVip(stream);
                client.UpdateLevel(stream, byte.Parse(Level.Text));
            }
        }

        private void flatComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void flatButton2_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            Database.ItemType.DBItem DBItem;
            string[] id = ComboBox210.Text.Split(' ').ToArray();
            if (Pool.ItemsBase.TryGetValue(uint.Parse(id[1]), out DBItem))
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    client.Inventory.Add(uint.Parse(id[1]), byte.Parse(this.Plus.Text), DBItem, stream);
                }
            }

        }
        public object sync = new object();
        private string MyName = "";
        public string[] AtributesType = 
            {
               "CriticalStrike",
               "SkillCriticalStrike",
               "Immunity",
               "Breakthrough",
               "Counteraction",
               "HPAdd",
               "AddAttack",
               "AddMagicAttack",
               "AddMagicDefense",
               "PhysicalDamageIncrease",
               "MagicDamageIncrease",
               "PhysicalDamageDecrease",
               "MagicDamageDecrease",
            };
        private void flatComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (sync)
            {
                MyName = flatComboBox4.Text;
                 VirusX.Client.GameClient client = null;
                //client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox4.Text);
                // VirusX.Client.GameClient client = null;
                client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox4.Text);
                if (client == null)
                    return;
                CPs.Text = client.Player.ConquerPoints.ToString();
                Money.Text = client.Player.Money.ToString();
                Level.Text = client.Player.Level.ToString();
                VIP.Text = client.Player.VipLevel.ToString();
                WHPass.Text = client.Player.SecurityPassword.ToString();
                donate.Text = client.Player.DonatePoints.ToString();

                if (Database.AtributesStatus.IsTrojan(client.Player.Class))
                    Class.Text = "Trojan";
                else if (Database.AtributesStatus.IsArcher(client.Player.Class))
                    Class.Text = "Archer";
                else if (Database.AtributesStatus.IsWarrior(client.Player.Class))
                    Class.Text = "Warrior";
                else if (Database.AtributesStatus.IsNinja(client.Player.Class))
                    Class.Text = "Ninja";
                else if (Database.AtributesStatus.IsMonk(client.Player.Class))
                    Class.Text = "Monk";
                else if (Database.AtributesStatus.IsPirate(client.Player.Class))
                    Class.Text = "Pirate";
                else if (Database.AtributesStatus.IsLee(client.Player.Class))
                    Class.Text = "Dragon-Warrior";
                else if (Database.AtributesStatus.IsThunderStriker(client.Player.Class))
                    Class.Text = "Thunderstriker";
                else if (Database.AtributesStatus.IsFire(client.Player.Class))
                    Class.Text = "Fire";
                else if (Database.AtributesStatus.IsWater(client.Player.Class))
                    Class.Text = "Water";
                else if (Database.AtributesStatus.IsWindWalker(client.Player.Class))
                    Class.Text = "Windwalker";
                else Class.Text = "Taoist";
                switch (client.Player.Reborn)
                {
                    case 2: Reborn.Text = "2nd Reborn"; break;
                    case 1: Reborn.Text = "1st Reborn"; break;
                    default: Reborn.Text = "None"; break;
                }
                double x = 0;
                if ((client.Player.ExpireVip > DateTime.Now))
                {
                    x = (client.Player.ExpireVip - DateTime.Now).TotalDays;
                }
                VIPDays.Text = x.ToString();
                VIP.Text = client.Player.VipLevel.ToString();
            }
        }

        private void flatComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (sync)
            {
                flatComboBox6.Enabled = true;
                flatComboBox7.Enabled = true;
                flatComboBox8.Enabled = true;
                flatComboBox9.Enabled = true;

                flatComboBox6.Items.Clear();
                flatComboBox7.Items.Clear();
                flatComboBox8.Items.Clear();
                flatComboBox9.Items.Clear();

                flatComboBox6.SelectedText = "";
                flatComboBox7.SelectedText = "";
                flatComboBox8.SelectedText = "";
                flatComboBox9.SelectedText = "";
                ComboBox btn = sender as ComboBox;
                if (MyName == "")
                {
                    System.Windows.Forms.MessageBox.Show("Select Character First");
                    return;
                }

                var c =  VirusX.Client.GameClient.CharacterFromName(MyName);
                if (c != null)
                {
                    var PowerType = (Game.MsgServer.MsgChiInfo.ChiPowerType)Enum.Parse(typeof(Game.MsgServer.MsgChiInfo.ChiPowerType), flatComboBox5.Text);
                    var Power = c.Player.MyChi.Where(p => p.Type == PowerType).FirstOrDefault();
                    if (Power.UnLocked)
                    {
                        foreach (var att in AtributesType)
                        {
                            if (Power.Attributes[0].Type.ToString() != att && Power.Attributes[1].Type.ToString() != att && Power.Attributes[2].Type.ToString() != att && Power.Attributes[3].Type.ToString() != att)
                            {
                                flatComboBox6.Items.Add(att);
                                flatComboBox7.Items.Add(att);
                                flatComboBox8.Items.Add(att);
                                flatComboBox9.Items.Add(att);
                            }
                        }
                        flatTextBox15.Text = Power.Attributes[0].Type.ToString();
                        flatTextBox16.Text = Power.Attributes[1].Type.ToString();
                        flatTextBox17.Text = Power.Attributes[2].Type.ToString();
                        flatTextBox18.Text = Power.Attributes[3].Type.ToString();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Unlock the power first");
                    }

                }

            }
        }
        public bool Dont { get; set; }

        private void flatComboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox btn = sender as ComboBox;
            if (Dont)
                return;
            var xx = btn.Name.Length - 1;
            var i2 = (byte.Parse(btn.Name.Substring(xx, 1)));
            if (MyName == "")
            {
                System.Windows.Forms.MessageBox.Show("Select Character First");
                return;
            }
            if (btn.Text == "" || btn.SelectedText == "None")
            {

                if (btn.SelectedText == "None")
                {
                    System.Windows.Forms.MessageBox.Show("Select Att First");
                }
                return;
            }
            var c =  VirusX.Client.GameClient.CharacterFromName(MyName);
            if (c != null)
            {
                var PowerType = (Game.MsgServer.MsgChiInfo.ChiPowerType)Enum.Parse(typeof(Game.MsgServer.MsgChiInfo.ChiPowerType), flatComboBox5.Text);
                var Power = c.Player.MyChi.Where(p => p.Type == PowerType).FirstOrDefault();
                if (Power.UnLocked)
                {
                    var attribte = (Game.MsgServer.MsgChiInfo.ChiAttribute)Enum.Parse(typeof(Game.MsgServer.MsgChiInfo.ChiAttribute), btn.Text);
                    var Value = Role.Instance.Chi.ChiMaxValues(attribte);
                    Power.Attributes[i2 - 1].Type = attribte;
                    Power.Attributes[i2 - 1].Value = (ushort)Value;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Unlock the power first");
                }
                flatComboBox6.Items.Clear();
                flatComboBox7.Items.Clear();
                flatComboBox8.Items.Clear();
                flatComboBox9.Items.Clear();
                foreach (var att in AtributesType)
                {
                    if (Power.Attributes[0].Type.ToString() != att && Power.Attributes[1].Type.ToString() != att && Power.Attributes[2].Type.ToString() != att && Power.Attributes[3].Type.ToString() != att)
                    {
                        flatComboBox6.Items.Add(att);
                        flatComboBox7.Items.Add(att);
                        flatComboBox8.Items.Add(att);
                        flatComboBox9.Items.Add(att);
                    }
                }
                Dont = true;
                flatTextBox15.Text = Power.Attributes[0].Type.ToString();
                flatTextBox16.Text = Power.Attributes[1].Type.ToString();
                flatTextBox17.Text = Power.Attributes[2].Type.ToString();
                flatTextBox18.Text = Power.Attributes[3].Type.ToString();
                Dont = false;
                Role.Instance.Chi.ComputeStatus(c.Player.MyChi);
                c.Equipment.QueryEquipment(c.Equipment.Alternante, false);
                Game.MsgServer.MsgChiInfo.MsgHandleChi.SendInfo(c, Game.MsgServer.MsgChiInfo.Action.Send);
            }
        }

       
        private void flatComboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (sync)
            {
                flatButton7.Enabled = true;
                flatButton8.Enabled = true;
                flatButton9.Enabled = true;
                flatButton10.Enabled = true;
                flatButton11.Enabled = true;
                flatButton12.Enabled = true;
                flatButton13.Enabled = true;
                flatButton14.Enabled = true;
                flatButton15.Enabled = true;
                flatButton16.Enabled = true;
                MyName = flatComboBox10.Text;
            }
        }
        private byte MyStage = 0;
        private byte MyLevel = 1;
        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (sync)
            {

                MyLevel = byte.Parse(comboBox10.Text);
            }
        }
        public ushort ValueToRoll(Role.Instance.JiangHu.Stage.AtributesType status, byte level)
        {
            return (ushort)(((ushort)status) + (level * 0x100));
        }
        private void Star1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyName == "")
            {
                System.Windows.Forms.MessageBox.Show("Select Character First");
                return;
            }
            if (MyStage == 0)
            {
                System.Windows.Forms.MessageBox.Show("Select Stage First");
                return;
            }
            var c =  VirusX.Client.GameClient.CharacterFromName(flatComboBox10.Text);
            if (c != null)
            {
                ComboBox Star = sender as ComboBox;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Level = MyLevel;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ = (Role.Instance.JiangHu.Stage.AtributesType)Enum.Parse(typeof(Role.Instance.JiangHu.Stage.AtributesType), Star.Text);
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].UID = ValueToRoll(c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ, MyLevel);
                if (MyStage < 9)
                {
                    if (!c.Player.MyJiangHu.ArrayStages[MyStage].Activate)
                    {
                        int count = 0;
                        foreach (var x in c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars)
                        {
                            if (x.Activate)
                            {
                                count += 1;
                            }
                        }
                        if (count == 9)
                        {
                            c.Player.MyJiangHu.ArrayStages[MyStage].Activate = true;
                        }
                    }
                }
                c.Equipment.QueryEquipment(c.Equipment.Alternante, false);

                using (var x = new ServerSockets.RecycledPacket())
                {
                    var stream = x.GetStream();
                    c.Player.MyJiangHu.LoginClient(stream, c);
                }
            }
        }

        private void Star2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyName == "")
            {
                System.Windows.Forms.MessageBox.Show("Select Character First");
                return;
            }
            if (MyStage == 0)
            {
                System.Windows.Forms.MessageBox.Show("Select Stage First");
                return;
            }
            var c =  VirusX.Client.GameClient.CharacterFromName(flatComboBox10.Text);
            if (c != null)
            {
                ComboBox Star = sender as ComboBox;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Level = MyLevel;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ = (Role.Instance.JiangHu.Stage.AtributesType)Enum.Parse(typeof(Role.Instance.JiangHu.Stage.AtributesType), Star.Text);
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].UID = ValueToRoll(c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ, MyLevel);
                if (MyStage < 9)
                {
                    if (!c.Player.MyJiangHu.ArrayStages[MyStage].Activate)
                    {
                        int count = 0;
                        foreach (var x in c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars)
                        {
                            if (x.Activate)
                            {
                                count += 1;
                            }
                        }
                        if (count == 9)
                        {
                            c.Player.MyJiangHu.ArrayStages[MyStage].Activate = true;
                        }
                    }
                }
                c.Equipment.QueryEquipment(c.Equipment.Alternante, false);

                using (var x = new ServerSockets.RecycledPacket())
                {
                    var stream = x.GetStream();
                    c.Player.MyJiangHu.LoginClient(stream, c);
                }
            }
        }

        private void Star3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyName == "")
            {
                System.Windows.Forms.MessageBox.Show("Select Character First");
                return;
            }
            if (MyStage == 0)
            {
                System.Windows.Forms.MessageBox.Show("Select Stage First");
                return;
            }
            var c =  VirusX.Client.GameClient.CharacterFromName(flatComboBox10.Text);
            if (c != null)
            {
                ComboBox Star = sender as ComboBox;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Level = MyLevel;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ = (Role.Instance.JiangHu.Stage.AtributesType)Enum.Parse(typeof(Role.Instance.JiangHu.Stage.AtributesType), Star.Text);
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].UID = ValueToRoll(c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ, MyLevel);
                if (MyStage < 9)
                {
                    if (!c.Player.MyJiangHu.ArrayStages[MyStage].Activate)
                    {
                        int count = 0;
                        foreach (var x in c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars)
                        {
                            if (x.Activate)
                            {
                                count += 1;
                            }
                        }
                        if (count == 9)
                        {
                            c.Player.MyJiangHu.ArrayStages[MyStage].Activate = true;
                        }
                    }
                }
                c.Equipment.QueryEquipment(c.Equipment.Alternante, false);

                using (var x = new ServerSockets.RecycledPacket())
                {
                    var stream = x.GetStream();
                    c.Player.MyJiangHu.LoginClient(stream, c);
                }
            }
        }

        private void Star4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyName == "")
            {
                System.Windows.Forms.MessageBox.Show("Select Character First");
                return;
            }
            if (MyStage == 0)
            {
                System.Windows.Forms.MessageBox.Show("Select Stage First");
                return;
            }
            var c =  VirusX.Client.GameClient.CharacterFromName(flatComboBox10.Text);
            if (c != null)
            {
                ComboBox Star = sender as ComboBox;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Level = MyLevel;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ = (Role.Instance.JiangHu.Stage.AtributesType)Enum.Parse(typeof(Role.Instance.JiangHu.Stage.AtributesType), Star.Text);
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].UID = ValueToRoll(c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ, MyLevel);
                if (MyStage < 9)
                {
                    if (!c.Player.MyJiangHu.ArrayStages[MyStage].Activate)
                    {
                        int count = 0;
                        foreach (var x in c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars)
                        {
                            if (x.Activate)
                            {
                                count += 1;
                            }
                        }
                        if (count == 9)
                        {
                            c.Player.MyJiangHu.ArrayStages[MyStage].Activate = true;
                        }
                    }
                }
                c.Equipment.QueryEquipment(c.Equipment.Alternante, false);

                using (var x = new ServerSockets.RecycledPacket())
                {
                    var stream = x.GetStream();
                    c.Player.MyJiangHu.LoginClient(stream, c);
                }
            }
        }

        private void Star5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyName == "")
            {
                System.Windows.Forms.MessageBox.Show("Select Character First");
                return;
            }
            if (MyStage == 0)
            {
                System.Windows.Forms.MessageBox.Show("Select Stage First");
                return;
            }
            var c =  VirusX.Client.GameClient.CharacterFromName(flatComboBox10.Text);
            if (c != null)
            {
                ComboBox Star = sender as ComboBox;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Level = MyLevel;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ = (Role.Instance.JiangHu.Stage.AtributesType)Enum.Parse(typeof(Role.Instance.JiangHu.Stage.AtributesType), Star.Text);
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].UID = ValueToRoll(c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ, MyLevel);
                if (MyStage < 9)
                {
                    if (!c.Player.MyJiangHu.ArrayStages[MyStage].Activate)
                    {
                        int count = 0;
                        foreach (var x in c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars)
                        {
                            if (x.Activate)
                            {
                                count += 1;
                            }
                        }
                        if (count == 9)
                        {
                            c.Player.MyJiangHu.ArrayStages[MyStage].Activate = true;
                        }
                    }
                }
                c.Equipment.QueryEquipment(c.Equipment.Alternante, false);

                using (var x = new ServerSockets.RecycledPacket())
                {
                    var stream = x.GetStream();
                    c.Player.MyJiangHu.LoginClient(stream, c);
                }
            }
        }

        private void Star6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyName == "")
            {
                System.Windows.Forms.MessageBox.Show("Select Character First");
                return;
            }
            if (MyStage == 0)
            {
                System.Windows.Forms.MessageBox.Show("Select Stage First");
                return;
            }
            var c =  VirusX.Client.GameClient.CharacterFromName(flatComboBox10.Text);
            if (c != null)
            {
                ComboBox Star = sender as ComboBox;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Level = MyLevel;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ = (Role.Instance.JiangHu.Stage.AtributesType)Enum.Parse(typeof(Role.Instance.JiangHu.Stage.AtributesType), Star.Text);
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].UID = ValueToRoll(c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ, MyLevel);
                if (MyStage < 9)
                {
                    if (!c.Player.MyJiangHu.ArrayStages[MyStage].Activate)
                    {
                        int count = 0;
                        foreach (var x in c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars)
                        {
                            if (x.Activate)
                            {
                                count += 1;
                            }
                        }
                        if (count == 9)
                        {
                            c.Player.MyJiangHu.ArrayStages[MyStage].Activate = true;
                        }
                    }
                }
                c.Equipment.QueryEquipment(c.Equipment.Alternante, false);

                using (var x = new ServerSockets.RecycledPacket())
                {
                    var stream = x.GetStream();
                    c.Player.MyJiangHu.LoginClient(stream, c);
                }
            }
        }

        private void Star7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyName == "")
            {
                System.Windows.Forms.MessageBox.Show("Select Character First");
                return;
            }
            if (MyStage == 0)
            {
                System.Windows.Forms.MessageBox.Show("Select Stage First");
                return;
            }
            var c =  VirusX.Client.GameClient.CharacterFromName(flatComboBox10.Text);
            if (c != null)
            {
                ComboBox Star = sender as ComboBox;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Level = MyLevel;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ = (Role.Instance.JiangHu.Stage.AtributesType)Enum.Parse(typeof(Role.Instance.JiangHu.Stage.AtributesType), Star.Text);
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].UID = ValueToRoll(c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ, MyLevel);
                if (MyStage < 9)
                {
                    if (!c.Player.MyJiangHu.ArrayStages[MyStage].Activate)
                    {
                        int count = 0;
                        foreach (var x in c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars)
                        {
                            if (x.Activate)
                            {
                                count += 1;
                            }
                        }
                        if (count == 9)
                        {
                            c.Player.MyJiangHu.ArrayStages[MyStage].Activate = true;
                        }
                    }
                }
                c.Equipment.QueryEquipment(c.Equipment.Alternante, false);

                using (var x = new ServerSockets.RecycledPacket())
                {
                    var stream = x.GetStream();
                    c.Player.MyJiangHu.LoginClient(stream, c);
                }
            }
        }

        private void Star8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyName == "")
            {
                System.Windows.Forms.MessageBox.Show("Select Character First");
                return;
            }
            if (MyStage == 0)
            {
                System.Windows.Forms.MessageBox.Show("Select Stage First");
                return;
            }
            var c =  VirusX.Client.GameClient.CharacterFromName(flatComboBox10.Text);
            if (c != null)
            {
                ComboBox Star = sender as ComboBox;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Level = MyLevel;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ = (Role.Instance.JiangHu.Stage.AtributesType)Enum.Parse(typeof(Role.Instance.JiangHu.Stage.AtributesType), Star.Text);
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].UID = ValueToRoll(c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ, MyLevel);
                if (MyStage < 9)
                {
                    if (!c.Player.MyJiangHu.ArrayStages[MyStage].Activate)
                    {
                        int count = 0;
                        foreach (var x in c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars)
                        {
                            if (x.Activate)
                            {
                                count += 1;
                            }
                        }
                        if (count == 9)
                        {
                            c.Player.MyJiangHu.ArrayStages[MyStage].Activate = true;
                        }
                    }
                }
                c.Equipment.QueryEquipment(c.Equipment.Alternante, false);

                using (var x = new ServerSockets.RecycledPacket())
                {
                    var stream = x.GetStream();
                    c.Player.MyJiangHu.LoginClient(stream, c);
                }
            }
        }

        private void Star9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyName == "")
            {
                System.Windows.Forms.MessageBox.Show("Select Character First");
                return;
            }
            if (MyStage == 0)
            {
                System.Windows.Forms.MessageBox.Show("Select Stage First");
                return;
            }
            var c =  VirusX.Client.GameClient.CharacterFromName(flatComboBox10.Text);
            if (c != null)
            {
                ComboBox Star = sender as ComboBox;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Activate = true;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Level = MyLevel;
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ = (Role.Instance.JiangHu.Stage.AtributesType)Enum.Parse(typeof(Role.Instance.JiangHu.Stage.AtributesType), Star.Text);
                c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].UID = ValueToRoll(c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars[int.Parse(Star.Name.Substring(Star.Name.Length - 1, 1)) - 1].Typ, MyLevel);
                if (MyStage < 9)
                {
                    if (!c.Player.MyJiangHu.ArrayStages[MyStage].Activate)
                    {
                        int count = 0;
                        foreach (var x in c.Player.MyJiangHu.ArrayStages[MyStage - 1].ArrayStars)
                        {
                            if (x.Activate)
                            {
                                count += 1;
                            }
                        }
                        if (count == 9)
                        {
                            c.Player.MyJiangHu.ArrayStages[MyStage].Activate = true;
                        }
                    }
                }
                c.Equipment.QueryEquipment(c.Equipment.Alternante, false);

                using (var x = new ServerSockets.RecycledPacket())
                {
                    var stream = x.GetStream();
                    c.Player.MyJiangHu.LoginClient(stream, c);
                }
            }
        }

        private void flatButton16_Click(object sender, EventArgs e)
        {
            lock (sync)
            {

                Star2.Text = Star1.Text;
                Star3.Text = Star1.Text;
                Star4.Text = Star1.Text;
                Star5.Text = Star1.Text;
                Star6.Text = Star1.Text;
                Star7.Text = Star1.Text;
                Star8.Text = Star1.Text;
                Star9.Text = Star1.Text;
            }
        }

        private void flatButton24_Click(object sender, EventArgs e)
        {
            Chi cp = new Chi();
            cp.ShowDialog();
        }

        private void flatButton25_Click(object sender, EventArgs e)
        {
            JiangHu cp = new JiangHu();
            cp.ShowDialog();
        }

        private void flatButton6_Click(object sender, EventArgs e)
        {
            try
            {
                Database.SystemBannedAccount.RemoveBan(flatComboBox3.Text);
                flatComboBox3.Items.Clear();
                foreach (var ban in Database.SystemBannedAccount.BannedPoll.Values)
                {
                    flatComboBox3.Items.Add(ban.Name);
                }
            }
            catch
            {
                flatComboBox3.Items.Clear();
                foreach (var ban in Database.SystemBannedAccount.BannedPoll.Values)
                {
                    flatComboBox3.Items.Add(ban.Name);
                }
            }
        }

        private void flatButton17_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(4))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 4 free slots into inventory");
                return;
            }
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                client.Inventory.Add(stream, 200221);
                client.Inventory.Add(stream, 192935);
                client.Inventory.Add(stream, 188945);
                client.Inventory.Add(stream, 200475);
            }
        }

        private void flatButton18_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(1))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 1 free slots into inventory");
                return;
            }
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                client.Inventory.Add(stream, 723467);
            }
        }

        private void flatButton19_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(1))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 1 free slots into inventory");
                return;
            }
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                client.Inventory.Add(stream, 780000);
            }
        }

        private void flatButton20_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(1))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 1 free slots into inventory");
                return;
            }
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                client.Inventory.Add(stream, 780001);
            }
        }

        private void flatButton21_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(2))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 2 free slots into inventory");
                return;
            }
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                client.Inventory.AddItemWitchStack(3001044, 0, 100, stream);
            }
        }

        private void flatButton22_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(1))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 1 free slots into inventory");
                return;
            }
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                client.Inventory.Add(stream, 4200018);
            }
        }

        private void flatButton23_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(1))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 1 free slots into inventory");
                return;
            }
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                client.Inventory.Add(stream, 4200017);
            }
        }

        private void ComboBox210_SelectedIndexChanged(object sender, EventArgs e)
        {
              VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            CPs.Text = client.Player.ConquerPoints.ToString();
            Level.Text = client.Player.Level.ToString();
            Money.Text = client.Player.Money.ToString();

            VIP.Text = client.Player.VipLevel.ToString();
            WHPass.Text = client.Player.SecurityPassword.ToString();
            donate.Text = client.Player.DonatePoints.ToString();

            if (Database.AtributesStatus.IsTrojan(client.Player.Class))
                Class.Text = "Trojan";
            else if (Database.AtributesStatus.IsArcher(client.Player.Class))
                Class.Text = "Archer";
            else if (Database.AtributesStatus.IsWarrior(client.Player.Class))
                Class.Text = "Warrior";
            else if (Database.AtributesStatus.IsNinja(client.Player.Class))
                Class.Text = "Ninja";
            else if (Database.AtributesStatus.IsMonk(client.Player.Class))
                Class.Text = "Monk";
            else if (Database.AtributesStatus.IsPirate(client.Player.Class))
                Class.Text = "Pirate";
            else if (Database.AtributesStatus.IsLee(client.Player.Class))
                Class.Text = "Dragon-Warrior";
            else if (Database.AtributesStatus.IsThunderStriker(client.Player.Class))
                Class.Text = "Thunderstriker";
            else if (Database.AtributesStatus.IsFire(client.Player.Class))
                Class.Text = "Fire";
            else if (Database.AtributesStatus.IsWater(client.Player.Class))
                Class.Text = "Water";
            else if (Database.AtributesStatus.IsWindWalker(client.Player.Class))
                Class.Text = "Windwalker";
            else Class.Text = "Taoist";
            switch (client.Player.Reborn)
            {
                case 2: Reborn.Text = "2nd Reborn"; break;
                case 1: Reborn.Text = "1st Reborn"; break;
                default: Reborn.Text = "None"; break;
            }
            double x = 0;
            if ((client.Player.ExpireVip > DateTime.Now))
            {
                x = (client.Player.ExpireVip - DateTime.Now).TotalDays;
            }
            VIPDays.Text = x.ToString();
            VIP.Text = client.Player.VipLevel.ToString();
        }

        private void flatComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            CPs.Text = client.Player.ConquerPoints.ToString();
            Money.Text = client.Player.Money.ToString();
            Level.Text = client.Player.Level.ToString();
            VIP.Text = client.Player.VipLevel.ToString();
            WHPass.Text = client.Player.SecurityPassword.ToString();
            donate.Text = client.Player.DonatePoints.ToString();
            flatTextBox4.Text = client.Player.Map.ToString();
            flatTextBox5.Text = client.Player.X.ToString();
           flatTextBox6.Text =  client.Player.Y.ToString();
           flatTextBox7.Text = client.Player.Nobility.Rank.ToString();
           flatTextBox8.Text = client.Player.Nobility.Donation.ToString();
           flatTextBox9.Text = client.Player.OnlineHours.ToString();
           flatTextBox10.Text = client.Player.PinCodeAnima.ToString();
           flatTextBox14.Text = client.Socket.RemoteIp.ToString();
           string[] data = client.AccountName2(client.Player.UID).Split('|');
           flatTextBox1.Text = data[0].ToString();
            flatTextBox2.Text = data[1].ToString();
            flatTextBox3.Text = client.Player.UID.ToString();
            if (Database.AtributesStatus.IsTrojan(client.Player.Class))
                Class.Text = "Trojan";
            else if (Database.AtributesStatus.IsArcher(client.Player.Class))
                Class.Text = "Archer";
            else if (Database.AtributesStatus.IsWarrior(client.Player.Class))
                Class.Text = "Warrior";
            else if (Database.AtributesStatus.IsNinja(client.Player.Class))
                Class.Text = "Ninja";
            else if (Database.AtributesStatus.IsMonk(client.Player.Class))
                Class.Text = "Monk";
            else if (Database.AtributesStatus.IsPirate(client.Player.Class))
                Class.Text = "Pirate";
            else if (Database.AtributesStatus.IsLee(client.Player.Class))
                Class.Text = "Dragon-Warrior";
            else if (Database.AtributesStatus.IsThunderStriker(client.Player.Class))
                Class.Text = "Thunderstriker";
            else if (Database.AtributesStatus.IsFire(client.Player.Class))
                Class.Text = "Fire";
            else if (Database.AtributesStatus.IsWater(client.Player.Class))
                Class.Text = "Water";
            else if (Database.AtributesStatus.IsWindWalker(client.Player.Class))
                Class.Text = "Windwalker";
            else Class.Text = "Taoist";
            switch (client.Player.Reborn)
            {
                case 2: Reborn.Text = "2nd Reborn"; break;
                case 1: Reborn.Text = "1st Reborn"; break;
                default: Reborn.Text = "None"; break;
            }
            double x = 0;
            if ((client.Player.ExpireVip > DateTime.Now))
            {
                x = (client.Player.ExpireVip - DateTime.Now).TotalDays;
            }
            VIPDays.Text = x.ToString();
            VIP.Text = client.Player.VipLevel.ToString();
        }

        private void flatButton26_Click(object sender, EventArgs e)
        {
           // var target = Pool.GamePoll.Values.FirstOrDefault(i => i.Player.Name == line[1]);
           // if (target != null)
            //{
               // target.Send(Alchemist.Launcher.RequestMachineInfo());
            //}
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;

            System.Windows.Forms.MessageBox.Show("Done Info PC");
        }

        private void flatButton27_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
           

            System.Windows.Forms.MessageBox.Show("Done Check File`s");
        }

        private void flatButton28_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;


            System.Windows.Forms.MessageBox.Show("Done Close Loader");
        }

        private void flatButton29_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            if (flatTextBox12.Text == "")
                return;
            
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
               // client.CreateBoxDialog("" + flatTextBox12.Text + "");
                client.SendWhisper("" + flatTextBox12.Text + " ", "NightMareCo[GM]", client.Player.Name);
                                   
            }
            System.Windows.Forms.MessageBox.Show("Done Send Message");
        }

        private void flatButton30_Click(object sender, EventArgs e)
        {
            if (flatTextBox13.Text == "")
                return;
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(flatTextBox13.Text, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center).GetArray(stream));
                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(flatTextBox13.Text, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.BroadcastMessage).GetArray(stream));
                Server.SendGlobalPacket(new Game.MsgServer.MsgMessage(flatTextBox13.Text, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.System).GetArray(stream));
            }
            System.Windows.Forms.MessageBox.Show("Done Send Message");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var cmd = new MySqlCommand(MySqlCommandType.SELECT))
            {
                cmd.Select("accounts").Where("Username", textBox5.Text);
                using (DBFunctionality.MySqlReader rdr = new DBFunctionality.MySqlReader(cmd))
                {
                    if (rdr.Read())
                    {
                        textBox1.Text = rdr.ReadUInt32("ID").ToString();
                        textBox2.Text = rdr.ReadString("Password");
                        textBox3.Text = rdr.ReadString("Email");
                        textBox4.Text = rdr.ReadString("IP");

                        textBox2.Enabled = true;
                        textBox3.Enabled = true;
                        textBox5.Enabled = false;

                        flatButton31.Enabled = true;
                     

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Username not found");
                    }
                }
            }
        }

        private void flatButton31_Click(object sender, EventArgs e)
        {
            var cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("accounts").Set("Password", textBox2.Text).Set("Email", textBox3.Text)
            .Where("ID", textBox1.Text);
            if (cmd.Execute2() > 0)
            {
                System.Windows.Forms.MessageBox.Show("Done Save");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox5.Enabled = true;

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();

            //button1.Enabled = false;
        }

        private void flatButton32_Click(object sender, EventArgs e)
        {
             VirusX.Client.GameClient client = null;
            client =  VirusX.Client.GameClient.CharacterFromName(flatComboBox1.Text);
            if (client == null)
                return;
            client.Socket.Disconnect();
        }
    }
}
