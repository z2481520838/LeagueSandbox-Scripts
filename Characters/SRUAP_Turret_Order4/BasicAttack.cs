using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using static GameServerLib.API.APIMapFunctionManager;

namespace Spells
{
    public class SRUAP_Turret_Order4BasicAttack : ISpellScript
    {
        IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
            AddParticleTarget(Owner, Owner, "SRU_Inhibitor_Order_Tower_Beam_Lvl1", Owner.TargetUnit, bone: "Buffbone_Glb_Weapon_1");
            AddParticleTarget(owner, owner, "SRU_Inhibitor_Tower_Chaos_Beam_Cast_Audio", owner);
            AddParticleTarget(owner, target, "SRU_Order_Laser_Turret_Tar", target);
            AddBuff("S5Test_TowerWrath", 0.5f, 1, spell, target, owner);
            //it also applies a buff to itself, that buff is yet to be known
        }

        public void OnLaunchAttack(ISpell spell)
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

        float delayTime = 500;
        float _delayTime = 500;
        float reductimer = 480.0f * 1000;
        int timesApplied = 0;
        public void OnUpdate(float diff)
        {
            if(Owner != null && Owner.TargetUnit != null)
            {
                _delayTime -= diff;
                if(_delayTime <= 0)
                {
                    //I got a feeling there's an equation for the damage, dealing the full damage seems too much.
                    //I haven't found any documentation on how the damage is processed though
                    Owner.TargetUnit.TakeDamage(Owner, Owner.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                    AddParticleTarget(Owner, Owner.TargetUnit, "SRU_Order_Laser_Turret_Tar", Owner.TargetUnit);

                    _delayTime = delayTime;
                }
            }
            if(GameTime() > reductimer && timesApplied < 30)
            {
                reductimer += 60.0f * 1000;
                delayTime -= 7.5f;
                timesApplied++;
            }
        }
    }
}

