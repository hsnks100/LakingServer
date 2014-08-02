using LKCamelot.library;
using LKCamelot.model;

namespace LKCamelot.script.item
{
	public class Fleuret : BaseWeapon
	{
		public override string Name { get { return "Fleuret"; } }

		public override int DamBase { get { return 15; } }
		public override int ACBase { get { return 0; } }

		public override int StrReq { get { return 55; } }
		public override int DexReq { get { return 20; } }

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override Class ClassReq { get { return Class.Knight | Class.Swordsman; } }
		public override WeaponType WeaponType { get { return WeaponType.Sword; } }

		public Fleuret() : base (10)
		{
		}

		public Fleuret(Serial serial) : base (serial)
		{
		}
	}
}
