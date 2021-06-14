﻿using System;
using System.Collections.Generic;
using Divine;
using O9K.AIO.Abilities;
using O9K.AIO.Abilities.Items;
using O9K.AIO.Heroes.Base;
using O9K.AIO.Heroes.Invoker.Abilities;
using O9K.AIO.Modes.Combo;
using O9K.Core.Entities.Abilities.Base;
using O9K.Core.Entities.Abilities.Heroes.Invoker.Helpers;
using O9K.Core.Entities.Metadata;
using O9K.Core.Entities.Units;
using O9K.Core.Helpers;
using O9K.Core.Logger;

namespace O9K.AIO.Heroes.Invoker.Units
{
    [UnitName(nameof(HeroId.npc_dota_hero_invoker))]
    internal class Invoker : ControllableUnit
    {
        private DisableAbility atos;

        private BlinkAbility blink;

        private EtherealBlade ethereal;

        private EulsScepterOfDivinity euls;

        private DisableAbility hex;

        private DebuffAbility shiva;

        private DebuffAbility veil;

        private UntargetableAbility quas;
        private UntargetableAbility wex;
        private UntargetableAbility exort;

        private UntargetableAbility refresher;

        private UntargetableAbility refresherShard;
        private DisableAbility orchid;
        private Nullifier nullifier;
        private Bloodthorn bloodthorn;
        private ShieldAbility bkb;

        private AoeAbility emp;
        private TornadoAbility tornado;
        private MeteorAbility meteor;
        private SunStrikeAbility sunStrike;
        private UntargetableAbility iceWall;
        private BlastAbility blast;
        private DisableAbility coldSnap;
        private UntargetableAbility invoke;

        // private Sleeper cycloneCastedSleeper = new Sleeper();
        private Sleeper meteorLaunched = new Sleeper();

        // private Sleeper blastLauched = new Sleeper();
        private BuffAbility alacrity;
        private UntargetableAbility forges;

        public Invoker(Unit9 owner, MultiSleeper abilitySleeper, Sleeper orbwalkSleeper, ControllableUnitMenu menu)
            : base(owner, abilitySleeper, orbwalkSleeper, menu)
        {
            ComboAbilities = new Dictionary<AbilityId, Func<ActiveAbility, UsableAbility>>
            {
                // {
                //     AbilityId.leshrac_split_earth, x =>
                //         {
                //             this.splitEarth = new SplitEarth(x);
                //             if (this.lightning != null)
                //             {
                //                 this.splitEarth.Storm = this.lightning;
                //             }
                //
                //             this.splitEarth.FailSafe = this.FailSafe;
                //             return this.splitEarth;
                //         }
                // },
                // { AbilityId.leshrac_diabolic_edict, x => this.diabolic = new DiabolicEdict(x) },
                // {
                //     AbilityId.leshrac_lightning_storm, x =>
                //         {
                //             this.lightning = new NukeAbility(x);
                //             if (this.splitEarth != null)
                //             {
                //                 this.splitEarth.Storm = this.lightning;
                //             }
                //
                //             return this.lightning;
                //         }
                // },
                // { AbilityId.leshrac_pulse_nova, x => this.nova = new PulseNova(x) },

                {AbilityId.item_ethereal_blade, x => ethereal = new EtherealBlade(x)},
                {AbilityId.item_veil_of_discord, x => veil = new DebuffAbility(x)},
                {AbilityId.item_orchid, x => this.orchid = new DisableAbility(x)},
                {AbilityId.item_nullifier, x => this.nullifier = new Nullifier(x)},
                {AbilityId.item_bloodthorn, x => this.bloodthorn = new Bloodthorn(x)},
                {AbilityId.item_black_king_bar, x => this.bkb = new ShieldAbility(x)},
                {AbilityId.item_blink, x => blink = new BlinkAbility(x)},
                {AbilityId.item_swift_blink, x => blink = new BlinkAbility(x)},
                {AbilityId.item_arcane_blink, x => blink = new BlinkAbility(x)},
                {AbilityId.item_overwhelming_blink, x => blink = new BlinkAbility(x)},
                {AbilityId.item_cyclone, x => euls = new EulsScepterOfDivinity(x)},
                {AbilityId.item_sheepstick, x => hex = new DisableAbility(x)},
                {AbilityId.item_shivas_guard, x => shiva = new DebuffAbility(x)},
                {AbilityId.item_rod_of_atos, x => atos = new DisableAbility(x)},

                {AbilityId.invoker_quas, x => quas = new UntargetableAbility(x)},
                {AbilityId.invoker_wex, x => wex = new UntargetableAbility(x)},
                {AbilityId.invoker_exort, x => exort = new UntargetableAbility(x)},

                {AbilityId.invoker_cold_snap, x => this.coldSnap = new DisableAbility(x)},
                {AbilityId.invoker_emp, x => emp = new AoeAbility(x)},
                {AbilityId.invoker_tornado, x => tornado = new TornadoAbility(x)},
                {AbilityId.invoker_chaos_meteor, x => meteor = new MeteorAbility(x)},
                {AbilityId.invoker_sun_strike, x => sunStrike = new SunStrikeAbility(x)},
                {AbilityId.invoker_ice_wall, x => iceWall = new UntargetableAbility(x)},
                {AbilityId.item_refresher, x => this.refresher = new UntargetableAbility(x)},
                {AbilityId.invoker_deafening_blast, x => this.blast = new BlastAbility(x)},
                {AbilityId.item_refresher_shard, x => this.refresherShard = new UntargetableAbility(x)},
                {AbilityId.invoker_alacrity, x => this.alacrity = new BuffAbility(x)},
                {AbilityId.invoker_forge_spirit, x => this.forges = new UntargetableAbility(x)},
                {AbilityId.invoker_invoke, x => this.invoke = new UntargetableAbility(x)},
            };

            // this.MoveComboAbilities.Add(AbilityId.leshrac_split_earth, _ => this.splitEarth);
        }

