namespace O9K.AIO.Heroes.Tinker.Abilities
{
    using System;
    using System.Collections.Generic;

    using AIO.Abilities;
    using AIO.Modes.Combo;

    using Core.Entities.Abilities.Base;

    using Divine;
    using Divine.SDK.Extensions;

    using O9K.AIO.Abilities.Menus;
    using O9K.AIO.Heroes.Pudge.Abilities;
    using O9K.Core.Helpers;

    using SharpDX;

    using TargetManager;

    internal class BlinkDaggerTinker : BlinkAbility
    {
        private Vector3 blinkPosition;

        public BlinkDaggerTinker(ActiveAbility ability)
            : base(ability)
        {
        }

        public override UsableAbilityMenu GetAbilityMenu(string simplifiedName)
        {
            return new BlinkDaggerTinkerMenu(this.Ability, simplifiedName);
        }

        public override bool CanBeCasted(TargetManager targetManager, bool channelingCheck, IComboModeMenu comboMenu)
        {
            if (!base.CanBeCasted(targetManager, channelingCheck, comboMenu))
            {
                return false;
            }

            return ShouldConditionCast(targetManager, comboMenu, new List<UsableAbility>());
        }

        public override bool ShouldConditionCast(TargetManager targetManager, IComboModeMenu comboMenu, List<UsableAbility> usableAbilities)
        {
            if (!base.ShouldConditionCast(targetManager, comboMenu, usableAbilities))
            {
                return false;
            }

            var menu = comboMenu.GetAbilitySettingsMenu<BlinkDaggerTinkerMenu>(this);

            var position = Owner.Position;
            var targetPosition = targetManager.Target.Position;
            var targetDistance = position.Distance(targetPosition);
            var mousePosition = GameManager.MousePosition;

            var distanceMousePosition = position.Distance(mousePosition);
            if (distanceMousePosition > menu.ActivateMouseDistance && targetDistance < menu.ActivateBasedEnemyDistance)
            {
                var castRange = Ability.CastRange;

                if (menu.EnableDistanceFromEnemy)
                {
                    blinkPosition = targetPosition.Extend(mousePosition, menu.DistanceFromEnemy);

                    if (position.Distance(blinkPosition) > castRange)
                    {
                        blinkPosition = position.Extend(mousePosition, Math.Min(distanceMousePosition, castRange));
                    }
                }
                else
                {
                    blinkPosition = position.Extend(mousePosition, Math.Min(distanceMousePosition, castRange));
                }

                return true;
            }

            return false;
        }

        public override bool UseAbility(TargetManager targetManager, Sleeper comboSleeper, bool aoe)
        {
            if (!base.UseAbility(targetManager, comboSleeper, blinkPosition))
            {
                return false;
            }

            return true;
        }
    }
}