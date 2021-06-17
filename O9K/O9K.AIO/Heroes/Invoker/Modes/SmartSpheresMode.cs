﻿using System.Linq;
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
using O9K.AIO.Heroes.Base;
using O9K.AIO.Modes.Base;
using O9K.Core.Helpers;
using O9K.Core.Managers.Menu.EventArgs;
using O9K.Core.Managers.Menu.Items;

namespace O9K.AIO.Heroes.Invoker.Modes
{
    internal class SmartSpheresMode : BaseMode
    {
        private readonly Sleeper sleeper = new();
        private readonly Sleeper inChanging = new();
        private readonly MultiSleeper<string> multySleeper = new();
        private readonly SmartSpheresModeModeMenu modeMenu;
        private MenuSlider HpSlider => modeMenu.hpSlider;

        public SmartSpheresMode(BaseHero baseHero, SmartSpheresModeModeMenu menu)
            : base(baseHero)
        {
            modeMenu = menu;
        }

        public void Disable()
        {
            OrderManager.OrderAdding -= PlayerOnOnExecuteOrder;
        }

        public override void Dispose()
        {
            OrderManager.OrderAdding -= PlayerOnOnExecuteOrder;
            modeMenu.Enabled.ValueChange -= EnabledOnValueChanged;
            base.Dispose();
        }

        private void EnabledOnValueChanged(object sender, SwitcherEventArgs e)
        {
            if (e.NewValue)
                OrderManager.OrderAdding += PlayerOnOnExecuteOrder;
            else
                OrderManager.OrderAdding -= PlayerOnOnExecuteOrder;
        }

        public void Enable()
        {
            modeMenu.Enabled.ValueChange += EnabledOnValueChanged;
        }

        private void PlayerOnOnExecuteOrder(OrderAddingEventArgs args)
        {
            if (Owner.Hero.IsInvisible)
            {
                return;
            }

            var order = args.Order.Type;
            if (order == OrderType.Cast && !args.IsCustom)
            {
                var abilityId = args.Order.Ability.Id;
                if (abilityId is AbilityId.invoker_quas or AbilityId.invoker_wex or AbilityId.invoker_exort or AbilityId.invoker_invoke or AbilityId.invoker_ghost_walk)
                {
                    sleeper.Sleep(1.5f);
                    multySleeper.Sleep("attack", 1.250f);
                    multySleeper.Sleep("move", 1.250f);
                }
            }

            if (sleeper.IsSleeping || Owner.Hero.IsSilenced)
                return;
            if (order == OrderType.AttackPosition || order == OrderType.AttackTarget)
            {
                if (multySleeper.IsSleeping("attack"))
                    return;
                multySleeper.Sleep("attack", .250f);
                var countOfModifiers =
                    Owner.Hero.BaseModifiers.Count(x => x.Name == $"modifier_{AbilityId.invoker_exort}_instance");
                if (countOfModifiers >= 3) return;
                var activeSphereForAttack = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_exort);
                if (activeSphereForAttack?.CanBeCasted() == true)
                {
                    for (var i = countOfModifiers; i < 3; i++) activeSphereForAttack.BaseAbility.Cast();
                    inChanging.Sleep(.250f);
                }
            }
            else if (order is OrderType.MovePosition or OrderType.MoveTarget)
            {
                if (multySleeper.IsSleeping("move"))
                    return;
                multySleeper.Sleep("move", .250f);

                var activeSphereForMove = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_wex);
                if (Owner.Hero.HealthPercentage / 100f <= HpSlider.Value / 100f)
                {
                    activeSphereForMove = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_quas);
                }

                if (activeSphereForMove is not null && activeSphereForMove.CanBeCasted())
                {
                    var countOfModifiers =
                        Owner.Hero.BaseModifiers.Count(x => x.Name == $"modifier_{activeSphereForMove.Id}_instance");
                    if (countOfModifiers >= 3) return;
                    for (var i = countOfModifiers; i < 3; i++) activeSphereForMove.BaseAbility.Cast();
                    inChanging.Sleep(.250f);
                }
            }
        }
    }
}