﻿using System.Collections.Generic;
using O9K.AIO.Abilities;
using O9K.AIO.Modes.Combo;
using O9K.Core.Entities.Abilities.Base;
using O9K.Core.Entities.Abilities.Heroes.Invoker;
using O9K.Core.Entities.Units;

namespace O9K.AIO.Heroes.Invoker.Abilities
{
    internal class BlastAbility : InvokerAoeAbility
    {
        private readonly DeafeningBlast blast;

        public BlastAbility(ActiveAbility ability)
            : base(ability)
        {
            this.blast = (DeafeningBlast) ability;
        }
        
        public override bool ShouldConditionCast(TargetManager.TargetManager targetManager, IComboModeMenu menu, List<UsableAbility> usableAbilities)
        {
            // var target = targetManager.Target;
            // if (target == null)
            //     return false;
            //
            // if (target.IsStunned)
            //     return false;
            //
            // if (!target.HasModifier( "modifier_invoker_ice_wall_slow_debuff", "modifier_invoker_chaos_meteor_burn"))
            //     return false;
        
            return true;
        }
    }
}