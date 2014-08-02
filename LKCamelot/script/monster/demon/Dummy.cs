﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.monster
{
    public class Dummy : Monster
    {
        public override string Name { get { return "Dummy"; } }
        public override int HP { get { return 2000; } }
        public override int Dam { get { return 204; } }
        public override int AC { get { return 140; } }
        public override int Hit { get { return 192; } }
        public override int XP { get { return 13900; } }
        public override int Color { get { return 0; } }
        public override int SpawnTime { get { return 30000; } }
        public override Race Race { get { return Race.Demon; } }

        public override LootPack Loot
        {
            get
            {
                return new LootPack(new LootPackEntry[]
                {
                    new LootPackEntry(0.02, typeof(script.item.FireHawkBook), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.02, typeof(script.item.AssassinBook), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.1, typeof(script.item.FireShotBook), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.1, typeof(script.item.ExecutionBook), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.2, typeof(script.item.EnamelLeather), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.2, typeof(script.item.CoatingShield), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.2, typeof(script.item.BroadAxe), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.2, typeof(script.item.ChestGuard), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.2, typeof(script.item.FieldCap), "10d22+250", 40, 1, 1),
                    new LootPackEntry(15.0, typeof(script.item.Gold), "10d30+550", 40, 1, 1),
                });
            }
        }

        public Dummy()
            : base(14)
        {
        }

        public Dummy(Serial temp, int x, int y, string map)
            : this(temp)
        {
            m_MonsterID = 14;
            m_Loc = new Point2D(x, y);
            m_SpawnLoc = new Point2D(m_Loc.X, m_Loc.Y);
            m_Map = map;
            m_Serial = temp;
        }

        public Dummy(Serial serial)
            : base(serial)
        {
        }
    }
}
