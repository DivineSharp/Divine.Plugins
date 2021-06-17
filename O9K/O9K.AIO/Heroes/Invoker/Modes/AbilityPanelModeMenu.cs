using System.Collections.Generic;
using System.Linq;
using Divine;
using O9K.AIO.Modes.Permanent;
using O9K.Core.Entities.Abilities.Heroes.Invoker.Helpers;
using O9K.Core.Managers.Menu;
using O9K.Core.Managers.Menu.Items;

namespace O9K.AIO.Heroes.Invoker.Modes
{
    public class AbilityPanelItem
    {
        public MenuSwitcher Enable { get; set; }
        public MenuHoldKey InvokeKey { get; set; }
        public MenuSwitcher Ignore { get; set; }

        public MenuSwitcher UseAfter { get; set; }
        // public MenuSwitcher ReInvoke { get; set; }
    }

    internal class AbilityPanelModeMenu : PermanentModeMenu
    {
        public Dictionary<AbilityId, AbilityPanelItem> AbilityPanelItems { get; set; } = new();

        public AbilityPanelModeMenu(Core.Managers.Menu.Items.Menu rootMenu, string displayName, string tooltip = null, InvokerBase invokerBase = null)
            : base(rootMenu, displayName, tooltip)
        {
            if (invokerBase == null) return;
            var list = invokerBase.Owner.Hero.Abilities.Where(x => x is IInvokableAbility).ToList();
            var panel = Menu.Add(new Core.Managers.Menu.Items.Menu("Panel"));
            isVisible = panel.Add(new MenuSwitcher("Show"));
            PanelPositionX = panel.Add(new MenuSlider("pos x", 500, 0, 3000));
            PanelPositionY = panel.Add(new MenuSlider("pos y", 500, 0, 3000));
            foreach (var ability in list)
            {
                var abilityMenu = Menu.Add(new Core.Managers.Menu.Items.Menu(ability.DisplayName));
                abilityMenu.SetTexture(ability.Id);
                var enableSwitcher = new MenuSwitcher("Enabled", "Enabled" + SimplifiedName);
                var invokeKey = new MenuHoldKey("Invoke key");
                var ignore = new MenuSwitcher("Ignore invisibility", defaultValue: false);
                var useAfter = new MenuSwitcher("UseAfterInvoke", defaultValue: false);
                // var reInvoke = new MenuSwitcher("ReInvoke if on last slot", defaultValue: true);
                abilityMenu.Add(enableSwitcher);
                abilityMenu.Add(invokeKey);
                abilityMenu.Add(ignore);
                abilityMenu.Add(useAfter);
                // abilityMenu.Add(reInvoke);
                AbilityPanelItems.Add(ability.Id, new AbilityPanelItem()
                {
                    Enable = enableSwitcher,
                    InvokeKey = invokeKey,
                    Ignore = ignore,
                    UseAfter = useAfter,
                    // ReInvoke = reInvoke,
                });
            }
        }

        public MenuSlider PanelPositionY { get; set; }

        public MenuSlider PanelPositionX { get; set; }

        public MenuSwitcher isVisible { get; set; }
    }
}