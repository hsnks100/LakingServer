using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LKCamelot.model;

namespace LKCamelot.script.npc
{
    public class Employee : BaseNPC
    {
        public override string Name { get { return "Employee"; } }
        public override string Map { get { return "Village1"; } }
        public override string ChatString { get { return "Employee: Click on me to see the menu."; } }
        public override int ID { get { return (int)LKCamelot.library.NPCs.Employee; } }
        public override int X { get { return 128; } }
        public override int Y { get { return 90; } }
        public override int Face { get { return 1; } }
        public override int Sprite { get { return 2; } }
        public override int aSpeed { get { return 1; } }
        public override int aFrames { get { return 1; } }
				static script.item.Item createWithIndex(int i, Serial ser)
				{
					if (ser != 0)
					{
						switch (i)
						{
							case 0:
								return new script.item.HealBook(ser);
							case 1:
								return new script.item.PlusHealBook(ser);
							case 2:
								return new script.item.ElectronicBallBook(ser);
							case 3:
								return new script.item.FireBallBook(ser);
							case 4:
								return new script.item.MagicArmorBook(ser);
							case 5:
								return new script.item.RainbowArmorBook(ser);
							case 6:
								return new script.item.GuardianSwordBook(ser);
							case 7:
								return new script.item.FireProtectorBook(ser);
							case 8:
								return new script.item.TeagueShieldBook(ser);
							case 9:
								return new script.item.MagicShieldBook(ser);
							case 10:
								return new script.item.TeleportBook(ser);
							case 11:
								return new script.item.MentalSwordBook(ser);
							case 12:
								return new script.item.TraceBook(ser);
							case 13:
								return new script.item.Dai(ser);
							case 14:
								return new script.item.Zel(ser);
						}
					}
					else
					{
						switch (i)
						{
							case 0:
								return new script.item.HealBook();
							case 1:
								return new script.item.PlusHealBook();
							case 2:
								return new script.item.ElectronicBallBook();
							case 3:
								return new script.item.FireBallBook();
							case 4:
								return new script.item.MagicArmorBook();
							case 5:
								return new script.item.RainbowArmorBook();
							case 6:
								return new script.item.GuardianSwordBook();
							case 7:
								return new script.item.FireProtectorBook();
							case 8:
								return new script.item.TeagueShieldBook();
							case 9:
								return new script.item.MagicShieldBook();
							case 10:
								return new script.item.TeleportBook();
							case 11:
								return new script.item.MentalSwordBook();
							case 12:
								return new script.item.TraceBook();
							case 13:
								return new script.item.Dai();
							case 14:
								return new script.item.Zel();
						}

					}
					return null;
				}
        List<script.item.Item> templ = new List<script.item.Item>()
            {
							createWithIndex(0, 1),
							createWithIndex(1, 1),
							createWithIndex(2, 1),
							createWithIndex(3, 1),
							createWithIndex(4, 1),
							createWithIndex(5, 1),
							createWithIndex(6, 1),
							createWithIndex(7, 1),
							createWithIndex(8, 1),
							createWithIndex(9, 1),
							createWithIndex(10, 1),
							createWithIndex(11, 1),
							createWithIndex(12, 1),
							createWithIndex(13, 1),
							createWithIndex(14, 1),

            };

        public override void Buy(model.Player player, int buyslot)
        {
            if (player.GetFreeSlot() != -1 && player.Gold >= templ[buyslot].BuyPrice)
            {
                LKCamelot.script.item.Item tempitem = null;

								tempitem = createWithIndex(buyslot, 0).Inventory(player);
								player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(
										tempitem).Compile());

                LKCamelot.model.World.NewItems.TryAdd(tempitem.m_Serial, tempitem);
                player.Gold -= (uint)templ[buyslot].BuyPrice;
            }

        }

        public Employee()
        {
        }

        public override GUMP Gump
        {
            get
            {
				//public GUMP(int ID, ushort titlec, ushort boxc, byte time, string shopname, List<item.Item> SellList)
							return new GUMP((int)LKCamelot.library.NPCs.Employee, 0xff85, 0x03ff, 0x70, "Menu", templ); // 게임 속 메뉴.
            }
        }
    }
}
