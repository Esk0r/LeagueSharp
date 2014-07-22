using LeagueSharp;
using LeagueSharp.Common;

namespace Evade
{

    internal enum SpellValidTargets
    {
        AllyMinions,
        EnemyMinions,
        
        AllyWards,
        EnemyWards,
        
        AllyChampions,
        EnemyChampions,
    }

    /// <summary>
    /// Class containing the needed info about the evading spells.
    /// </summary>
    internal class EvadeSpellData
    {
        public string Name;

        public int Delay;
        public bool FixedRange;

        public bool IsTargetted
        {
            get
            {
                return ValidTargets != null;
            }
        }

        public SpellValidTargets[] ValidTargets;

        public bool IsBlink;
        public bool IsDash;
        public bool IsShield;
        public bool IsInvulnerability;
        public bool IsSpellShield;

        public float MaxRange;

        public int Speed;

        public int _dangerLevel;
        public SpellSlot Slot;
        public bool IsSummonerSpell;
        public int DangerLevel
        {
            get
            {
                if (Config.Menu.Item("DangerLevel" + Name) != null)
                {
                    return Config.Menu.Item("DangerLevel" + Name).GetValue<Slider>().Value;
                }
                return _dangerLevel;
            }
        }

        public bool Enabled
        {
            get
            {
                if (Config.Menu.Item("Enabled" + Name) != null)
                {
                    return Config.Menu.Item("Enabled" + Name).GetValue<bool>();
                }
                return true;
            }
        }

        public EvadeSpellData()
        {
            
        }

        public EvadeSpellData(string name, int dangerLevel)
        {
            Name = name;
            _dangerLevel = dangerLevel;
        }
    }

    internal class DashData : EvadeSpellData
    {
        public DashData(string name, SpellSlot slot , float range, bool fixedRange, int delay, int speed, int dangerLevel)
        {
            Name = name;
            MaxRange = range;
            Slot = slot;
            FixedRange = fixedRange;
            Delay = delay;
            Speed = speed;
            _dangerLevel = dangerLevel;
            IsDash = true;
        }
    }

    internal class BlinkData : EvadeSpellData
    {
        public BlinkData(string name, SpellSlot slot, float range, int delay, int dangerLevel, bool isSummonerSpell = false)
        {
            Name = name;
            MaxRange = range;
            Slot = slot;
            Delay = delay;
            _dangerLevel = dangerLevel;
            IsSummonerSpell = isSummonerSpell;
            IsBlink = true;
        }
    }

    internal class InvulnerabilityData : EvadeSpellData
    {
        public InvulnerabilityData(string name, SpellSlot slot, int delay, int dangerLevel)
        {
            Name = name;
            Slot = slot;
            Delay = delay;
            _dangerLevel = dangerLevel;
            IsInvulnerability = true;
        }
    }

    internal class ShieldData : EvadeSpellData
    {
        public ShieldData(string name, SpellSlot slot, int delay, int dangerLevel, bool isSpellShield = false)
        {
            Name = name;
            Slot = slot;
            Delay = delay;
            _dangerLevel = dangerLevel;
            IsSpellShield = isSpellShield;
            IsShield = !IsSpellShield;
        }
    }
}