using System.Linq;
using Divine;
using O9K.AIO.Heroes.Base;
using O9K.AIO.Modes.Base;
using O9K.Core.Helpers;
using O9K.Core.Managers.Menu.EventArgs;
using O9K.Core.Managers.Menu.Items;

namespace O9K.AIO.Heroes.Invoker.Modes
{
    internal class SmartSpheresMode : BaseMode
    {
        private readonly Sleeper sleeper = new();
        private readonly Sleeper inChanging = new();
        private readonly MultiSleeper<string> multySleeper = new();
        private readonly SmartSpheresModeModeMenu modeMenu;
        private MenuSlider HpSlider => modeMenu.hpSlider;

        public SmartSpheresMode(BaseHero baseHero, SmartSpheresModeModeMenu menu)
            : base(baseHero)
        {
            modeMenu = menu;
        }

        public void Disable()
        {
            OrderManager.OrderAdding -= PlayerOnOnExecuteOrder;
        }

        public override void Dispose()
        {
            OrderManager.OrderAdding -= PlayerOnOnExecuteOrder;
            modeMenu.Enabled.ValueChange -= EnabledOnValueChanged;
            base.Dispose();
        }

        private void EnabledOnValueChanged(object sender, SwitcherEventArgs e)
        {
            if (e.NewValue)
                OrderManager.OrderAdding += PlayerOnOnExecuteOrder;
            else
                OrderManager.OrderAdding -= PlayerOnOnExecuteOrder;
        }

        public void Enable()
        {
            modeMenu.Enabled.ValueChange += EnabledOnValueChanged;
        }

        private void PlayerOnOnExecuteOrder(OrderAddingEventArgs args)
        {
            if (inChanging.IsSleeping)
                return;
            
            if (Owner.Hero.IsInvisible)
                return;

            var order = args.Order.Type;
            if (order == OrderType.Cast && !args.IsCustom)
            {
                var abilityId = args.Order.Ability.Id;
                if (abilityId is AbilityId.invoker_quas or AbilityId.invoker_wex or AbilityId.invoker_exort or AbilityId.invoker_invoke or AbilityId.invoker_ghost_walk)
                {
                    sleeper.Sleep(1.5f);
                    multySleeper.Sleep("attack", 2.5f);
                    multySleeper.Sleep("move", 2.5f);
                }
            }

            if (sleeper.IsSleeping || Owner.Hero.IsSilenced)
                return;
            if (order == OrderType.AttackPosition || order == OrderType.AttackTarget)
            {
                if (multySleeper.IsSleeping("attack"))
                    return;
                multySleeper.Sleep("attack", .550f);
                var countOfModifiers =
                    Owner.Hero.BaseModifiers.Count(x => x.Name == $"modifier_{AbilityId.invoker_exort}_instance");
                if (countOfModifiers >= 3) return;
                var activeSphereForAttack = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_exort);
                if (activeSphereForAttack?.CanBeCasted() == true)
                {
                    for (var i = countOfModifiers; i < 3; i++) activeSphereForAttack.BaseAbility.Cast();
                    inChanging.Sleep(.250f);
                }
            }
            else if (order is OrderType.MovePosition or OrderType.MoveTarget)
            {
                if (multySleeper.IsSleeping("move"))
                    return;
                multySleeper.Sleep("move", .550f);

                var activeSphereForMove = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_wex);
                if (Owner.Hero.HealthPercentage / 100f <= HpSlider.Value / 100f)
                {
                    activeSphereForMove = Owner.Hero.Abilities.FirstOrDefault(x => x.Id == AbilityId.invoker_quas);
                }

                if (activeSphereForMove is not null && activeSphereForMove.CanBeCasted())
                {
                    var countOfModifiers =
                        Owner.Hero.BaseModifiers.Count(x => x.Name == $"modifier_{activeSphereForMove.Id}_instance");
                    if (countOfModifiers >= 3) return;
                    for (var i = countOfModifiers; i < 3; i++) activeSphereForMove.BaseAbility.Cast();
                    inChanging.Sleep(.350f);
                }
            }
        }
    }
}