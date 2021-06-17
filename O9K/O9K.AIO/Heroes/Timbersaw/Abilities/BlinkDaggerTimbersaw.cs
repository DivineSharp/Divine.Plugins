﻿namespace O9K.AIO.Heroes.Timbersaw.Abilities
{
    using System.Collections.Generic;
    using System.Linq;

    using AIO.Abilities;

    using Core.Entities.Abilities.Base;
    using Core.Helpers;
    using Core.Prediction.Data;

    using Divine;
    using Divine.Camera;
    using Divine.Entity;
    using Divine.Extensions;
    using Divine.Game;
    using Divine.GameConsole;

    using Divine.Input;
    using Divine.Log;
    using Divine.Map;

    using Divine.Modifier;
    using Divine.Numerics;
    using Divine.Orbwalker;
    using Divine.Order;
    using Divine.Particle;
    using Divine.Projectile;
    using Divine.Renderer;
    using Divine.Service;
    using Divine.Update;
    using Divine.Entity.Entities;
    using Divine.Entity.EventArgs;
    using Divine.Game.EventArgs;
    using Divine.GameConsole.Exceptions;
    using Divine.Input.EventArgs;
    using Divine.Map.Components;
    using Divine.Menu.Animations;
    using Divine.Menu.Components;

    using Divine.Menu.Helpers;

    using Divine.Menu.Styles;
    using Divine.Modifier.EventArgs;
    using Divine.Modifier.Modifiers;
    using Divine.Order.EventArgs;
    using Divine.Order.Orders;
    using Divine.Particle.Components;
    using Divine.Particle.EventArgs;
    using Divine.Particle.Particles;
    using Divine.Plugins.Humanizer;
    using Divine.Projectile.EventArgs;
    using Divine.Projectile.Projectiles;
    using Divine.Renderer.ValveTexture;
    using Divine.Entity.Entities.Abilities;
    using Divine.Entity.Entities.Components;
    using Divine.Entity.Entities.EventArgs;
    using Divine.Entity.Entities.Exceptions;
    using Divine.Entity.Entities.PhysicalItems;
    using Divine.Entity.Entities.Players;
    using Divine.Entity.Entities.Runes;
    using Divine.Entity.Entities.Trees;
    using Divine.Entity.Entities.Units;
    using Divine.Modifier.Modifiers.Components;
    using Divine.Modifier.Modifiers.Exceptions;
    using Divine.Order.Orders.Components;
    using Divine.Particle.Particles.Exceptions;
    using Divine.Projectile.Projectiles.Components;
    using Divine.Projectile.Projectiles.Exceptions;
    using Divine.Entity.Entities.Abilities.Components;
    using Divine.Entity.Entities.Abilities.Items;
    using Divine.Entity.Entities.Abilities.Spells;
    using Divine.Entity.Entities.Players.Components;
    using Divine.Entity.Entities.Runes.Components;
    using Divine.Entity.Entities.Units.Buildings;
    using Divine.Entity.Entities.Units.Components;
    using Divine.Entity.Entities.Units.Creeps;
    using Divine.Entity.Entities.Units.Heroes;
    using Divine.Entity.Entities.Units.Wards;
    using Divine.Entity.Entities.Abilities.Items.Components;
    using Divine.Entity.Entities.Abilities.Items.Neutrals;
    using Divine.Entity.Entities.Abilities.Spells.Abaddon;
    using Divine.Entity.Entities.Abilities.Spells.Components;
    using Divine.Entity.Entities.Units.Creeps.Neutrals;
    using Divine.Entity.Entities.Units.Heroes.Components;

    using Modes.Combo;

    using Divine.Numerics;

    using TargetManager;

    internal class BlinkDaggerTimbersaw : BlinkAbility
    {
        private Vector3 blinkPosition;

        public BlinkDaggerTimbersaw(ActiveAbility ability)
            : base(ability)
        {
        }

        public override bool ShouldCast(TargetManager targetManager)
        {
            if (!base.ShouldCast(targetManager))
            {
                return false;
            }

            if (this.Owner.HasModifier("modifier_shredder_timber_chain"))
            {
                return false;
            }

            return true;
        }

        public override bool ShouldConditionCast(TargetManager targetManager, IComboModeMenu menu, List<UsableAbility> usableAbilities)
        {
            var target = targetManager.Target;

            if (this.Owner.Distance(target) < 600)
            {
                return false;
            }

            var chain = usableAbilities.FirstOrDefault(x => x.Ability.Id == AbilityId.shredder_timber_chain);
            if (chain == null)
            {
                return true;
            }

            this.blinkPosition = target.Position;
            var whirlingDeath = usableAbilities.FirstOrDefault(x => x.Ability.Id == AbilityId.shredder_whirling_death);
            if (whirlingDeath != null)
            {
                var input = whirlingDeath.Ability.GetPredictionInput(target, targetManager.EnemyHeroes);
                input.Range += this.Ability.CastRange;
                input.CastRange = this.Ability.CastRange;
                input.SkillShotType = SkillShotType.Circle;
                var output = whirlingDeath.Ability.GetPredictionOutput(input);
                if (output.HitChance < HitChance.Low)
                {
                    return false;
                }

                this.blinkPosition = output.CastPosition;
            }

            if (this.Owner.Distance(this.blinkPosition) < this.Ability.CastRange)
            {
                return true;
            }

            return false;
        }

        public override bool UseAbility(TargetManager targetManager, Sleeper comboSleeper, bool aoe)
        {
            if (!this.Ability.UseAbility(this.blinkPosition))
            {
                return false;
            }

            this.Sleeper.Sleep(this.Ability.GetCastDelay(targetManager.Target));
            return true;
        }
    }
}