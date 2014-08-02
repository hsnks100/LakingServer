using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
	public class Hood : BaseArmor
	{
		public override string Name { get { return "HOOD"; } }

		public override int DamBase { get { return 0; } }
		public override int ACBase { get { return 4; } }

		public override int StrReq { get { return 0; } }
		public override int DexReq { get { return 0; } }

		public override int InitMinHits { get { return 30; } }
		public override int InitMaxHits { get { return 30; } }
        public override ulong BuyPrice { get { return 5000; } }
        public override int SellPrice { get { return 1000; } }

		public override Class ClassReq { get { return 0; } }
		public override ArmorType ArmorType { get { return ArmorType.Helmet; } }

		public Hood() : base (4)
		{
		}

		public Hood(Serial serial) : base (serial)
		{
            m_ItemID = 4;
		}
	}
}