        public override bool Combo(TargetManager.TargetManager targetManager, ComboModeMenu comboModeMenu)
        {
            var abilityHelper = new AbilityHelper(targetManager, comboModeMenu, this);
            if (abilityHelper.UseAbility(blink, 550, 350))
            {
                return true;
            }

            if (abilityHelper.UseAbility(veil))
            {
                return true;
            }

            if (abilityHelper.UseAbility(shiva))
            {
                return true;
            }

            if (abilityHelper.UseAbility(nullifier))
            {
                return true;
            }

            if (abilityHelper.UseAbility(ethereal))
            {
                return true;
            }

            if (abilityHelper.UseAbility(orchid))
            {
                return true;
            }

            if (abilityHelper.UseAbility(bloodthorn))
            {
                return true;
            }

            if (abilityHelper.UseAbility(hex))
            {
                return true;
            }

            if (abilityHelper.UseAbility(atos))
            {
                return true;
            }

            var target = targetManager.Target;

            if (target == null)
                return false;

            if (!meteorLaunched.IsSleeping && abilityHelper.UseAbilityIfAny(this.euls, sunStrike, meteor, iceWall, emp))
            {
                ComboSleeper.Sleep(0.2f);
                return true;
            }

            if (!meteorLaunched.IsSleeping && abilityHelper.UseAbilityIfAny(this.tornado, sunStrike, meteor, iceWall, emp))
            {
                ComboSleeper.Sleep(tornado.Ability.GetHitTime(target));
                return true;
            }

            var duration = target.GetInvulnerabilityDuration();
            if (duration > 1.8)
            {
                if (abilityHelper.CanBeCasted(sunStrike, false, false))
                {
                    if (abilityHelper.ReInvokeIfOnLastPosition(sunStrike, meteor, blast))
                    {
                        ComboSleeper.Sleep(0.1f);
                        return true;
                    }

                    if (!abilityHelper.IsInvoked(sunStrike))
                    {
                        if (abilityHelper.SafeInvoke(sunStrike, meteor, blast))
                        {
                            ComboSleeper.Sleep(0.1f);
                            return true;
                        }
                    }

                    ComboSleeper.Sleep(0.1f);
                    return false;
                }
            }

            if (duration > 1.4)
            {
                if (abilityHelper.CanBeCasted(meteor, false, false))
                {
                    if (abilityHelper.ReInvokeIfOnLastPosition(meteor, sunStrike))
                    {
                        ComboSleeper.Sleep(0.2f);
                        return true;
                    }
                    
                    if (!abilityHelper.IsInvoked(blast) && abilityHelper.CanBeCasted(blast, false, false))
                    {
                        if (abilityHelper.IsInvoked(meteor) && abilityHelper.CanBeCasted(meteor, false, false))
                        {
                            if (abilityHelper.SafeInvoke(blast, meteor))
                            {
                                ComboSleeper.Sleep(0.2f);
                                return true;
                            }

                            ComboSleeper.Sleep(0.2f);
                            return false;
                        }
                    }

                    ComboSleeper.Sleep(0.1f);
                    return false;
                }
            }

            if (abilityHelper.UseAbilityIfCondition(meteor))
            {
                meteorLaunched.Sleep(meteor.Ability.GetHitTime(LastTarget));
                return true;
            }

            
            if (abilityHelper.UseAbilityIfCondition(blast))
            {
                return true;
            }

            if (abilityHelper.UseAbilityIfCondition(sunStrike))
            {
                return true;
            }

            if (abilityHelper.UseInvokedAbilityIfCondition(emp))
            {
                return true;
            }

            if (abilityHelper.UseAbility(tornado))
            {
                ComboSleeper.Sleep(0.2f);
                return true;
            }


            if (abilityHelper.UseAbilityIfCondition(emp))
            {
                ComboSleeper.Sleep(0.3f);
                return true;
            }

            if (abilityHelper.UseAbility(coldSnap))
            {
                ComboSleeper.Sleep(0.3f);
                return true;
            }

            if (abilityHelper.UseAbility(alacrity))
            {
                ComboSleeper.Sleep(0.3f);
                return true;
            }

            if (abilityHelper.UseAbility(forges))
            {
                ComboSleeper.Sleep(0.3f);
                return true;
            }

            return false;
        }

        protected override bool MoveComboUseDisables(AbilityHelper abilityHelper)
        {
            if (base.MoveComboUseDisables(abilityHelper))
            {
                return true;
            }
            
            return false;
        }
    }
}