﻿namespace O9K.Core.Entities.Abilities.Items
{
    using Base;

    using Divine.Entity.Entities.Abilities;
    using Divine.Entity.Entities.Abilities.Components;

    using Metadata;

    [AbilityId(AbilityId.item_ultimate_scepter_2)]
    public class AghanimsBlessing : PassiveAbility
    {
        public AghanimsBlessing(Ability baseAbility)
            : base(baseAbility)
        {
        }
    }
}