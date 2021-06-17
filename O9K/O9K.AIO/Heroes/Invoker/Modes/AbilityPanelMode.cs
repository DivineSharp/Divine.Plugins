using System;
using System.Globalization;
using System.Linq;
using Divine;
using Divine.SDK.Extensions;
using O9K.AIO.Heroes.Base;
using O9K.AIO.Modes.Base;
using O9K.AIO.Modes.Permanent;
using O9K.Core.Entities.Abilities.Base;
using O9K.Core.Entities.Abilities.Base.Components.Base;
using O9K.Core.Entities.Abilities.Heroes.Invoker;
using O9K.Core.Entities.Abilities.Heroes.Invoker.Helpers;
using O9K.Core.Helpers;
using O9K.Core.Logger;
using O9K.Core.Managers.Menu.EventArgs;
using SharpDX;

namespace O9K.AIO.Heroes.Invoker.Modes
{
    internal class AbilityPanelMode : PermanentMode
    {
        public Sleeper Sleeper { get; set; }

        public AbilityPanelMode(BaseHero baseHero, AbilityPanelModeMenu menu) : base(baseHero, menu)
        {
            ModeMenu = menu;
            Sleeper = new Sleeper();
        }

        private void EnabledOnValueChange(object sender, SwitcherEventArgs e)
        {
            if (e.NewValue)
            {
                RendererManager.Draw += RendererManagerOnDraw;
            }
            else
            {
                RendererManager.Draw -= RendererManagerOnDraw;
            }
        }

        private AbilityPanelModeMenu ModeMenu { get; set; }

        public override void Disable()
        {
            ModeMenu.Enabled.ValueChange -= EnabledOnValueChange;
            base.Disable();
        }

        public override void Dispose()
        {
            ModeMenu.Enabled.ValueChange -= EnabledOnValueChange;
            base.Dispose();
        }

        public override void Enable()
        {
            ModeMenu.Enabled.ValueChange += EnabledOnValueChange;
            base.Enable();
        }

        private void RendererManagerOnDraw()
        {
            if (!ModeMenu.isVisible)
                return;

            var size = new Vector2(ModeMenu.AbilityPanelItems.Count * 50, 50);
            var startPos = new RectangleF(ModeMenu.PanelPositionX, ModeMenu.PanelPositionY, size.X, size.Y);
            RendererManager.DrawFilledRectangle(startPos, Color.Black, new Color(0, 0, 0, 100), 1);


            var currentPos = new Vector2(startPos.X, startPos.Y);
            ModeMenu.AbilityPanelItems.ForEach(keyValue =>
            {
                var id = keyValue.Key;
                var item = keyValue.Value;
                var rect = new RectangleF(currentPos.X, currentPos.Y, size.Y, size.Y);
                RendererManager.DrawTexture(id, rect);

                if (Owner.Hero.Abilities.FirstOrDefault(x => x.Id == id) is IActiveAbility ability)
                {
                    if (ability.CanBeCasted())
                    {
                        RendererManager.DrawText(item.InvokeKey.Key.ToString(), rect, Color.White, FontFlags.VerticalCenter | FontFlags.Center, 15);
                    }
                    else
                    {
                        RendererManager.DrawFilledRectangle(rect, Color.White, new Color(0, 0, 0, 200), 1);
                        
                        RendererManager.DrawText(ability.BaseAbility.Cooldown.ToString("0.0"), rect, Color.White, FontFlags.VerticalCenter | FontFlags.Center, 15);
                    }
                }
                currentPos.X += size.Y;
            });
        }

        protected override void Execute()
        {
            if (Sleeper.IsSleeping)
                return;
            var invoke = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_invoke);
            if (invoke == null || !invoke.CanBeCasted())
            {
                Console.WriteLine($"{invoke?.CanBeCasted()}");
                return;
            }

            ModeMenu.AbilityPanelItems.ForEach(keyValue =>
            {
                var id = keyValue.Key;
                var item = keyValue.Value;
                if (item.Enable && item.InvokeKey.IsActive)
                {
                    Console.WriteLine($"Execute {id}");
                    var ability = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == id) as IInvokableAbility;
                    if (ability is not {CanBeInvoked: true})
                        return;
                    if (Owner.Hero.IsInvisible && !item.Ignore)
                        return;
                    if (ability.IsInvoked)
                    {
                        if (ability.GetAbilitySlot == AbilitySlot.Slot_5 && invoke.CanBeCasted())
                        {
                            ability.Invoke();
                            Sleeper.Sleep(.200f);
                            return;
                        }

                        if (ability is ActiveAbility activeAbility && activeAbility.CanBeCasted())
                        {
                            switch (id)
                            {
                                case AbilityId.invoker_alacrity:
                                {
                                    activeAbility.UseAbility(Owner);
                                    break;
                                }
                                case AbilityId.invoker_chaos_meteor:
                                case AbilityId.invoker_deafening_blast:
                                case AbilityId.invoker_emp:
                                case AbilityId.invoker_sun_strike:
                                case AbilityId.invoker_tornado:
                                {
                                    activeAbility.UseAbility(GameManager.MousePosition);
                                    break;
                                }
                                case AbilityId.invoker_cold_snap:
                                {
                                    if (TargetManager.HasValidTarget)
                                        activeAbility.UseAbility(this.TargetManager.Target);
                                    break;
                                }
                                case AbilityId.invoker_forge_spirit:
                                case AbilityId.invoker_ghost_walk:
                                case AbilityId.invoker_ice_wall:
                                {
                                    activeAbility.UseAbility();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        ability.Invoke();
                        Sleeper.Sleep(.200f);
                        return;
                    }
                }
            });
        }
    }
}