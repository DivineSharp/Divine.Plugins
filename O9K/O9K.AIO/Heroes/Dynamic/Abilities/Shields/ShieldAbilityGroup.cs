﻿namespace O9K.AIO.Heroes.Dynamic.Abilities.Shields
{
    using System.Collections.Generic;
    using System.Linq;

    using AIO.Modes.Combo;

    using Base;

    using Blinks;

    using Core.Entities.Abilities.Base;
    using Core.Entities.Abilities.Base.Types;
    using Core.Entities.Units;
    using Core.Extensions;

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

    internal class ShieldAbilityGroup : OldAbilityGroup<IShield, OldShieldAbility>
    {
        public ShieldAbilityGroup(BaseHero baseHero)
            : base(baseHero)
        {
        }

        public BlinkAbilityGroup Blinks { get; set; }

        protected override HashSet<AbilityId> Ignored { get; } = new HashSet<AbilityId>
        {
            AbilityId.item_ethereal_blade,
            AbilityId.dark_willow_shadow_realm,
            AbilityId.puck_phase_shift,
            AbilityId.obsidian_destroyer_astral_imprisonment,
            AbilityId.bane_nightmare,
            AbilityId.chen_holy_persuasion,
            AbilityId.dazzle_shallow_grave,
            AbilityId.earth_spirit_petrify,
            AbilityId.life_stealer_assimilate,
            AbilityId.naga_siren_song_of_the_siren,
            AbilityId.necrolyte_sadist,
            AbilityId.omniknight_guardian_angel,
            AbilityId.oracle_false_promise,
            AbilityId.oracle_fates_edict,
            AbilityId.phoenix_supernova,
            AbilityId.pugna_decrepify,
            AbilityId.shadow_demon_disruption,
            AbilityId.slark_shadow_dance,
            AbilityId.vengefulspirit_nether_swap,
            AbilityId.weaver_time_lapse,
            AbilityId.winter_wyvern_cold_embrace,
            AbilityId.item_cyclone,
            AbilityId.item_sphere,
        };

        public override bool Use(Unit9 target, ComboModeMenu menu, params AbilityId[] except)
        {
            foreach (var ability in this.Abilities)
            {
                if (!ability.Ability.IsValid)
                {
                    continue;
                }

                if (except.Contains(ability.Ability.Id))
                {
                    continue;
                }

                if (!ability.CanBeCasted(ability.Shield.Owner, target, this.Blinks, menu))
                {
                    continue;
                }

                if (ability.Use(ability.Shield.Owner))
                {
                    return true;
                }
            }

            return false;
        }

        protected override bool IsIgnored(Ability9 ability)
        {
            if (base.IsIgnored(ability))
            {
                return true;
            }

            return ability is IDisable disable && disable.IsInvulnerability();
        }
    }
}