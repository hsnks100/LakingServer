using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.script.spells;
namespace LKCamelot.model
{
	public class CastHandler
	{
		io.IOClient client;
		PlayerHandler handler;
		public CastHandler(io.IOClient client, PlayerHandler handler)
		{
			this.client = client;
			this.handler = handler;
		}
		struct Location
		{
        public string m_Map;
        public Point2D m_Loc; 
		};
		System.Collections.Generic.Stack<Location> m_comeBacks = new Stack<Location>();
		public void CreateMagicEffect(Point2D target, string map, byte sprite, int time = 1500)
		{
			int mobile = Serial.NewMobile;
			World.SendToAll(new QueDele(map, new CreateMagicEffect(mobile, 1, (short)target.X, (short)target.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, sprite }, 0).Compile()));
			var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + time, map, new DeleteObject(mobile).Compile());
			tmp.tempser = mobile;
			World.TickQue.Add(tmp);
		}

		public void TakeDamage(Player caster, Player target, script.spells.Spell spell)
		{
			float h = ((float)caster.Hit / ((float)caster.Hit + (float)target.Hit)) * 200;

			if (h >= 100 || new Random().Next(0, 100) < (int)h)
			{
				int take = spell.DamBase + (spell.DamPl * spell.Level);// +(spell.DamPl * spell.SLevel2);
				if (spell.ManaCostPl != 0)
				{
					take += (caster.GetStat("men") / spell.menCoff);
					take += (caster.GetStat("str") / spell.strCoff);
					take += (caster.GetStat("dex") / spell.dexCoff);
				}
				if (spell is script.spells.DemonDeath)
					take = Convert.ToInt32(caster.HP * 0.5) + caster.GetStat("dex");

				if (target.Color == 0)
					caster.pkpinkktime = Server.tickcount.ElapsedMilliseconds;

				target.HPCur -= (take - target.AC); // 방어력을 고려해서 깜.j
				if (target.Map == "Rest" && target.Color != 1)
				{
					caster.pklastpk.Add(Server.tickcount.ElapsedMilliseconds);
					caster.pklastred = Server.tickcount.ElapsedMilliseconds;
				}
				// 맞았을 때 깜빡 거리는 코드인 것 같음.
				World.SendToAll(new QueDele(caster.m_Map, new HitAnimation(target.Serial,
					 Convert.ToByte(((((float)target.HPCur / (float)target.HP) * 100) * 1))).Compile()));
			}
		}



		public void HandleCast(int header, script.spells.Spell castspell, Player player, int target = 0, short castx = 0, short casty = 0)
		{
			if (castspell is Teleport)
			{
				castspell.KSCast(header, player, target, castx, casty);
				return;	
			}
			if (castspell is ComeBack)
			{ 
				if (player.MPCur > castspell.RealManaCost(player))
				{
					player.MPCur -= castspell.RealManaCost(player);
					castspell.CheckLevelUp(player);
					Location loc;
					loc.m_Loc = player.Loc;
					loc.m_Map = player.Map;
					m_comeBacks.Push(loc);

					try
					{
						player.Loc = new Point2D(8, 12);
						player.Map = "Arnold";
					}
					catch
					{
						return;	
					} 
				}
			}
			if (castspell is Trace)
			{
				if (player.MPCur > castspell.RealManaCost(player))
				{
					player.MPCur -= castspell.RealManaCost(player);
					castspell.CheckLevelUp(player);

					try
					{
						Location loc = m_comeBacks.Pop();
						player.Loc = loc.m_Loc;
						player.Map = loc.m_Map;
					}
					catch { return; }

					World.SendToAll(new QueDele(player.Map, new MoveSpriteTele(player.Serial, player.Face, player.X, player.Y).Compile()));
					return;
				}
			}

			// 도달 가능한 몬스터에 대한 배열
			var caston = World.NewMonsters.Where(xe => xe.Value.m_Serial == target
													&& World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.X, player.Y) <= castspell.Range
													&& xe.Value.Alive
					&& xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map
					).Select(xe => xe.Value);
			// 도달 가능한 플레이어에 대한 배열.
			var playcaston = PlayerHandler.getSingleton().add.Where(xe => xe.Value != null && xe.Value != player && xe.Value.loggedIn
					&& World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.m_Loc.X, player.m_Loc.Y) <= castspell.Range
					&& xe.Value.Serial == (Serial)target
					&& xe.Value.m_Map != null && xe.Value.m_Map == player.m_Map).FirstOrDefault();

			// 무슨 의미인지 잘 모르겠음. 공격 타입에 따라 몬스터 범위를 다시 구하는것 같음.
			if (castspell.mType == LKCamelot.library.MagicType.Casted || castspell.mType == LKCamelot.library.MagicType.Target)
			{
				caston = World.NewMonsters.Where(xe => xe.Value.m_Map != null
							 && xe.Value.m_Map == player.Map
							 && World.Dist2d(xe.Value.m_Loc.X, xe.Value.m_Loc.Y, player.X, player.Y) <= castspell.Range
							 && xe.Value.Alive)
							.Select(xe => xe.Value);
			}

			// P.K 에 대한 코드 인것 같음.
			if (playcaston.Key != null
					&& !(player.Map == "Village1" || player.Map == "Rest" || player.Map == "Arnold" || player.Map == "Loen")
					)
			{


				if (castspell.Name == "DEMON DEATH")
				{
					if (player.HPCur < (int)(player.HP * 0.70))
						return;

					var miyamo = player.Equipped.Where(xe => xe.GetType() == typeof(script.item.MiyamotosStick)).FirstOrDefault();
					var recast = castspell.RecastTime;
					if (miyamo != null)
					{
						recast -= 1000;
						recast -= miyamo.Stage * 300;
					}

					if (LKCamelot.Server.tickcount.ElapsedMilliseconds - recast > castspell.Cooldown)
					{
						castspell.Cooldown = LKCamelot.Server.tickcount.ElapsedMilliseconds;
					}
					else
						return;

					player.HPCur -= castspell.RealManaCost(player);
					castspell.CheckLevelUp(player);

					int mobile = Serial.NewMobile;
					World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)playcaston.Value.m_Loc.X, (short)playcaston.Value.m_Loc.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, (byte)castspell.Seq.OnImpactSprite }, 0).Compile()));
					var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 2000, player.m_Map, new DeleteObject(mobile).Compile());
					tmp.tempser = mobile;
					World.TickQue.Add(tmp);

					TakeDamage(player, playcaston.Value, castspell);

					return;
				}

				else // demon 이 아닌 경우
				{
					if (player.MPCur < castspell.RealManaCost(player))
						return;
					player.MPCur -= castspell.RealManaCost(player);
					castspell.CheckLevelUp(player);
					TakeDamage(player, playcaston.Value, castspell);
					World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial,
							castx, casty, castspell.Seq).Compile()));
					System.Console.WriteLine("pk 데몽이 아닌 경우");
				}
			}


			switch (castspell.mType)
			{
				case (LKCamelot.library.MagicType.Target2):
					foreach (var targete in caston)
					{
						if (castspell is ISingle)
						{
							System.Diagnostics.Debug.Assert(false);
							if (player.MPCur < castspell.RealManaCost(player))
								return;
							player.MPCur -= castspell.RealManaCost(player);
							castspell.CheckLevelUp(player);

							CreateMagicEffect(targete.m_Loc, targete.m_Map, (byte)castspell.Seq.OnImpactSprite, 1500);

							targete.TakeDamage(player, castspell);
							return;
						}

						if (castspell.Name == "DEMON DEATH")
						{
							if (player.HPCur < (int)(player.HP * 0.70))
								return;

							var miyamo = player.Equipped.Where(xe => xe.GetType() == typeof(script.item.MiyamotosStick)).FirstOrDefault();
							var recast = castspell.RecastTime;
							if (miyamo != null)
							{
								recast -= 1000;
								recast -= miyamo.Stage * 300;
							}

							if (LKCamelot.Server.tickcount.ElapsedMilliseconds - recast > castspell.Cooldown)
							{
								castspell.Cooldown = LKCamelot.Server.tickcount.ElapsedMilliseconds;
							}
							else
								return;

							player.HPCur -= castspell.RealManaCost(player);
							castspell.CheckLevelUp(player);

							int mobile = Serial.NewMobile;
							World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)targete.m_Loc.X, (short)targete.m_Loc.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, (byte)castspell.Seq.OnImpactSprite }, 0).Compile()));
							var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 2000, player.m_Map, new DeleteObject(mobile).Compile());
							tmp.tempser = mobile;
							World.TickQue.Add(tmp);

							targete.TakeDamage(player, castspell);

							return;
						}

						if (player.MPCur < castspell.RealManaCost(player))
							return;
						player.MPCur -= castspell.RealManaCost(player);
						castspell.CheckLevelUp(player);
						targete.TakeDamage(player, castspell);
						World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial,
								castx, casty, castspell.Seq).Compile()));
					}

					break;

				case (LKCamelot.library.MagicType.Casted):
					if (player.MPCur < castspell.RealManaCost(player))
						return;
					player.MPCur -= castspell.RealManaCost(player);

					if (castspell.Cast(player))
						return;

					foreach (var targete in caston)
						targete.TakeDamage(player, castspell);

					World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial,
						1, 1, castspell.Seq).Compile()));

					break;
				case (LKCamelot.library.MagicType.Target):
					if (player.MPCur < castspell.RealManaCost(player))
						return;
					player.MPCur -= castspell.RealManaCost(player);
					if (castspell.Cast(player))
						return;

					World.SendToAll(new QueDele(player.Map, new CurveMagic(player.Serial, 1, 1, castspell.Seq).Compile()));
					foreach (var targetee in caston)
					{
						int mobile = Serial.NewMobile;
						World.SendToAll(new QueDele(player.Map, new CreateMagicEffect(mobile, 1, (short)targetee.m_Loc.X, (short)targetee.m_Loc.Y, new byte[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, (byte)castspell.Seq.OnImpactSprite }, 0).Compile()));
						// World.SendToAll(new QueDele(player.Map, new SetObjectEffectsMonsterSpell(targetee, castspell.Seq.OnImpactSprite).Compile()));
						targetee.TakeDamage(player, castspell);
						var tmp = new QueDele(LKCamelot.Server.tickcount.ElapsedMilliseconds + 1000, player.m_Map, new DeleteObject(mobile).Compile());
						tmp.tempser = mobile;
						World.TickQue.Add(tmp);
					}
					break;
			}
		}
	}
}
