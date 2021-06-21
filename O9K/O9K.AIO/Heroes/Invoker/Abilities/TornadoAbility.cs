using System;
using System.Collections.Generic;
using System.Linq;
using O9K.AIO.Abilities;
using O9K.AIO.Modes.Combo;
using O9K.Core.Entities.Abilities.Base;
using O9K.Core.Entities.Abilities.Heroes.Invoker;
using O9K.Core.Entities.Units;
using O9K.Core.Logger;

namespace O9K.AIO.Heroes.Invoker.Abilities
{
    internal class TornadoAbility : InvokerAoeAbility
    {
        private readonly Tornado tornado;

        public TornadoAbility(ActiveAbility ability)
            : base(ability)
        {
            this.tornado = (Tornado) ability;
        }
        
        public override bool ShouldConditionCast(TargetManager.TargetManager targetManager, IComboModeMenu menu, List<UsableAbility> usableAbilities)
        {
            var target = targetManager.Target;
            if (target == null)
                return false;

            if (target.IsStunned)
                return false;

            var hasAnyModifier = target.HasModifier("modifier_invoker_deafening_blast_knockback",
                "modifier_invoker_ice_wall_slow_debuff", "modifier_invoker_chaos_meteor_burn");
            
            if (hasAnyModifier)
                return false;

            if (usableAbilities.All(x => !x.CanBeCasted(targetManager, false, menu)))
            {
                return false;
            }
            
            return true;
        }
        
        
        protected override bool ChainStun(Unit9 target, bool invulnerability)
        {
            var immobile = target.GetInvulnerabilityDuration();
            if (immobile <= 0)
            {
                return false;
            }
            var hitTime = this.Ability.GetCastDelay(target) + this.Ability.ActivationDelay;
            // {
            //     hitTime -= 0.1f;
            // }
            // Console.WriteLine($"immobile {immobile} hitTime {hitTime} {Ability.DisplayName}");

            return hitTime > immobile;
        }
    }
}