﻿namespace LKCamelot.script.spells
{
    public class HailStorm : Spell
    {
        public override string Name { get { return "HAIL STORM"; } }
        public override int SpellLearnedIcon { get { return 40; } }
        public override LKCamelot.library.MagicType mType { get { return LKCamelot.library.MagicType.Target2; } }

        public override int DamBase { get { return 76; } }
        public override int DamPl { get { return 9; } }
        public override int ManaCost { get { return -42; } }
        public override int ManaCostPl { get { return 2; } }
        public override LKCamelot.library.Class ClassReq { get { return LKCamelot.library.Class.Wizard; } }
        public override SpellSequence Seq
        {
            get
            {
                return new SpellSequence(
                    0,  //oncast
                    0,  //moving
                    0,  //impact
                    0,  //thickness
                    0,  //type
                    0,  //speed
                    0  //streak
                    );
            }
        }

        public HailStorm()
        {
        }
    }
}