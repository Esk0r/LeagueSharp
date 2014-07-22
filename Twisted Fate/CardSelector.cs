#region

using System;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

#endregion

namespace TwistedFate
{
    public enum Cards
    {
        Red,
        Yellow,
        Blue,
        None,
    }

    public enum SelectStatus
    {
        Ready,
        Selecting,
        Selected,
        Cooldown,
    }


    public static class CardSelector
    {

        public static Cards Select;
        public static SelectStatus Status { get; set; }
        public static int LastWSent = 0;
        public static int LastSendWSent = 0;


        static CardSelector()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Game.OnGameProcessPacket += Game_OnGameProcessPacket;
            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        private static void SendWPacket()
        {
            var packet = Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(0, SpellSlot.W));
            packet.Send();
            LastSendWSent = Environment.TickCount;
        }

        public static void StartSelecting(Cards card)
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "PickACard" && Status == SelectStatus.Ready)
            {
                Select = card;
                if (Environment.TickCount - LastWSent > 200)
                {
                    if (ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W))
                        LastWSent = Environment.TickCount;
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if ((ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.W) == SpellState.Ready && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "PickACard" && (Status != SelectStatus.Selecting || Environment.TickCount - LastWSent > 500)) 
                
                || ObjectManager.Player.IsDead) Status = SelectStatus.Ready;

            if (ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.W) == SpellState.Cooldown && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "PickACard")
            {
                Select = Cards.None;
                Status = SelectStatus.Cooldown;
            }

            if (ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.W) == SpellState.Surpressed &&
                !ObjectManager.Player.IsDead)
                Status = SelectStatus.Selected;
        }

        private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            if (args.PacketData[0] == 0x17)
            {
                var packet = new GamePacket(args.PacketData);
                packet.Position = 1;
                if (packet.ReadInteger() == ObjectManager.Player.NetworkId)
                {
                    packet.Position = 7;
                    var id = packet.ReadByte();
                    switch (id)
                    {
                        case 0x42:
                            if (Select == Cards.Blue) SendWPacket();
                            break;
                        case 0x47:
                            if (Select == Cards.Yellow) SendWPacket();
                            break;
                        case 0x52:
                            if (Select == Cards.Red) SendWPacket();
                            break;
                    }
                }
            }
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name == "PickACard")
            {
                Status = SelectStatus.Selecting;
            }

            if (args.SData.Name == "goldcardlock" || args.SData.Name == "bluecardlock" ||
                args.SData.Name == "redcardlock")
            {
                Status = SelectStatus.Selected;
            }
        }
    }
}