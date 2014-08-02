using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
	public class Hanger : BaseWeapon
	{
		public override string Name { get { return "Hanger"; } }

		public override int DamBase { get { return 28; } }
		public override int ACBase { get { return 0; } }

		public override int StrReq { get { return 141; } }
		public override int DexReq { get { return 42; } }

		public override int InitMinHits { get { return 400; } }
		public override int InitMaxHits { get { return 400; } }
		
		public override int SellPrice { get { return 2000; } }

		public override Class ClassReq { get { return Class.Knight | Class.Swordsman; } }
		public override WeaponType WeaponType { get { return WeaponType.Sword; } }

		public Hanger() : base (11)
		{
		}

		public Hanger(Serial serial) : base (serial)
		{
		}
	}
}
