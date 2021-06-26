//namespace O9K.AIO.Heroes.Pudge.Abilities
//{
//    using AIO.Abilities.Menus;

//    using Core.Entities.Abilities.Base;
//    using Core.Managers.Menu.Items;

//    internal class BlinkDaggerTinkerMenu : UsableAbilityMenu
//    {
//        public BlinkDaggerTinkerMenu(Ability9 ability, string simplifiedName)
//            : base(ability, simplifiedName)
//        {
//            this.ActivateMouseDistance = this.Menu.Add(new MenuSlider("Activate on mouse distance", "activateMouseDistance" + simplifiedName, 650, 0, 1200)); //.SetTooltip("Use ability only when enemy is moving in the same direction"));
//            //this.ActivateMouseDistance.AddTranslation(Lang.Ru, "Задержка (мс)");
//            //this.ActivateMouseDistance.AddTooltipTranslation(Lang.Ru, "Использовать только тогда, когда враг движется в том же направлении");
//            //this.ActivateMouseDistance.AddTranslation(Lang.Cn, "延迟（毫秒）");
//            //this.ActivateMouseDistance.AddTooltipTranslation(Lang.Cn, "仅当敌人向同一方向移动给定时间时才使用");

//            this.ActivateBasedEnemyDistance = this.Menu.Add(new MenuSlider("Activate based on enemy distance", "activateBasedEnemyDistance" + simplifiedName, 1850, 0, 2500)); //.SetTooltip("Use ability only when enemy is moving in the same direction"));
//            //this.ActivateBasedEnemyDistance.AddTranslation(Lang.Ru, "Задержка (мс)");
//            //this.ActivateBasedEnemyDistance.AddTooltipTranslation(Lang.Ru, "Использовать только тогда, когда враг движется в том же направлении");
//            //this.ActivateBasedEnemyDistance.AddTranslation(Lang.Cn, "延迟（毫秒）");
//            //this.ActivateBasedEnemyDistance.AddTooltipTranslation(Lang.Cn, "仅当敌人向同一方向移动给定时间时才使用");

//            this.EnableDistanceFromEnemy = this.Menu.Add(new MenuSwitcher("Enable distance from enemy", "enableDistanceFromEnemy" + simplifiedName, false)); //.SetTooltip("Use ability only when enemy is moving in the same direction"));
//            //this.DistanceFromEnemy.AddTranslation(Lang.Ru, "Задержка (мс)");
//            //this.DistanceFromEnemy.AddTooltipTranslation(Lang.Ru, "Использовать только тогда, когда враг движется в том же направлении");
//            //this.DistanceFromEnemy.AddTranslation(Lang.Cn, "延迟（毫秒）");
//            //this.DistanceFromEnemy.AddTooltipTranslation(Lang.Cn, "仅当敌人向同一方向移动给定时间时才使用");

//            this.DistanceFromEnemy = this.Menu.Add(new MenuSlider("Distance from enemy", "distanceFromEnemy" + simplifiedName, 300, 0, 500)); //.SetTooltip("Use ability only when enemy is moving in the same direction"));
//            //this.DistanceFromEnemy.AddTranslation(Lang.Ru, "Задержка (мс)");
//            //this.DistanceFromEnemy.AddTooltipTranslation(Lang.Ru, "Использовать только тогда, когда враг движется в том же направлении");
//            //this.DistanceFromEnemy.AddTranslation(Lang.Cn, "延迟（毫秒）");
//            //this.DistanceFromEnemy.AddTooltipTranslation(Lang.Cn, "仅当敌人向同一方向移动给定时间时才使用");
//        }

//        public MenuSlider ActivateMouseDistance { get; }

//        public MenuSlider ActivateBasedEnemyDistance { get; }

//        public MenuSwitcher EnableDistanceFromEnemy { get; }

//        public MenuSlider DistanceFromEnemy { get; }
//    }
//}