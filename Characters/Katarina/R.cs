using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Linq;
using GameServerCore;


namespace Spells
{
    public class KatarinaR : ISpellScript
    {
        private bool cancelled;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };

        private Vector2 basepos;
        public ISpellSector DamageSector;
        IParticle p;
        IObjAiBase Owner;
        float ticks;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            basepos = owner.Position;
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            Owner = owner;
            p = AddParticleTarget(owner, owner, "Katarina_deathLotus_cas.troy", owner, lifetime: 2.5f, bone: "C_BUFFBONE_GLB_CHEST_LOC");
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                BindObject = owner,
                Length = 500f,
                Tickrate = 4,
                CanHitSameTargetConsecutively = true,
                Type = SectorType.Area,
                Lifetime = 2.5f
                
            });
            var champs = GetChampionsInRange(owner.Position, 500, true);
            if (champs.Count < 1)
            {
                DamageSector.SetToRemove();
                p.SetToRemove();
                RemoveParticle(p);
            }
            if (owner.Position != basepos)
            {
                DamageSector.SetToRemove();
                p.SetToRemove();
                RemoveParticle(p);
            }
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile swag, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var AP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.25f;
            var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
            float damage = 5f + spell.CastInfo.SpellLevel * 35f + AP + AD;
            //var champs = GetChampionsInRange(owner.Position, 500, true);
            var champs = GetChampionsInRange(owner.Position, 500f, true).OrderBy(enemy => Vector2.DistanceSquared(enemy.Position, owner.Position)).ToList();
            if (champs.Count > 3)
            {
                foreach (var enemy in champs.GetRange(0, 4)
                     .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
                {
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, true, enemy, owner.Position);
                    enemy.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
            else
            {
                foreach (var enemy in champs.GetRange(0, champs.Count)
                    .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
                {
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, true, enemy, owner.Position);
                    enemy.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
            if (champs.Count == 0)
            {
                DamageSector.SetToRemove();
                p.SetToRemove();
                RemoveParticle(p);
            }
            if (owner.Position != basepos)
            {
                DamageSector.SetToRemove();
                p.SetToRemove();
                RemoveParticle(p);
            }


        }


        public void OnSpellChannel(ISpell spell)
        {

        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
            DamageSector.SetToRemove();
            p.SetToRemove();
            RemoveParticle(p);
        }

        public void OnSpellPostChannel(ISpell spell)
        {
            DamageSector.SetToRemove();
            p.SetToRemove();
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
        }
    }






    public class KatarinaRMis : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner as IChampion;

            foreach (var enemy in GetUnitsInRange(target.Position, 550, true)
             .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            {
                //SpellCast(owner, 0, SpellSlotType.ExtraSlots, true, enemy, target.Position);
            }

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
}