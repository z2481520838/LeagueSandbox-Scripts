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
    class KatarinaR : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_DEHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();


        private IParticle _createdParticle;
        private IChampion owner;
        private IBuff sourceBuff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            IChampion champion = unit as IChampion;
            owner = champion;
            sourceBuff = buff;

            ApiEventManager.OnUnitUpdateMoveOrder.AddListener(this, champion, OnUpdateMoveOrder, true);
        }
        public void OnUpdateMoveOrder(IObjAiBase unit, OrderType order)
        {
            var buff = unit.GetBuffWithName("KatarinaR");
            if (buff != null)
            {
                if (order != OrderType.Hold && order != OrderType.Stop)
                {
                    buff.DeactivateBuff();
                }
                else
                {
                    // After the callback ends, it will remove the listener, so we make a new one before the callback ends.
                    ApiEventManager.OnUnitUpdateMoveOrder.AddListener(this, unit, OnUpdateMoveOrder, true);
                }
            }
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
