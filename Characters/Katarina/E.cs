using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class KatarinaE : ISpellScript
    {
        private IAttackableUnit Target;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            PlayAnimation(owner, "Spell2");
            if (target.Team != owner.Team)
            {
                float AP = owner.Stats.AbilityPower.Total * 0.4f;
                float damage = 45f + 25 * spell.CastInfo.SpellLevel + AP;
                var MarkAP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.15f;
                float MarkDamage = 15f * (owner.GetSpell("KatarinaQ").CastInfo.SpellLevel) + MarkAP;

                if (target.HasBuff("KatarinaQMark"))
                {
                    target.TakeDamage(owner, MarkDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
                    RemoveBuff(target, "KatarinaQMark");
                }

                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(owner, null, "katarina_shadowStep_tar.troy", Target);
            }
            AddParticleTarget(owner, null, "katarina_shadowStep_cas.troy", owner);

            ForceMovement(owner, "Spell2", Vector2.Zero, 20, 20, 0.3f, 20);
            TeleportTo(owner, Target.Position.X, Target.Position.Y);
            AddBuff("KatarinaEReduction", 1.5f, 1, spell, owner, owner);
            PlayAnimation(owner, "Spell3", 1f);
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