using System.Collections.Generic;
using Divine;
using O9K.AIO.Modes.Permanent;
using O9K.Core.Managers.Menu;
using O9K.Core.Managers.Menu.Items;

namespace O9K.AIO.Heroes.Invoker.Modes
{
    internal class SmartSpheresModeModeMenu : PermanentModeMenu
    {
        public SmartSpheresModeModeMenu(Core.Managers.Menu.Items.Menu rootMenu, string displayName, string tooltip = null)
            : base(rootMenu, displayName, tooltip)
        {
            // IsVisible = Menu.Add(new MenuSwitcher("Show"));
            // PanelPositionX = Menu.Add(new MenuSlider("pos x", 500, 0, 3000));
            // PanelPositionY = Menu.Add(new MenuSlider("pos y", 500, 0, 3000));
            // IconSize = Menu.Add(new MenuSlider("Size", 50, 10, 100));

            UseHpDetection = Menu.Add(new MenuSwitcher("Use hp % detection"));

            UseHpDetection.ValueChange += (sender, args) =>
            {
                if (args.NewValue)
                {
                    HpSlider = Menu.Add(new MenuSlider("Health for quas", 75, 0, 100));
                    HpSlider.SetTooltip("Change on move to quas when hp% is lower");
                    HpSlider.AddTranslation(Lang.Ru, "Жизни для quas");
                    HpSlider.AddTooltipTranslation(Lang.Ru, "Использовать quas, а не wex при движении, когда процент хп меньше выбранного значения");
                    if (OnAttackAbility != null)
                        Menu.Remove(OnAttackAbility);
                    if (OnMoveAbility != null)
                        Menu.Remove(OnMoveAbility);
                }
                else
                {
                    if (HpSlider != null)
                    {
                        HpSlider.Hide();
                        Menu.Remove(HpSlider);
                    }

                    OnAttackAbility = Menu.Add(new MenuAbilityPriorityChanger("On Attack", new Dictionary<AbilityId, bool>()
                    {
                        {AbilityId.invoker_exort, true},
                        {AbilityId.invoker_wex, false},
                        {AbilityId.invoker_quas, false},
                    }));
                    OnMoveAbility = Menu.Add(new MenuAbilityPriorityChanger("On Move", new Dictionary<AbilityId, bool>()
                    {
                        {AbilityId.invoker_quas, true},
                        {AbilityId.invoker_wex, true},
                        {AbilityId.invoker_exort, false},
                    }));
                }
            };
        }

        public MenuAbilityPriorityChanger OnMoveAbility { get; set; }

        public MenuAbilityPriorityChanger OnAttackAbility { get; set; }

        public MenuSwitcher UseHpDetection { get; set; }

        public MenuSlider IconSize { get; set; }

        public MenuSlider PanelPositionX { get; set; }

        public MenuSlider PanelPositionY { get; set; }

        public MenuSwitcher IsVisible { get; set; }

        public MenuSlider HpSlider { get; set; }
    }
}