using System;
using Divine;
using O9K.AIO.Heroes.Base;
using O9K.AIO.Heroes.Invoker.Modes;
using O9K.AIO.Modes.Permanent;
using O9K.Core.Entities.Metadata;

namespace O9K.AIO.Heroes.Invoker
{
    [HeroId(HeroId.npc_dota_hero_invoker)]
    internal class InvokerBase : BaseHero
    {
        private readonly SmartSpheresMode smartSpheresMode;
        private readonly AutoGhostWalkMode autoGhostWalkMode;
        private readonly AutoSunStrikeMode autoSunStrikeMode;
        private readonly AbilityPanelMode abilityPanelMode;

        public InvokerBase()
        {
            smartSpheresMode = new SmartSpheresMode(this, new SmartSpheresModeModeMenu(Menu.RootMenu, "Smart spheres"));
            autoGhostWalkMode = new AutoGhostWalkMode(this, new AutoGhostWalkModeMenu(Menu.RootMenu, "Auto ghostWalk"));
            autoSunStrikeMode = new AutoSunStrikeMode(this, new AutoSunStrikeModeMenu(Menu.RootMenu, "Auto sunStrike"));
            abilityPanelMode = new AbilityPanelMode(this, new AbilityPanelModeMenu(Menu.RootMenu, "Ability panel", invokerBase: this));
        }

        public override void Dispose()
        {
            smartSpheresMode.Disable();
            autoGhostWalkMode.Disable();
            autoSunStrikeMode.Disable();
            abilityPanelMode.Disable();
            base.Dispose();
        }
        
        protected override void DisableCustomModes()
        {
            smartSpheresMode.Disable();
            autoGhostWalkMode.Disable();
            autoSunStrikeMode.Disable();
            abilityPanelMode.Disable();
        }

        protected override void EnableCustomModes()
        {
            smartSpheresMode.Enable();
            autoGhostWalkMode.Enable();
            autoSunStrikeMode.Enable();
            abilityPanelMode.Enable();
        }
    }
}