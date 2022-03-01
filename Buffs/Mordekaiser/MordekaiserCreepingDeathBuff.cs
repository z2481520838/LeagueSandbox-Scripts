using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;


namespace Buffs
{
    class MordekaiserCreepingDeathBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase Owner;
        ISpell Spell;
        IAttackableUnit Target;
        float ticks = 1000;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            Spell = ownerSpell;
            Target = unit;

            StatsModifier.Armor.FlatBonus += 10 + (5 * (ownerSpell.CastInfo.SpellLevel - 1));
            StatsModifier.MagicResist.FlatBonus += 10 + (5 * (ownerSpell.CastInfo.SpellLevel - 1));
            unit.AddStatModifier(StatsModifier);

            AddParticleTarget(Owner, unit, "mordekaiser_creepingDeath_aura.troy", unit, buff.Duration);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            ticks += diff;
            if (ticks >= 1000.0f && Target != null)
            {
                var units = GetUnitsInRange(Target.Position, 350f, true);
                var damage = 24f + (14f * (Spell.CastInfo.SpellLevel - 1)) + Owner.Stats.AbilityPower.Total * 0.2f;
                for (int i = units.Count - 1; i >= 0; i--)
                {
                    if (units[i].Team != Spell.CastInfo.Owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret) && units[i] is IObjAiBase ai)
                    {
                        units[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                        units.RemoveAt(i);
                    }
                }
                ticks = 0f;
            }
        }
    }
}
