using VirusX.Panels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//  using SubSonic;


namespace VirusX
{
    public partial class SpawnMonster : Form
    {
        public SpawnMonster()
        {
            InitializeComponent();
        }
       
        private Queue<byte[]> Packets = new Queue<byte[]>();
        public void AddPacket(byte[] buffer)
        {
            lock (Packets)
            {
                byte[] newBuffer = new byte[buffer.Length];
                for (int i = 0; i < newBuffer.Length; i++)
                    newBuffer[i] = buffer[i];
                Packets.Enqueue(newBuffer);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
            var map = Pool.ServerMaps[client.Player.Map];
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                Server.AddMapMonster(stream, map, uint.Parse(textBox1.Text), client.Player.X, client.Player.Y, 18, 18, byte.Parse(MaxSpawn.Text));

                //string m = Game.MsgMonster.MonsterID.MonsterSpawnInformationPath + "Monsters" + client.Player.Map + ".txt";
                //string o = Pool.XUID.Next + " " + client.Player.Map + " " + client.Player.X + " " + client.Player.Y + " " + "18" + " " + "18" + " " + "5" + " " + byte.Parse(MaxSpawn.Text) + " " + uint.Parse(textBox1.Text);
                //if (Checkline(m, o))
                //{
                //    TextWriter tw = new StreamWriter(m, true);
                //    tw.WriteLine(o);
                //    tw.Close();
                //}
            }
        }
        private void Control_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (var user in Pool.GamePoll.Values)
            {
                comboBox1.Items.Add(user.Player.Name);
            }
          
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Client.GameClient client = null;
            client = Client.GameClient.CharacterFromName(comboBox1.Text);
            if (client == null)
                return;
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (var user in Pool.GamePoll.Values)
            {
                comboBox1.Items.Add(user.Player.Name);
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
        private void AutoGetXY(object sender, EventArgs e)
        {
            if (Packets.Count != 0)
            {
                lock (Packets)
                {
                    var buffer = Packets.Dequeue();
                    {
                        if (buffer.Length >= 2)
                        {
                            int type = BitConverter.ToInt16(buffer, 2);
                            if (type == 2046 || type == 2205)
                            {
                                Client.GameClient client = null;
                                client = Client.GameClient.CharacterFromName(comboBox1.Text);
                                if (client == null)
                                    return;
                                XYSpawn.Text = "" + client.Player.X + "  " + client.Player.Y;
                            }
                        }
                        
                    }
                }
            }
           
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}