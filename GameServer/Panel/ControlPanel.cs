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
//  using SubSonic;


namespace VirusX
{
    public partial class Mahmoud : Form
    {
        public Mahmoud()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            CPs.Text = client.Player.ConquerPoints.ToString();
            Money.Text = client.Player.Money.ToString();
            Level.Text = client.Player.Level.ToString();
            textBox4.Text = client.Player.SecurityPassword.ToString();
            textBox2.Text = client.Player.VipLevel.ToString();
            donate.Text = client.Player.DonatePoints.ToString();
            textBox8.Text = client.Player.WorldPoints.ToString();
            
            textBox9.Text = client.Player.CpsBank.ToString();
            textBox10.Text = client.Player.WHMoney.ToString();

            OnlinePointTB.Text = client.Player.OnlineMinutes.ToString();

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
            textBox3.Text = x.ToString();
            textBox2.Text = client.Player.VipLevel.ToString();

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            client.Socket.Disconnect();

        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            client.Player.ConquerPoints = (uint)uint.Parse(CPs.Text);
            client.Player.Money = long.Parse(Money.Text);
            client.Player.ExpireVip = DateTime.Now.AddDays(double.Parse(textBox3.Text));
            client.Player.VipLevel = byte.Parse(textBox2.Text);
            client.Player.SecurityPassword = uint.Parse(textBox4.Text);
            client.Player.DonatePoints = uint.Parse(donate.Text);
            client.Player.WorldPoints = uint.Parse(textBox8.Text);

            client.Player.CpsBank = long.Parse(textBox9.Text);
            client.Player.WHMoney = uint.Parse(textBox10.Text);

