﻿using Divine.Order;
using O9K.AIO.Heroes.Tinker.Abilities;

namespace O9K.AIO.Heroes.ArcWarden.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abilities;
    using AIO.Abilities;
    using AIO.Abilities.Items;
    using Base;
    using Core.Entities.Abilities.Base;
    using Core.Entities.Metadata;
    using Core.Entities.Units;
    using Core.Helpers;
    using Core.Managers.Entity;
    using Divine.Game;
    using Divine.Entity.Entities.Abilities.Components;
    using Divine.Entity.Entities.Units.Heroes.Components;
    using Modes.Combo;
    using TargetManager;

    [UnitName(nameof(HeroId.npc_dota_hero_arc_warden))]
    internal class ArcWarden : ControllableUnit
    {
        private readonly Sleeper moveSleeper = new Sleeper();

        private DisableAbilityArcWarden abyssal;

        private DisableAbilityArcWarden bloodthorn;

        private DebuffAbility diffusal;

        private NukeAbility spark;

        private DebuffAbility flux;

        private MagneticFieldAbility magneticFieldAbility;

        private BuffAbility tempestDouble;

        private BuffAbility manta;

        private Nullifier nullifier;

        private DisableAbilityArcWarden hex;

        private DisableAbilityArcWarden orchid;

        private BlinkAbility blink;

        private HurricanePike pike;
        private ForceStaff force;


        private ShieldAbility mjollnir;

        private DisableAbilityArcWarden atos;
        private DisableAbilityArcWarden gungir;

        private EtherealBlade ethereal;
        private NukeAbility dagon;

        private BuffAbility shadow;
        private BuffAbility silver;


        public ArcWarden(Unit9 owner, MultiSleeper abilitySleeper, Sleeper orbwalkSleeper, ControllableUnitMenu menu)
            : base(owner, abilitySleeper, orbwalkSleeper, menu)
        {
            this.ComboAbilities = new Dictionary<AbilityId, Func<ActiveAbility, UsableAbility>>
            {
                {AbilityId.arc_warden_spark_wraith, x => this.spark = new NukeAbility(x)},
                {AbilityId.arc_warden_flux, x => this.flux = new DebuffAbility(x)},
                {AbilityId.arc_warden_magnetic_field, x => this.magneticFieldAbility = new MagneticFieldAbility(x)},
                {AbilityId.arc_warden_tempest_double, x => this.tempestDouble = new BuffAbility(x)},


                {AbilityId.item_rod_of_atos, x => this.atos = new DisableAbilityArcWarden(x)},
                {AbilityId.item_gungir, x => this.gungir = new DisableAbilityArcWarden(x)},
                {AbilityId.item_diffusal_blade, x => this.diffusal = new DebuffAbility(x)},
                {AbilityId.item_abyssal_blade, x => this.abyssal = new DisableAbilityArcWarden(x)},
                {AbilityId.item_manta, x => this.manta = new BuffAbility(x)},
                {AbilityId.item_bloodthorn, x => this.bloodthorn = new DisableAbilityArcWarden(x)},
                {AbilityId.item_orchid, x => this.orchid = new DisableAbilityArcWarden(x)},
                {AbilityId.item_nullifier, x => this.nullifier = new Nullifier(x)},
                {AbilityId.item_sheepstick, x => this.hex = new DisableAbilityArcWarden(x)},
                {AbilityId.item_mjollnir, x => this.mjollnir = new ShieldAbility(x)},


                {AbilityId.item_blink, x => this.blink = new BlinkDaggerArcWarden(x)},
                {AbilityId.item_swift_blink, x => this.blink = new BlinkDaggerArcWarden(x)},
                {AbilityId.item_arcane_blink, x => this.blink = new BlinkDaggerArcWarden(x)},
                {AbilityId.item_overwhelming_blink, x => this.blink = new BlinkDaggerArcWarden(x)},
                {AbilityId.item_hurricane_pike, x => this.pike = new HurricanePike(x)},
                {AbilityId.item_force_staff, x => this.force = new ForceStaff(x)},

                {AbilityId.item_ethereal_blade, x => this.ethereal = new EtherealBlade(x)},
                {AbilityId.item_dagon_5, x => this.dagon = new NukeAbility(x)},

                {AbilityId.item_silver_edge, x => this.silver = new BuffAbility(x)},
                {AbilityId.item_invis_sword, x => this.shadow = new BuffAbility(x)},
            };
        }

        public override bool Combo(TargetManager targetManager, ComboModeMenu comboModeMenu)
        {
            var abilityHelper = new AbilityHelper(targetManager, comboModeMenu, this);
            var isMainHero = (this.Owner == EntityManager9.Owner);

            if (OrderManager.Orders.Count() != 0)
            {
                return false;
            }


            if (abilityHelper.UseAbility(this.blink))
            {
                return true;
            }

            if (!isMainHero && abilityHelper.UseAbility(this.force, 500, 300) &&
                !this.Owner.HasModifier(magneticFieldAbility.Shield.ShieldModifierName))
            {
                return true;
            }

            if (!isMainHero && abilityHelper.UseAbility(this.force, 500, 300) &&
                !this.Owner.HasModifier(magneticFieldAbility.Shield.ShieldModifierName))
            {
                return true;
            }

            if (isMainHero && abilityHelper.CanBeCasted(this.pike) && !this.MoveSleeper.IsSleeping &&
                (!this.Owner.HasModifier(magneticFieldAbility.Shield.ShieldModifierName) || this.Owner.Health < 1000))
            {
                if (this.pike.UseAbilityOnTarget(targetManager, this.ComboSleeper))
                {
                    return true;
                }
            }

            if (abilityHelper.UseAbility(this.atos))
            {
                return true;
            }

            if (!Divine.Helpers.MultiSleeper<string>.Sleeping("ArcWardenMagneticRootDisable") &&
                abilityHelper.UseAbility(this.gungir))
            {
                Divine.Helpers.MultiSleeper<string>.Sleep("ArcWardenMagneticRootDisable", 2000);
                return true;
            }

            if (!Divine.Helpers.MultiSleeper<string>.Sleeping("ArcWardenMagneticRootDisable") &&
                abilityHelper.UseAbility(this.abyssal))
            {
                Divine.Helpers.MultiSleeper<string>.Sleep("ArcWardenMagneticRootDisable", 2000);
                return true;
            }

            if (abilityHelper.UseAbility(hex))
            {
                return true;
            }

            if (!Divine.Helpers.MultiSleeper<string>.Sleeping("ArcWardenMagneticNullifier") &&
                abilityHelper.UseAbility(this.nullifier))
            {
                Divine.Helpers.MultiSleeper<string>.Sleep("ArcWardenMagneticNullifier", 2000);
                return true;
            }

            if (abilityHelper.UseAbility(this.bloodthorn))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.orchid))
            {
                return true;
            }

            if (!Divine.Helpers.MultiSleeper<string>.Sleeping("ArcWardenMagneticSilverEdge") &&
                abilityHelper.UseAbility(this.silver))
            {
                Divine.Helpers.MultiSleeper<string>.Sleep("ArcWardenMagneticSilverEdge", 5000);
                ComboSleeper.Sleep(0.5f);
                return true;
            }

            if (!isMainHero && abilityHelper.UseAbility(this.shadow))
            {
                ComboSleeper.Sleep(0.5f);
                return true;
            }

            if (abilityHelper.UseAbility(this.flux))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.tempestDouble))
            {
                return true;
            }

            if (!Divine.Helpers.MultiSleeper<string>.Sleeping("ArcWardenMagneticField") &&
                abilityHelper.UseAbility(this.magneticFieldAbility))
            {
                Divine.Helpers.MultiSleeper<string>.Sleep("ArcWardenMagneticField", 550);
                return true;
            }

            if (abilityHelper.UseAbility(this.diffusal))
            {
                return true;
            }
            // fast nukes section before sparks

            if (abilityHelper.UseAbility(this.ethereal))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.dagon))
            {
                return true;
            }


            // end of fast nukes

            if (abilityHelper.UseAbility(this.spark))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.manta, this.Owner.GetAttackRange()))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.mjollnir, 600))
            {
                return true;
            }


            return false;
        }

        protected override bool Attack(Unit9 target, ComboModeMenu comboMenu)
        {
            if (this.Owner.Distance(target) > this.Owner.GetAttackRange(target, 200))
            {
                if (this.Owner.BaseUnit.Attack(target.BaseUnit))
                {
                    this.AttackSleeper.Sleep(0.5f);
                    this.MoveSleeper.Sleep(0.5f);
                    return true;
                }
            }

            return base.Attack(target, comboMenu);
        }

        protected override bool MoveComboUseBlinks(AbilityHelper abilityHelper)
        {
            if (this.moveSleeper)
            {
                return false;
            }

            if (base.MoveComboUseBlinks(abilityHelper))
            {
                return true;
            }

            if (this.Owner.IsAttacking)
            {
                this.OrbwalkSleeper.Reset();
                this.MoveSleeper.Reset();
                return true;
            }


            return false;
        }
    }
}