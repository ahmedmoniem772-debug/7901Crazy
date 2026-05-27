using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirusX.Client;
using VirusX.Game;
namespace VirusX.Panels
{
    public partial class ChatPanal : Form
    {
        public static System.Collections.Generic.Dictionary<string, Client> Clients = new Dictionary<string, Client>();
        public string SelectedClient = "";
        public ChatPanal()
        {
            InitializeComponent();
            Clients.Clear();
            SelectedClient = "";
            ClientList.ForeColor = Color.Black;
            RecList.ForeColor = Color.Black;
            this.Text = "ChatBox " + Program.ServerConfig.ServerName;
            this.RecList.DoubleClick += new System.EventHandler(this.RecList_MouseClick);
            this.ClientList.DoubleClick += new System.EventHandler(this.ClientList_MouseClick);
            base.Closing += this.Form1_Closing;

        }
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                VirusX.Client.GameClient client;
                if (Pool.GamePoll.TryRemove(uint.MaxValue, out client))
                {
                    var Map = Pool.ServerMaps[1002];
                    Map.Denquer(client);
                }
                Clients.Clear();
                SelectedClient = "";
            }
            catch
            {
            }
            e.Cancel = false;
        }
        private void RecList_MouseClick(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = RecList.SelectedItem.ToString();
            }
            catch
            {
                textBox1.Text = "";
            }
        }
        private void ClientList_MouseClick(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = ClientList.SelectedItem.ToString();
            }
            catch
            {
                textBox1.Text = "";
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Client client;
                SelectedClient = ClientList.Text;
                if (SelectedClient.Contains("(UnSeen)"))
                {
                    SelectedClient = SelectedClient.Remove(SelectedClient.Length - 8, 8);
                    ClientList.Text = SelectedClient;
                    ClientList.Items[ClientList.SelectedIndex] = SelectedClient;
                }
                if (Clients.TryGetValue(SelectedClient, out client))
                {
                    client.Seen = true;
                    RecList.Items.Clear();
                    foreach (var item in client.Mess)
                    {
                        RecList.Items.Add(item);
                    }
                }
            }
            catch
            {

            }
        }
        private void Send_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var user in Pool.GamePoll.Values)
                {
                    if (user.Player.Name == SelectedClient)
                    {
                        user.SendWhisper(SendText.Text, "[GM]Momen[PM]", SelectedClient);
                        Clients[SelectedClient].Mess.Add("[" + DateTime.Now.ToString() + "] [GM]Momen[PM]>>>" + SendText.Text);
                        var x = Clients[SelectedClient].Mess.Last();
                        RecList.Items.Add(x);
                        SendText.Text = "";
                    }
                }
            }
            catch
            {

            }
        }
        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ClientList.Items.Clear();
                foreach (var x in Clients)
                {
                    if (x.Value.Seen)
                    {
                        ClientList.Items.Add(x.Key);
                    }
                    else
                    {
                        if (SelectedClient != x.Key)
                        {
                            ClientList.Items.Add(x.Key + "(UnSeen)");
                        }
                        else
                        {
                            ClientList.Items.Add(x.Key);
                        }
                    }
                }
                RecList.Items.Clear();
                Client client;
                if (SelectedClient != "")
                {
                    if (Clients.TryGetValue(SelectedClient, out client))
                    {
                        client.Seen = true;
                        foreach (var item in client.Mess)
                        {
                            RecList.Items.Add(item);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void ChatPanal_Load(object sender, EventArgs e)
        {
            if (!Pool.GamePoll.ContainsKey(uint.MaxValue))
            {
                VirusX.Client.GameClient pclient = new VirusX.Client.GameClient(null);
                pclient.Fake = true;

                pclient.Player = new Role.Player(pclient);
                pclient.Inventory = new Role.Instance.Inventory(pclient);
                pclient.Equipment = new Role.Instance.Equip(pclient);
                pclient.Warehouse = new Role.Instance.Warehouse(pclient);
                pclient.MyProfs = new Role.Instance.Proficiency(pclient);
                pclient.MySpells = new Role.Instance.Spell(pclient);
                pclient.Achievement = new Database.AchievementCollection();
                pclient.Status = new Game.MsgServer.MsgStatus();
                pclient.Player.InnerPower = new Role.Instance.InnerPower(pclient.Player.Name, pclient.Player.UID);
                pclient.HundredWeapons = new Role.Instance.HundredWeapons(pclient);
                pclient.Player.SubClass = new Role.Instance.SubClass();
                pclient.Player.MyUnion = new Role.Instance.Union();
                pclient.Player.MyChi = new Role.Instance.Chi(pclient.Player.UID);
                pclient.Player.MyJiangHu = new Role.Instance.JiangHu(pclient.Player.UID);
                pclient.Player.Associate = new Role.Instance.Associate.MyAsociats(pclient.Player.UID);

                pclient.Rune = new Role.Instance.Rune(pclient);
                pclient.Beasts = new Role.Instance.Beasts(pclient);
                pclient.Player.MyClan = new Role.Instance.Clan();
                pclient.Player.Name = "[GM]PinoyConquer[PM]";
                pclient.Player.Body = 1008;
                pclient.Player.UID = uint.MaxValue;
                pclient.Player.HitPoints = ushort.MaxValue;
                pclient.Status.MaxHitpoints = ushort.MaxValue;

                pclient.Player.X = (ushort)313;
                pclient.Player.Y = (ushort)285;
                pclient.Player.Map = 1002;
                pclient.Player.Level = 255;
                pclient.Player.ServerID = (ushort)Database.GroupServerList.MyServerInfo.ID;
                pclient.Player.Face = 153;
                pclient.Player.Action = Role.Flags.ConquerAction.Sit;
                pclient.Player.Angle = Role.Flags.ConquerAngle.SouthWest;
                pclient.Player.Hair = 774;
                pclient.Player.GarmentId = 193625;
                pclient.Player.LeftWeaponAccessoryId = 360047;
                pclient.Player.RightWeaponAccessoryId = 360047;
                pclient.Map = Pool.ServerMaps[1002];
                pclient.Map.Enquer(pclient);
                Pool.GamePoll.TryAdd(pclient.Player.UID, pclient);
                using (var p = new ServerSockets.RecycledPacket())
                {
                    var stream = p.GetStream();
                    pclient.Player.View.SendView(pclient.Player.GetArray(stream, false), false);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Controlpanel cp = new Controlpanel();
                cp.ShowDialog();
            }
            catch
            {

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clients.ContainsKey(ClientList.Items[ClientList.SelectedIndex].ToString()))
                {
                    Clients.Remove(ClientList.Items[ClientList.SelectedIndex].ToString());
                }
                ClientList.Items.RemoveAt(ClientList.SelectedIndex);
            }
            catch
            {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (var user in Pool.GamePoll.Values)
                {
                    if (user.Player.Name == SelectedClient)
                    {
                        user.SendWhisper(comboBox1.Text, "[GM]Momen[PM]", SelectedClient);
                        Clients[SelectedClient].Mess.Add("[" + DateTime.Now.ToString() + "] [GM]Momen[PM]>>>" + comboBox1.Text);
                        var x = Clients[SelectedClient].Mess.Last();
                        RecList.Items.Add(x);
                        comboBox1.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

    }
    public class Client
    {
        public List<string> Mess;
        public bool Seen = true;
    }
}
