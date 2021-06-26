namespace O9K.AIO.Heroes.Tinker.Abilities
{
    using System.Collections.Generic;
    using System.Linq;

    using AIO.Abilities;

    using Core.Entities.Abilities.Base;

    using O9K.AIO.Modes.Combo;

    using TargetManager;

    internal class Rearm : UntargetableAbility
    {
        public Rearm(ActiveAbility ability)
            : base(ability)
        {
        }

        public override bool ShouldConditionCast(TargetManager targetManager, IComboModeMenu menu, List<UsableAbility> usableAbilities)
        {
            return !usableAbilities.Any(x => x.CanBeCasted(targetManager, false, menu));
        }
    }
}