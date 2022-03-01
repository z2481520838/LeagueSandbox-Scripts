using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;



namespace Buffs
{
    internal class MordekaiserCOTGPet : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        float timeSinceLastTick;
        IAttackableUnit Unit;
        ISpell spell;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Unit = unit;
            spell = ownerSpell;
            //TODO: Set the Ghost stats here
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {

        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 30000.0)
            {
                Unit.TakeDamage(spell.CastInfo.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }
    }
}
//TODO: Make healing for POST MITIGATION damage
