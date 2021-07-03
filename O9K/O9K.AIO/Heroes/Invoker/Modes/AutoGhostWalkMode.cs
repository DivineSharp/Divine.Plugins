using System.Linq;
using Divine;
using O9K.AIO.Heroes.Base;
using O9K.AIO.Modes.Permanent;
using O9K.Core.Entities.Abilities.Heroes.Invoker;
using O9K.Core.Helpers;
using O9K.Core.Managers.Menu.Items;

namespace O9K.AIO.Heroes.Invoker.Modes
{
    internal class AutoGhostWalkMode : PermanentMode
    {
        private readonly AutoGhostWalkModeMenu modeMenu;
        private MenuSlider HpSlider => modeMenu.hpSlider;
        private readonly Sleeper sleeper = new Sleeper();

        public AutoGhostWalkMode(BaseHero baseHero, AutoGhostWalkModeMenu menu)
            : base(baseHero, menu)
        {
            modeMenu = menu;
        }


        protected override void Execute()
        {
            if (sleeper.IsSleeping)
                return;
            var ghostWalk = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_ghost_walk) as GhostWalk;
            var wex = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_wex);
            if (ghostWalk is not null && ghostWalk.CanBeCasted() && (ghostWalk.CanBeInvoked || ghostWalk.IsInvoked))
            {
                if (Owner.Hero.HealthPercentage < HpSlider)
                {
                    if (!ghostWalk.IsInvoked)
                    {
                        if (!ghostWalk.Invoke())
                            return;
                    }

                    if (wex != null)
                    {
                        // wex.BaseAbility.Cast();
                        // wex.BaseAbility.Cast();
                        // wex.BaseAbility.Cast();
                    }
                    
                    ghostWalk.BaseAbility.Cast();
                    sleeper.Sleep(0.500f);
                }
            }
            
            sleeper.Sleep(0.15f);
        }
    }
}