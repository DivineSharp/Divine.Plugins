﻿namespace O9K.AIO.Heroes.Mirana.Units
{
    using System;
    using System.Collections.Generic;

    using Abilities;
    using Abilities.Items;

    using Base;

    using Core.Entities.Abilities.Base;
    using Core.Entities.Metadata;
    using Core.Entities.Units;
    using Core.Helpers;

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

    using TargetManager;

    [UnitName(nameof(HeroId.npc_dota_hero_mirana))]
    internal class Mirana : ControllableUnit
    {
        private DisableAbility arrow;

        private DisableAbility atos;

        private BlinkAbility blink;

        private DebuffAbility diffusal;

        private ForceStaff leap;

        private BuffAbility manta;

        private SpeedBuffAbility phase;

        private NukeAbility starstorm;

        private DisableAbility gungir;

        public Mirana(Unit9 owner, MultiSleeper abilitySleeper, Sleeper orbwalkSleeper, ControllableUnitMenu menu)
            : base(owner, abilitySleeper, orbwalkSleeper, menu)
        {
            this.ComboAbilities = new Dictionary<AbilityId, Func<ActiveAbility, UsableAbility>>
            {
                { AbilityId.mirana_starfall, x => this.starstorm = new NukeAbility(x) },
                { AbilityId.mirana_arrow, x => this.arrow = new DisableAbility(x) },
                { AbilityId.mirana_leap, x => this.leap = new ForceStaff(x) },

                { AbilityId.item_phase_boots, x => this.phase = new SpeedBuffAbility(x) },
                { AbilityId.item_diffusal_blade, x => this.diffusal = new DebuffAbility(x) },
                { AbilityId.item_manta, x => this.manta = new BuffAbility(x) },
                { AbilityId.item_blink, x => this.blink = new BlinkAbility(x) },
                { AbilityId.item_swift_blink, x => this.blink = new BlinkAbility(x) },
                { AbilityId.item_arcane_blink, x => this.blink = new BlinkAbility(x) },
                { AbilityId.item_overwhelming_blink, x => this.blink = new BlinkAbility(x) },
                { AbilityId.item_rod_of_atos, x => this.atos = new DisableAbility(x) },
                { AbilityId.item_gungir, x => this.gungir = new DisableAbility(x) },
            };

            this.MoveComboAbilities.Add(AbilityId.mirana_leap, _ => this.leap);
        }

        public override bool Combo(TargetManager targetManager, ComboModeMenu comboModeMenu)
        {
            var abilityHelper = new AbilityHelper(targetManager, comboModeMenu, this);

            if (abilityHelper.UseAbility(this.atos))
            {
                return true;
            }
            
            if (abilityHelper.UseAbility(this.gungir))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.starstorm))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.diffusal))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.blink, this.Owner.GetAttackRange(), 350))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.arrow))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.leap, this.Owner.GetAttackRange(targetManager.Target) + 100, 800))
            {
                return true;
            }

            if (abilityHelper.UseForceStaffAway(this.leap, 200))
            {
                this.OrbwalkSleeper.Sleep(0.2f);
                this.ComboSleeper.Sleep(0.2f);
                this.leap.Sleeper.Sleep(2f);
                return true;
            }

            if (abilityHelper.UseAbility(this.manta, this.Owner.GetAttackRange()))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.phase))
            {
                return true;
            }

            return false;
        }

        protected override bool MoveComboUseBlinks(AbilityHelper abilityHelper)
        {
            if (base.MoveComboUseBlinks(abilityHelper))
            {
                return true;
            }

            if (abilityHelper.UseMoveAbility(this.leap))
            {
                return true;
            }

            return false;
        }
    }
}