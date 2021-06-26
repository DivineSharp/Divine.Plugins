//namespace O9K.AIO.Heroes.Tinker.Abilities
//{
//    using System.Collections.Generic;

//    using AIO.Abilities;
//    using AIO.Modes.Combo;

//    using Core.Entities.Abilities.Base;

//    using O9K.AIO.Abilities.Menus;
//    using O9K.AIO.Heroes.Pudge.Abilities;

//    using TargetManager;

//    internal class BlinkDaggerTinker : BlinkAbility
//    {
//        public BlinkDaggerTinker(ActiveAbility ability)
//            : base(ability)
//        {
//        }

//        public override UsableAbilityMenu GetAbilityMenu(string simplifiedName)
//        {
//            return new BlinkDaggerTinkerMenu(this.Ability, simplifiedName);
//        }

//        /*public bool UseAbility(TargetManager targetManager, IComboModeMenu menu)
//        {
//            if (!CanBeCasted(targetManager, true, menu))
//            {
//                return false;
//            }

//            return UseAbility();
//        }*/
//    }
//}