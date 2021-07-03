using System.Linq;
using System.Windows.Input;
using Divine;
using Divine.SDK.Extensions;
using O9K.AIO.Heroes.Base;
using O9K.AIO.Modes.Permanent;
using O9K.Core.Entities.Abilities.Base;
using O9K.Core.Entities.Abilities.Base.Components.Base;
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
            
            foreach (var modeMenuAbilityPanelItem in ModeMenu.AbilityPanelItems)
            {
                AbilityId id = modeMenuAbilityPanelItem.Key;
                var item = modeMenuAbilityPanelItem.Value;
                
                modeMenuAbilityPanelItem.Value.InvokeKey.ValueChange += (o, args) =>
                {
                    if (!args.NewValue)
                    {
                        return;
                    }
                    
                    var invoke = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_invoke);
                    if (invoke == null)
                    {
                        return;
                    }


                    if (!item.InvokeKey.IsActive)
                    {
                        return;
                    }
                    IInvokableAbility ability = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == id) as IInvokableAbility;
                    if (ability is not {CanBeInvoked: true})
                        return;
                    if (Owner.Hero.IsInvisible && !item.Ignore)
                        return;
                    if (ability.IsInvoked)
                    {
                        if (item.UseIfInvoked.IsEnabled)
                        {
                            if (UseAbility(ability, id))
                            {
                                Sleeper.Sleep(.300f);
                                return;
                            }
                        }
                        if (ability.GetAbilitySlot == AbilitySlot.Slot_5 && invoke.CanBeCasted() && item.ReInvoke)
                        {
                            if (ability.Invoke())
                            {
                                Sleeper.Sleep(.200f);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (ability.Invoke())
                        {
                            Sleeper.Sleep(.200f);
                            if (item.UseAfter)
                                UpdateManager.BeginInvoke(100, () =>
                                {
                                    if (UseAbility(ability, id))
                                        Sleeper.Sleep(.300f);
                                });
                            return;
                        }
                    }

                };
            }
        }

        private void EnabledOnValueChange(object sender, SwitcherEventArgs e)
        {
            if (e.NewValue)
            {
                RendererManager.Draw += RendererManagerOnDraw;
                InputManager.MouseKeyDown += InputManagerOnMouseKeyDown;

            }
            else
            {
                RendererManager.Draw -= RendererManagerOnDraw;
                InputManager.MouseKeyDown -= InputManagerOnMouseKeyDown;
            }
        }

        private bool IsIn(RectangleF rect, Vector2 vector2) => rect.Contains((int) vector2.X, (int) vector2.Y);

        private void InputManagerOnMouseKeyDown(MouseEventArgs e)
        {
            var result = GetAbilityByCoordinates(e.Position);
            if (result != null)
            {
                Logger.Warn($"{result.Id}");
                ((IInvokableAbility) result).Invoke();
                e.Process = true;
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

        private IActiveAbility GetAbilityByCoordinates(Vector2 clickPosition)
        {
            var size = new Vector2(ModeMenu.AbilityPanelItems.Count * ModeMenu.IconSize, ModeMenu.IconSize);
            var startPos = new RectangleF(ModeMenu.PanelPositionX, ModeMenu.PanelPositionY, size.X, size.Y);


            var currentPos = new Vector2(startPos.X, startPos.Y);
            IActiveAbility ability9 = null;
            ModeMenu.AbilityPanelItems
                .OrderBy(x => Owner.Hero.Abilities.FirstOrDefault(z => z.Id == x.Key)?.Cooldown)
                .ForEach(keyValue =>
            {
                var id = keyValue.Key;
                var item = keyValue.Value;
                
                var rect = new RectangleF(currentPos.X, currentPos.Y, size.Y, size.Y);

                if (Owner.Hero.Abilities.FirstOrDefault(x => x.Id == id) is IActiveAbility ability && item.Enable)
                {
                    if (IsIn(rect, clickPosition))
                    {
                        ability9 = ability;
                    }
                }
                currentPos.X += size.Y;
            });
            return ability9;
        }

        private void RendererManagerOnDraw()
        {
            if (!ModeMenu.isVisible)
                return;

            var size = new Vector2(ModeMenu.AbilityPanelItems.Count * ModeMenu.IconSize, ModeMenu.IconSize);
            var startPos = new RectangleF(ModeMenu.PanelPositionX, ModeMenu.PanelPositionY, size.X, size.Y);
            RendererManager.DrawFilledRectangle(startPos, Color.Black, new Color(0, 0, 0, 100), 1);


            var currentPos = new Vector2(startPos.X, startPos.Y);
            ModeMenu.AbilityPanelItems.OrderBy(x => Owner.Hero.Abilities.FirstOrDefault(z => z.Id == x.Key)?.Cooldown).ForEach(keyValue =>
            {
                var id = keyValue.Key;
                var item = keyValue.Value;
                var rect = new RectangleF(currentPos.X, currentPos.Y, size.Y, size.Y);
                RendererManager.DrawTexture(id, rect);

                if (Owner.Hero.Abilities.FirstOrDefault(x => x.Id == id) is IActiveAbility ability)
                {
                    if (ability.CanBeCasted())
                    {
                        if (item.InvokeKey.Key != Key.None)
                            RendererManager.DrawText(item.InvokeKey.Key.ToString(), rect, Color.White, FontFlags.VerticalCenter | FontFlags.Center, 15);
                        RendererManager.DrawFilledRectangle(rect, Color.White, new Color(0, 0, 0, 50), 1);
                    }
                    else
                    {
                        RendererManager.DrawFilledRectangle(rect, Color.White, new Color(0, 0, 0, 200), 1);
                        var cd = ability.BaseAbility.Cooldown;
                        if (cd > 0.0)
                            RendererManager.DrawText(cd.ToString("0.0"), rect, Color.White, FontFlags.VerticalCenter | FontFlags.Center, 15);
                    }
                }
                currentPos.X += size.Y;
            });
        }

        private bool UseAbility(IInvokableAbility ability, AbilityId id)
        {
            if (ability is ActiveAbility activeAbility && activeAbility.CanBeCasted())
            {
                switch (id)
                {
                    case AbilityId.invoker_alacrity:
                    {
                        return activeAbility.BaseAbility.Cast(Owner);
                        break;
                    }
                    case AbilityId.invoker_chaos_meteor:
                    case AbilityId.invoker_deafening_blast:
                    case AbilityId.invoker_emp:
                    case AbilityId.invoker_sun_strike:
                    case AbilityId.invoker_tornado:
                    {
                        return activeAbility.BaseAbility.Cast(GameManager.MousePosition);
                        break;
                    }
                    case AbilityId.invoker_cold_snap:
                    {
                        if (TargetManager.HasValidTarget)
                            return activeAbility.BaseAbility.Cast(this.TargetManager.Target);
                        break;
                    }
                    case AbilityId.invoker_forge_spirit:
                    case AbilityId.invoker_ghost_walk:
                    case AbilityId.invoker_ice_wall:
                    {
                        return activeAbility.BaseAbility.Cast();
                        break;
                    }
                }
            }

            return false;
        }

        protected override void Execute()
        {
            /*if (Sleeper.IsSleeping)
                return;
            var invoke = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_invoke);
            if (invoke == null)
            {
                return;
            }

            ModeMenu.AbilityPanelItems.ForEach(keyValue =>
            {
                AbilityId id = keyValue.Key;
                var item = keyValue.Value;
                if (item.Enable && item.InvokeKey.IsActive)
                {
                    IInvokableAbility ability = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == id) as IInvokableAbility;
                    if (ability is not {CanBeInvoked: true})
                        return;
                    if (Owner.Hero.IsInvisible && !item.Ignore)
                        return;
                    if (ability.IsInvoked)
                    {
                        if (item.UseIfInvoked.IsEnabled)
                        {
                            if (UseAbility(ability, id))
                            {
                                Sleeper.Sleep(.300f);
                                return;
                            }
                        }
                        if (ability.GetAbilitySlot == AbilitySlot.Slot_5 && invoke.CanBeCasted() && item.ReInvoke)
                        {
                            if (ability.Invoke())
                            {
                                Sleeper.Sleep(.200f);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (ability.Invoke())
                        {
                            Sleeper.Sleep(.200f);
                            if (item.UseAfter)
                                UpdateManager.BeginInvoke(100, () =>
                                {
                                    if (UseAbility(ability, id))
                                        Sleeper.Sleep(.300f);
                                });
                            return;
                        }
                    }
                }
            });*/
        }
    }
}