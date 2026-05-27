using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VirusX.Loader
{
    public static class NPC_NameColor
    {
        public static VirusX.ServerSockets.Packet CreateGHPacket(Client.GameClient client)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                stream.InitWriter();
                stream.Write((int)client.NameColor);
                stream.Write((int)0);
                stream.Write(client.Player.HitPoints);
                stream.Write(client.Status.MaxHitpoints);
                stream.Write(client.Player.Name.Length);
                stream.Write(client.Player.Name, client.Player.Name.Length);
                stream.Finalize(4714);
                return stream;
            }
        }

        private static readonly Dictionary<byte, uint> NameColors = new Dictionary<byte, uint>()
            {
    { 1, 0 }, // Normal
    { 2, 0xFFFFFF }, // White
    { 3, 0x0000FF }, // Blue
    { 4, 0xFF0000 }, // Red
    { 5, 0x00FF00 }, // Green
    { 6, 0xFFFF00 }, // Yellow
    { 7, 0xFFA500 }, // Orange
    { 8, 0x800080 }, // Purple
    { 9, 0xFFC0CB }, // Pink
    { 10, 0x8B4513 }, // Brown
    { 11, 0x808080 }, // Gray
    { 12, 0x00FFFF }, // Cyan
    { 13, 0x32CD32 }, // Lime
    { 14, 0x000080 }, // Navy
    { 15, 0x008080 }, // Teal

    // VIP
    { 30, 0xFFD700 }, // Gold
    { 31, 0x8B0000 }, // Dark Red
    { 32, 0x00008B }, // Dark Blue
    { 33, 0x006400 }, // Dark Green
    { 34, 0xFF00FF }, // Magenta
    { 35, 0x00FFFF }, // Neon Cyan
    { 36, 0x39FF14 }, // Neon Green
    { 37, 0x87CEEB }, // Sky Blue
    { 38, 0x8A2BE2 }, // Violet
    { 39, 0xDC143C }, // Crimson
    { 40, 0x4169E1 }, // Royal Blue
    { 41, 0xFF1493 }, // Hot Pink
    { 42, 0xC0C0C0 }, // Silver
    { 43, 0xFF8C00 }, // Dark Orange
    { 44, 0x98FF98 }, // Mint
        };

        static int MakeColor(int hex)
        {
            if (hex == 0)
                return 0;
            int r = (hex >> 16) & 0xFF;
            int g = (hex >> 8) & 0xFF;
            int b = hex & 0xFF;
            int argb = (255 << 24) | (r << 16) | (g << 8) | b;
            return argb;
        }

        private static void SetColor(VirusX.Client.GameClient client, byte Option, VirusX.ServerSockets.Packet stream)
        {
            if (NameColors.TryGetValue(Option, out uint color))
            {
                if (Option >= 30 && client.Player.VipLevel <= 0)
                {
                    VirusX.Game.MsgNpc.Dialog data = new VirusX.Game.MsgNpc.Dialog(client, stream);
                    data.AddText("This color is for VIP players only!")
                       .AddOption("I see.", 255)
                       .FinalizeDialog();
                    return;
                }
                client.SetGHInfo(MakeColor((int)color));
            }
        }

        [VirusX.Game.MsgNpc.NpcAttribute((VirusX.Game.MsgNpc.NpcID)79158)]
        public static void ExitMapShop(VirusX.Client.GameClient client, VirusX.ServerSockets.Packet stream, byte Option, string Input, uint id)
        {
            VirusX.Game.MsgNpc.Dialog data = new VirusX.Game.MsgNpc.Dialog(client, stream);
            switch (Option)
            {
                case 0:
                    {
                        data.AddText("Greetings, " + client.Player.Name + "~Through my power, you may alter your name color and stand distinguished among all players.");
                        data.AddOption("Normal", 1);
                        data.AddOption("Blue", 3);
                        data.AddOption("Red", 4);
                        data.AddOption("Green", 5);
                        data.AddOption("Yellow", 6);
                        data.AddOption("Orange", 7);
                        data.AddOption("Purple", 8);
                        data.AddOption("Pink", 9);
                        data.AddOption("Brown", 10);
                        data.AddOption("Gray", 11);
                        data.AddOption("Cyan", 12);
                        data.AddOption("Lime", 13);
                        data.AddOption("Navy", 14);
                        data.AddOption("Teal", 15);
                        data.AddOption("Gold [VIP]", 30);
                        data.AddOption("Dark Red [VIP]", 31);
                        data.AddOption("Dark Blue [VIP]", 32);
                        data.AddOption("Dark Green [VIP]", 33);
                        data.AddOption("Magenta [VIP]", 34);
                        data.AddOption("Neon Cyan [VIP]", 35);
                        data.AddOption("Neon Green [VIP]", 36);
                        data.AddOption("Sky Blue [VIP]", 37);
                        data.AddOption("Violet [VIP]", 38);
                        data.AddOption("Crimson [VIP]", 39);
                        data.AddOption("Royal Blue [VIP]", 40);
                        data.AddOption("Hot Pink [VIP]", 41);
                        data.AddOption("Silver [VIP]", 42);
                        data.AddOption("Dark Orange [VIP]", 43);
                        data.AddOption("Mint [VIP]", 44);
                        data.AddOption("Thanks.", 255);
                        data.AddAvatar(63).FinalizeDialog();
                        break;
                    }

                default:
                    {
                        SetColor(client, Option, stream);
                        break;
                    }
            }
        }
    }
}
