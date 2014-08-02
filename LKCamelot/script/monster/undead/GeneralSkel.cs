﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.monster
{
    public class GeneralSkel : Monster
    {
        public override string Name { get { return "General Skel"; } }
        public override int HP { get { return 350; } }
        public override int Dam { get { return 108; } }
        public override int AC { get { return 83; } }
        public override int Hit { get { return 119; } }
        public override int XP { get { return 1408; } }
        public override int Color { get { return 0; } }
        public override int SpawnTime { get { return 30000; } }
        public override Race Race { get { return Race.Undead; } }

        public override LootPack Loot
        {
            get
            {
                return new LootPack(new LootPackEntry[]
                {
                    new LootPackEntry(0.1, typeof(script.item.Hack), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.1, typeof(script.item.BroadSword), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.1, typeof(script.item.SpikedClub), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.1, typeof(script.item.LeatherArmor), "10d22+250", 40, 1, 1),
                    new LootPackEntry(0.1, typeof(script.item.LargeShield), "10d22+250", 40, 1, 1),
                    new LootPackEntry(15.0, typeof(script.item.Gold), "10d24+300", 40, 1, 1),
                });
            }
        }

        public GeneralSkel()
            : base(25)
        {
        }

        public GeneralSkel(Serial temp, int x, int y, string map)
            : this(temp)
        {
            m_MonsterID = 25;
            m_Loc = new Point2D(x, y);
            m_SpawnLoc = new Point2D(m_Loc.X, m_Loc.Y);
            m_Map = map;
            m_Serial = temp;
        }

        public GeneralSkel(Serial serial)
            : base(serial)
        {
        }
    }
}
