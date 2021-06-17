﻿namespace O9K.Evader.Abilities.Units.OgreFrostmage.IceArmor
{
    using Base.Evadable;

    using Core.Entities.Abilities.Base;
    using Core.Entities.Units;

    using Divine.Modifier.Modifiers;

    using Metadata;

    using Pathfinder.Obstacles.Modifiers;

    internal class IceArmorEvadable : ModifierCounterEvadable
    {
        public IceArmorEvadable(Ability9 ability, IPathfinder pathfinder, IMainMenu menu)
            : base(ability, pathfinder, menu)
        {
        }

        public override bool ModifierEnemyCounter { get; } = true;

        public override void AddModifier(Modifier modifier, Unit9 modifierOwner)
        {
            var obstacle = new ModifierEnemyObstacle(this, modifier, modifierOwner, 400);
            this.Pathfinder.AddObstacle(obstacle);
        }
    }
}