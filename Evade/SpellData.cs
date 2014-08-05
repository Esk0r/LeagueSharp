#region

using LeagueSharp;

#endregion

namespace Evade
{
    public class SpellData
    {
        public bool AddHitbox;
        public string BaseSkinName;
        public bool CanBeRemoved = false;
        public bool Centered;
        public int DangerValue;
        public int Delay;
        public bool DisableFowDetection = false;
        public bool DonCross = false;
        public bool DontAddExtraDuration;
        public int ExtraDuration;
        public int ExtraRange = -1;
        public bool FixedRange;
        public string FromObject = "";
        public int Id = -1;
        public bool Invert;
        public bool IsDangerous = false;
        public bool MissileFollowsUnit;
        public int MissileSpeed;
        public string MissileSpellName;
        public float MultipleAngle;
        public int MultipleNumber = -1;
        public int RingRadius;
        public SpellSlot Slot;
        public string SpellName;
        public string ToggleParticleName = "";
        public SkillShotType Type;
        private int _radius;
        private int _range;

        public SpellData()
        {
        }

        public SpellData(string baseSkinName, string spellName, SpellSlot slot, SkillShotType type, int delay, int range,
            int radius, int missileSpeed, bool addHitbox, bool fixedRange, int defaultDangerValue)
        {
            BaseSkinName = baseSkinName;
            SpellName = spellName;
            Slot = slot;
            Type = type;
            Delay = delay;
            Range = range;
            _radius = radius;
            MissileSpeed = missileSpeed;
            AddHitbox = addHitbox;
            FixedRange = fixedRange;
            DangerValue = defaultDangerValue;
        }

        public string MenuItemName
        {
            get { return BaseSkinName + " - " + SpellName; }
        }

        public int Radius
        {
            get
            {
                return (!AddHitbox)
                    ? _radius + Config.SkillShotsExtraRadius
                    : Config.SkillShotsExtraRadius + _radius + (int)ObjectManager.Player.BoundingRadius;
            }
            set { _radius = value; }
        }


        public int Range
        {
            get
            {
                return _range +
                       ((Type == SkillShotType.SkillshotLine || Type == SkillShotType.SkillshotMissileLine)
                           ? Config.SkillShotsExtraRange
                           : 0);
            }
            set { _range = value; }
        }
    }
}