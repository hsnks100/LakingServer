using LKCamelot.model;
using System.Linq;
using System;
namespace LKCamelot.script.spells
{
    public class Teleport : Spell
    {
        public override string Name { get { return "TELEPORT"; } }
        public override int SpellLearnedIcon { get { return 53; } }
        public override LKCamelot.library.MagicType mType { get { return LKCamelot.library.MagicType.Target2; } }

        public override int DamBase { get { return 0; } }
        public override int DamPl { get { return 0; } }
        public override int ManaCost { get { return 125; } }
        public override int ManaCostPl { get { return 0; } }
        public override int RecastTime
        {
            get
            {
                return 3000;
            }
        }

        public void CastIt(model.Player player, LKCamelot.model.Point2D castx)
        {
            if (LKCamelot.model.World.Dist2d(castx.X, castx.Y, player.X, player.Y) <= 7
                && player.MPCur > this.RealManaCost(player))
            {
                player.MPCur -= this.RealManaCost(player);
                this.CheckLevelUp(player);


                player.Loc = new LKCamelot.model.Point2D(castx.X, castx.Y);
                LKCamelot.model.World.SendToAll(new LKCamelot.model.QueDele(player.Map, new LKCamelot.model.MoveSpriteTele(player.Serial, player.Face, player.X, player.Y).Compile()));
                return;
            }
        }
				public override bool KSCast(int header, model.Player player, int target = 0, short castx = 0, short casty = 0)
				{
					var teleportdist = ((Level / 2) * 2);
					if (teleportdist <= 3) teleportdist = 4;
					if (teleportdist > 12) teleportdist = 12;
					if (World.Dist2d(castx, casty, player.X, player.Y) <= teleportdist
							&& player.MPCur > RealManaCost(player))
					{
						var nmap = LKCamelot.model.Map.FullMaps.Where(xe => xe.Key == player.Map).FirstOrDefault().Value;
						
						TiledMap curmap = null;
						try
						{
							curmap = LKCamelot.model.Map.loadedmaps[nmap];
						}
						catch
						{
							Console.WriteLine(string.Format("Failed to nmap at {0}", nmap));
						}
						LKCamelot.model.MyPathNode randomtile;
						try
						{
							randomtile = curmap.tiles[castx, casty];
						}
						catch
						{
							return true;
						}
						if (randomtile.IsWall)
						{
							return true;
						}
						player.MPCur -= RealManaCost(player);
						CheckLevelUp(player);

						player.Loc = new Point2D(castx, casty);
						World.SendToAll(new QueDele(player.Map, new MoveSpriteTele(player.Serial, player.Face, player.X, player.Y).Compile()));
						World.SendToAll(new QueDele(player.Map, new ExecuteMagic(player.Serial).Compile()));
						return true;
					}
					return base.KSCast(header, player, target, castx, casty);
				}
        public override SpellSequence Seq
        {
            get
            {
                return new SpellSequence(
                    0,  //oncast
                    0,  //moving
                    0,  //impact
                    0,  //thickness
                    0xff,  //type
                    0,  //speed
                    0  //streak
                    );
            }
        }

        public Teleport()
        {
        }
    }
}