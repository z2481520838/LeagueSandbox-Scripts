using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class KatarinaQ : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Chained,
                MaximumHits = 4,
                CanHitSameTarget = false,
                CanHitSameTargetConsecutively = false
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var ap = owner.Stats.AbilityPower.Total;
            float damage = (float)(20 + ap * 0.4 + spell.CastInfo.SpellLevel * 20);
            //target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            //AddParticleTarget(owner, target, "katarina_bouncingBlades_tar.troy", target);
            AddBuff("KatarinaQMark", 4f, 1, spell, target, owner, false);
            var xx = GetClosestUnitInRange(target, 300, true);
            if (xx != owner) SpellCast(owner, 2, SpellSlotType.ExtraSlots, true, xx, target.Position);
            if (missile is ISpellChainMissile chainMissile && chainMissile.ObjectsHit.Count > 3) missile.SetToRemove();
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }

    public class KatarinaQMis : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = false,
            PersistsThroughDeath = true,
            SpellDamageRatio = 1.0f,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target,
            }
        };

        private IAttackableUnit firstTarget;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            firstTarget = target;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, true);
        }

        private int bounce = 1;

        //CAN CRASH!!! CARE
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            //if (firstTarget == target)
            //{
            //    return;
            //}
            AddBuff("KatarinaQMark", 4f, 1, spell, target, spell.CastInfo.Owner, false);
            /* var x = GetClosestUnitInRange(target, 600, true);
             if (x.IsDead == false)
             {
                 var owner = spell.CastInfo.Owner;
                 var ap = owner.Stats.AbilityPower.Total * 0.5f;
                 var damage = 45f + spell.CastInfo.SpellLevel * 35f + ap;
                 target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                 if (bounce != 3)
                 {
                     bounce++;
                     SpellCast(owner, 2, SpellSlotType.ExtraSlots, true, x, target.Position);
                     AddBuff("KatarinaQMark", 4f, 1, spell, target, owner, false);
                 }
                 else
                 {
                     bounce = 0;
                 }
             } */
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}