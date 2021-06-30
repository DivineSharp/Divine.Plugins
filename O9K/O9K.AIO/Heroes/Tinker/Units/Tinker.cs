namespace O9K.AIO.Heroes.Tinker.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Base;

    using Core.Entities.Abilities.Base;
    using Core.Entities.Metadata;
    using Core.Entities.Units;
    using Core.Helpers;

    using Divine;

    using Modes.Combo;

    using O9K.AIO.Abilities;
    using O9K.AIO.Abilities.Items;
    using O9K.AIO.Heroes.Tinker.Abilities;

    using TargetManager;

    [UnitName(nameof(HeroId.npc_dota_hero_tinker))]
    internal class Tinker : ControllableUnit
    {
        private NukeAbility laser;

        private NukeAbility heatSeeking;

        private NukeAbility march;

        private Rearm rearm;

        private ShieldAbility glimmer;

        private ShieldAbility ghost;

        private BlinkAbility blink;

        private HexTinker hex;

        private DisableAbility orchid;

        private DisableAbility bloodthorn;

        private Nullifier nullifier;

        private DisableAbility atos;

        private DebuffAbility veil;

        private EtherealTinker ethereal;

        private DebuffAbility shiva;

        private NukeAbility dagon;

        public Tinker(Unit9 owner, MultiSleeper abilitySleeper, Sleeper orbwalkSleeper, ControllableUnitMenu menu)
            : base(owner, abilitySleeper, orbwalkSleeper, menu)
        {
            this.ComboAbilities = new Dictionary<AbilityId, Func<ActiveAbility, UsableAbility>>
            {
                { AbilityId.tinker_laser, x => this.laser = new NukeAbility(x) },
                { AbilityId.tinker_heat_seeking_missile, x => this.heatSeeking = new NukeAbility(x) },
                { AbilityId.tinker_march_of_the_machines, x => this.march = new NukeAbility(x) },
                { AbilityId.tinker_rearm, x => this.rearm = new Rearm(x) },

                { AbilityId.item_glimmer_cape, x => this.glimmer = new ShieldAbility(x) },
                { AbilityId.item_ghost, x => this.ghost = new ShieldAbility(x) },
                { AbilityId.item_blink, x => this.blink = new BlinkDaggerTinker(x) },
                { AbilityId.item_swift_blink, x => this.blink = new BlinkDaggerTinker(x) },
                { AbilityId.item_arcane_blink, x => this.blink = new BlinkDaggerTinker(x) },
                { AbilityId.item_overwhelming_blink, x => this.blink = new BlinkDaggerTinker(x) },
                { AbilityId.item_sheepstick, x => this.hex = new HexTinker(x) },
                { AbilityId.item_orchid, x => this.orchid = new DisableAbility(x) },
                { AbilityId.item_bloodthorn, x => this.bloodthorn = new DisableAbility(x) },
                { AbilityId.item_nullifier, x => this.nullifier = new Nullifier(x) },
                { AbilityId.item_rod_of_atos, x => this.atos = new DisableAbility(x) },
                { AbilityId.item_veil_of_discord, x => this.veil = new DebuffAbility(x) },
                { AbilityId.item_ethereal_blade, x => this.ethereal = new EtherealTinker(x) },
                { AbilityId.item_shivas_guard, x => this.shiva = new DebuffAbility(x) },
                { AbilityId.item_dagon_5, x => this.dagon = new NukeAbility(x) },
            };
        }

        public override bool Combo(TargetManager targetManager, ComboModeMenu comboModeMenu)
        {
            var abilityHelper = new AbilityHelper(targetManager, comboModeMenu, this);

            if (OrderManager.Orders.Count() != 0)
            {
                return false;
            }

            if (abilityHelper.UseAbility(this.glimmer, 900, false))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.ghost, 900, false))
            {
                return true;
            }

            if (abilityHelper.UseAbilityIfCondition(this.blink))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.hex))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.orchid))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.bloodthorn))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.nullifier))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.atos))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.veil))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.ethereal))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.shiva))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.heatSeeking))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.dagon))
            {
                return true;
            }

            if (abilityHelper.UseAbility(this.laser))
            {
                return true;
            }

            if (abilityHelper.UseAbilityIfCondition(rearm, glimmer, ghost, blink, hex, orchid, bloodthorn, nullifier, atos, veil, ethereal, shiva, heatSeeking, dagon, laser))
            {
                return true;
            }

            GGGG

            return false;
        }

        protected override bool MoveComboUseBlinks(AbilityHelper abilityHelper)
        {
            if (base.MoveComboUseBlinks(abilityHelper))
            {
                return true;
            }

            /*if (abilityHelper.UseMoveAbility(this.timberChainBlink)) // TODO rocco
            {
                return true;
            }*/

            return false;
        }
    }
}