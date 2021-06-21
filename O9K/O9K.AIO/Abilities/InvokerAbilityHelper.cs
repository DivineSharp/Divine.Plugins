using System.Collections.Generic;
using System.Linq;
using Divine;
using O9K.AIO.Heroes.Base;
using O9K.AIO.Modes.Combo;
using O9K.Core.Entities.Abilities.Base;
using O9K.Core.Entities.Abilities.Heroes.Invoker.Helpers;

namespace O9K.AIO.Abilities
{
    internal class InvokerAbilityHelper : AbilityHelper
    {
        public InvokerAbilityHelper(TargetManager.TargetManager targetManager, IComboModeMenu comboModeMenu, ControllableUnit controllableUnit) : base(targetManager, comboModeMenu, controllableUnit)
        {
            
        }
        
        public bool UseInvokedAbilityIfCondition(UsableAbility ability, params UsableAbility[] checkAbilities)
        {
            if (!this.CanBeCasted(ability))
            {
                return false;
            }

            if (!IsInvoked(ability))
            {
                return false;
            }

            if (!ability.ShouldConditionCast(
                this.TargetManager,
                this.menu,
                checkAbilities.Where(x => this.CanBeCasted(x, false, false)).ToList()))
            {
                return false;
            }

            return ability.UseAbility(this.TargetManager, this.comboSleeper, true);
        }

        public bool IsInvoked(UsableAbility ability)
        {
            if (ability.Ability is IInvokableAbility invokable)
            {
                return invokable.IsInvoked;
            }

            return false;
        }

        public bool Invoke(UsableAbility ability)
        {
            if (ability.Ability is not IInvokableAbility ss) return false;
            return ss.Invoke(null, false, false, true);
        }

        public List<Ability9> GetInvokedAbilities(bool canBeCasted = true)
        {
            var abilities = this.unit.Abilities.Where(x =>
            {
                var isInvoked = (x as IInvokableAbility)?.IsInvoked;
                return isInvoked != null && (bool) isInvoked && (!canBeCasted || x.CanBeCasted());
            }).ToList();

            return abilities;
        }

        public bool IsInvokedOnLastSlot(UsableAbility ability)
        {
            if (ability.Ability is IInvokableAbility invokable)
            {
                return invokable.GetAbilitySlot == AbilitySlot.Slot_5;
            }

            return false;
        }

        public bool ReInvokeIfOnLastPosition(UsableAbility ability, params UsableAbility[] ignoredOnFirstSlot)
        {
            if (IsInvokedOnLastSlot(ability))
            {
                foreach (var usableAbility in ignoredOnFirstSlot.Where(x => x != null))
                {
                    if (usableAbility.Ability is IInvokableAbility usable)
                    {
                        if (usable.GetAbilitySlot == AbilitySlot.Slot_4 && usableAbility.Ability.CanBeCasted())
                        {
                            return false;
                        }
                    }
                }

                if (ability.Ability is IInvokableAbility ss)
                {
                    return ss.Invoke(null, false, false, true);
                }
            }

            return false;
        }

        public bool SafeInvoke(UsableAbility ability, params UsableAbility[] ignoredOnInvokedSlots)
        {
            if (IsInvoked(ability))
            {
                return true;
            }
            foreach (var usableAbility in ignoredOnInvokedSlots)
            {
                if (usableAbility.Ability is not IInvokableAbility usable) continue;
                if (usable.GetAbilitySlot != AbilitySlot.Slot_5 || !usableAbility.Ability.CanBeCasted()) continue;
                var anyOnFirst = ignoredOnInvokedSlots.Any(x =>
                {
                    if (x.Ability is not IInvokableAbility usable2) return false;
                    return usable2.GetAbilitySlot == AbilitySlot.Slot_4 && usableAbility.Ability.CanBeCasted();
                });
                if (!anyOnFirst) continue;
                return false;
            }

            if (ability.Ability is not IInvokableAbility ss) return false;
            return ss.Invoke(null, false, false, true);
        }

        public bool CanBeInvoked(UsableAbility ability)
        {
            if (ability.Ability is IInvokableAbility invokable)
            {
                return invokable.CanBeInvoked;
            }

            return false;
        }
    }
}