            client.Player.OnlineMinutes = uint.Parse(OnlinePointTB.Text);

            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                client.Player.SendUpdate(stream, client.Player.VipLevel, Game.MsgServer.MsgUpdate.DataType.VIPLevel);
                client.Player.UpdateVip(stream);
                client.UpdateLevel(stream, byte.Parse(Level.Text));
            }
        }
        private void Control_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (var user in Pool.GamePoll.Values)
            {
                comboBox1.Items.Add(user.Player.Name);
            }
            foreach (var item in Pool.ItemsBase.Values)
            {
                comboBox3.Items.Add(item.Name + " " + item.ID);
            }
            foreach (var ban in Database.SystemBannedAccount.BannedPoll.Values)
            {
                comboBox2.Items.Add(ban.Name);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            CPs.Text = client.Player.ConquerPoints.ToString();
            Money.Text = client.Player.Money.ToString();
            Level.Text = client.Player.Level.ToString();
            textBox2.Text = client.Player.VipLevel.ToString();
            textBox4.Text = client.Player.SecurityPassword.ToString();
            donate.Text = client.Player.DonatePoints.ToString();
            textBox8.Text = client.Player.WorldPoints.ToString();
            textBox9.Text = client.Player.CpsBank.ToString();
            textBox10.Text = client.Player.WHMoney.ToString();
            OnlinePointTB.Text = client.Player.OnlineMinutes.ToString();
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
            textBox3.Text = x.ToString();
            textBox2.Text = client.Player.VipLevel.ToString();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            Database.ItemType.DBItem DBItem;
            string[] id = comboBox3.Text.Split(' ').ToArray();
            uint itemId;
            if (uint.TryParse(id.Last(), out itemId) && Pool.ItemsBase.TryGetValue(itemId, out DBItem))
            {
                using (var rec = new ServerSockets.RecycledPacket())
                {
                    var stream = rec.GetStream();
                    client.Inventory.Add(itemId, byte.Parse(this.Plus.Text), DBItem, stream);
                }
            }
        }

        private async void button5_Click_1(object sender, EventArgs e)
        {
            var users = await Task.Run(() =>
                Pool.GamePoll.Values
                    .Select(u => u.Player.Name)
                    .ToList());

            var items = await Task.Run(() =>
                Pool.ItemsBase.Values
                    .Select(i => i.Name + " " + i.ID)
                    .ToList());

            var bans = await Task.Run(() =>
                Database.SystemBannedAccount.BannedPoll.Values
                    .Select(b => b.Name)
                    .ToList());

            comboBox1.DataSource = users;
            comboBox3.DataSource = items;
            comboBox2.DataSource = bans;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var user in Pool.GamePoll.Values)
            {
                if (user.Player.Name.ToLower() == comboBox1.Text.ToLower())
                {
                    Database.SystemBannedAccount.AddBan(user.Player.UID, user.Player.Name, uint.Parse(textBox1.Text));
                    user.Socket.Disconnect();
                    break;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            JiangHu cp = new JiangHu();
            cp.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Chi cp = new Chi();
            cp.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;


            // if (!client.Player.Name.Contains("[PM]"))
            // {
            //  Game.MsgServer.MsgNameChange.ChangeName(client, client.Player.Name + "[PM]", true);
            // }
            if (!client.HelpDesk)
            {
                client.HelpDesk = true;

                client.Player.Level = 255;
                System.Windows.Forms.MessageBox.Show(client.Player.Name + " is now helpdesk");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            if (!client.ProjectManager)
            {
                client.ProjectManager = true;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            if (client.ProjectManager)
            {
                client.ProjectManager = false;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                Database.SystemBannedAccount.RemoveBan(comboBox2.Text);
                comboBox2.Items.Clear();
                foreach (var ban in Database.SystemBannedAccount.BannedPoll.Values)
                {
                    comboBox2.Items.Add(ban.Name);
                }
            }
            catch
            {
                comboBox2.Items.Clear();
                foreach (var ban in Database.SystemBannedAccount.BannedPoll.Values)
                {
                    comboBox2.Items.Add(ban.Name);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {
            /*VirusX.Panels.AccountsForm cp = new Panels.AccountsForm();
            cp.ShowDialog();*/
        }

        private void button18_Click(object sender, EventArgs e)
        {
            // ??????????????? عاوز اسم السلاح واللبس بتوع الوايند والكر علشان اسطفته 
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button13_Click_1(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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

        private void button10_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                // client.Inventory.Add(stream, 3004247);
            }
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                client.Inventory.Add(stream, 3004124);
                // client.Inventory.Add(stream, 3004247);
            }
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                client.Inventory.Add(stream, 4200015);
                client.Inventory.AddItemWitchStack(4200015, 0, 1, stream);
                //     System.Windows.Forms.MessageBox.Show("Done");
                //   client.Inventory.Add(stream, 3004247);
            }
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                client.Inventory.Add(stream, 3001044);
                client.Inventory.Add(stream, 3001044);
                client.Inventory.Add(stream, 3001044);
                client.Inventory.Add(stream, 3001044);
                client.Inventory.AddItemWitchStack(3001044, 0, 100, stream);
                client.Inventory.Add(stream, 3004247);
            }
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                client.Inventory.Add(stream, 4200016);
                client.Inventory.AddItemWitchStack(4200016, 0, 1, stream);
                //     System.Windows.Forms.MessageBox.Show("Done");
                //   client.Inventory.Add(stream, 3004247);
            }
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                client.Inventory.Add(stream, 4200018);
                client.Inventory.AddItemWitchStack(4200018, 0, 1, stream);
                //       System.Windows.Forms.MessageBox.Show("Done");

            }
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            //client.Send(GuardShield.MsgGuardShield.RequestOpenedProcesses());
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            // client.Send(GuardShield.MsgGuardShield.RequestMachineInfo());
        }

        private void button21_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            //client.Send(GuardShield.MsgGuardShield.TerminateLoader(Program.ServerConfig.ServerName, "your client will be closed by GM!"));
        }
        private void button23_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                client.Inventory.Add(stream, 4200017);
                client.Inventory.AddItemWitchStack(4200017, 0, 1, stream);
                //       System.Windows.Forms.MessageBox.Show("Done");

            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                client.Inventory.Add(stream, 3324226);
                client.Inventory.AddItemWitchStack(3324226, 0, 100, stream);
                //       System.Windows.Forms.MessageBox.Show("Done");

            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(4))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 4 free slots into inventory");
                return;
            }
            //var AllFrame = HairfaceStorageTable.Hairfaces.Where(i => i.Type == MsgHairfaceStorage.Type.Frame).ToArray();
            //foreach (var AddAllFrame in AllFrame)
            //{
            //    client.HairfaceStorage.Add(AddAllFrame, true);
            //}
        }

        private void button26_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(4))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 4 free slots into inventory");
                return;
            }
            client.Player.ClassExperience += 1500000;
            //  client.CreateBoxDialog("You have received 5000 ClassExperience.");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(4))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 4 free slots into inventory");
                return;
            }
            foreach (var user in client.Player.MyChi)
            {
                user.Exp = 6063000;
            }
            Role.Instance.Chi.ComputeStatus(client.Player.MyChi);
            client.Equipment.QueryEquipment(client.Equipment.Alternante, false);
            Game.MsgServer.MsgChiInfo.MsgHandleChi.SendInfo(client, Game.MsgServer.MsgChiInfo.Action.Send);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                client.Inventory.Add(stream, 3326003);
                client.Inventory.AddItemWitchStack(3326003, 0, 50, stream);
                //       System.Windows.Forms.MessageBox.Show("Done");

            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(4))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 4 free slots into inventory");
                return;
            }
            client.Player.PayNobilitySystem.Paid((30), (Role.Instance.Nobility.NobilityRank)(12));
            client.Socket.Disconnect();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(4))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 4 free slots into inventory");
                return;
            }
            client.Player.PayNobilitySystem.Paid((30), (Role.Instance.Nobility.NobilityRank)(7));
            client.Socket.Disconnect();
        }
        //private void label110_Click(object sender, EventArgs e)
        //{

        //}
        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                Database.SystemBannedAccount.RemoveBan(comboBox3.Text);
                comboBox3.Items.Clear();
                foreach (var ban in Database.SystemBannedPC.BannedPoll.Values)
                {
                    comboBox3.Items.Add(ban.PlayerName);
                }
            }
            catch
            {
                comboBox3.Items.Clear();
                foreach (var ban in Database.SystemBannedPC.BannedPoll.Values)
                {
                    comboBox2.Items.Add(ban.PlayerName);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_2(object sender, EventArgs e)
        {

            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                client.Inventory.Add(stream, 3314913);
                client.Inventory.AddItemWitchStack(3314913, 0, 1, stream);
                //       System.Windows.Forms.MessageBox.Show("Done");

            }
        }

        //private void label110_Click(object sender, EventArgs e)
        //{

        //}

        //private void textBox7_TextChanged_1(object sender, EventArgs e)
        //{

        //}

        //private void label110_Click(object sender, EventArgs e)
        //{

        //}

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
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
                client.Inventory.Add(stream, 4038009);
                //        client.Inventory.AddItemWitchStack(4038009, 0, 1, stream);
                //       System.Windows.Forms.MessageBox.Show("Done");

            }
        }

        private void donate_TextChanged(object sender, EventArgs e)
        {

        }
        private void donatee_TextChanged(object sender, EventArgs e)
        {

        }
        private void button22_Click_1(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            if (!client.Inventory.HaveSpace(4))
            {
                System.Windows.Forms.MessageBox.Show("Character must have 4 free slots into inventory");
                return;
            }

            //var AllEmoj = Database.HairfaceStorageTable.Hairfaces.Where(i => i.Type == MsgHairfaceStorage.Type.Emoj).ToArray();
            //foreach (var AddAllEmoji in AllEmoj)
            //{
            //    client.HairfaceStorage.Add(AddAllEmoji, true);
            //}
        }

        private void textBox7_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}