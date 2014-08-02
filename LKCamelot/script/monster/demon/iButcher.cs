﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.monster
{
    public class iButcher : Monster
    {
        public override string Name { get { return "Butcher"; } }
        public override int HP { get { return 120; } }
        
        public override int AC { get { return 50; } }
        public override int Hit { get { return 79; } }
        public override int Dam { get { return 56; } }

        public override int XP { get { return 288; } }
        public override int Color { get { return 0; } }
        public override int SpawnTime { get { return 30000; } }
        public override Race Race { get { return Race.Demon; } }

        public override LootPack Loot
        {
            get
            {
                return new LootPack(new LootPackEntry[]
                {
                    new LootPackEntry(.5, typeof(script.item.ExecutionBook), "10d12+150", 40, 1, 1),//added
                    new LootPackEntry(.5, typeof(script.item.HealBook), "10d12+150", 40, 1, 1),//added
                    new LootPackEntry(.5, typeof(script.item.MagicArmorBook), "10d12+150", 40, 1, 1),//added
                    new LootPackEntry(.5, typeof(script.item.ZigZagBook), "10d12+150", 40, 1, 1),//added
                    new LootPackEntry(.5, typeof(script.item.FireBallBook), "10d12+150", 40, 1, 1),//added
                    new LootPackEntry(1, typeof(script.item.SquareShield), "10d12+150", 40, 1, 1),//added
                    new LootPackEntry(1, typeof(script.item.Surplice), "10d12+150", 40, 1, 1),//added
                    new LootPackEntry(1, typeof(script.item.Headgear), "10d12+150", 40, 1, 1),//added
                    new LootPackEntry(1, typeof(script.item.SquareShield), "10d12+150", 40, 1, 1),//added
                    new LootPackEntry(15.0, typeof(script.item.Gold), "10d12+150", 40, 1, 1),
                });
            }
        }

        public iButcher()
            : base(8)
        {
        }

        public iButcher(Serial temp, int x, int y, string map)
            : this(temp)
        {
            m_MonsterID = 8;
            m_Loc = new Point2D(x, y);
            m_SpawnLoc = new Point2D(m_Loc.X, m_Loc.Y);
            m_Map = map;
            m_Serial = temp;
        }

        public iButcher(Serial serial)
            : base(serial)
        {
        }
    }
}
