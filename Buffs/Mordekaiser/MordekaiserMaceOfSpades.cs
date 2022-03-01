using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;

namespace Buffs
{
    class MordekaiserMaceOfSpades : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ISpell Spell;
        IBuff Buff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Spell = ownerSpell;
            Buff = buff;
            ownerSpell.CastInfo.Owner.CancelAutoAttack(true);
            if(unit is IObjAiBase obj)
            {
                ApiEventManager.OnHitUnit.AddListener(this, obj, TargetExecute, true);
            }
        }

        public void TargetExecute(IDamageData damageData)
        {
            var owner = damageData.Attacker;
            var ADratio = owner.Stats.AttackDamage.FlatBonus;
            var APratio = owner.Stats.AbilityPower.Total * 0.4f;
            var damage = 80f + (30 * (Spell.CastInfo.SpellLevel - 1)) + ADratio + APratio;
            bool isCrit = false;

            AddParticleTarget(owner, owner, "mordakaiser_siphonOfDestruction_self.troy", owner, 1f);

            var units = GetUnitsInRange(owner.Position, 300f, true);
            for (var i = units.Count - 1; i >= 0; i--)
            {
                if (units[i].Team == owner.Team || units[i] is IBaseTurret || units[i] is INexus || units[i] is IObjBuilding || units[i] is ILaneTurret)
                {
                    units.RemoveAt(i);
                }
            }

            string particles = "mordakaiser_maceOfSpades_tar.troy";
            for (var i = 0; i < units.Count; i++)
            {
                if ((units.Count) == 1)
                {
                    damage *= 1.65f;
                    isCrit = true;
                    particles = "mordakaiser_maceOfSpades_tar2.troy";
                }
                units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, isCrit);
                AddParticleTarget(owner, owner, particles, units[i], 1f);
            }

            Buff.DeactivateBuff();
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        public void OnPreAttack(ISpell spell)
        {

        }

        public void OnUpdate(float diff)
        {
        }
    }
}
