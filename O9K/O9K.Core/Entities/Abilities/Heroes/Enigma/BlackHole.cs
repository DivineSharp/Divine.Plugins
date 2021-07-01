namespace O9K.Core.Entities.Abilities.Heroes.Enigma
{
    using Base;
    using Base.Components;
    using Base.Types;

    using Divine;

    using Helpers;

    using Metadata;

    [AbilityId(AbilityId.enigma_black_hole)]
    public class BlackHole : CircleAbility, IChanneled, IDisable
    {
        public BlackHole(Ability baseAbility)
            : base(baseAbility)
        {
            this.RadiusData = new SpecialData(baseAbility, "radius");
            this.ChannelTime = baseAbility.AbilityData.GetChannelMaximumTime(0);
        }

        public UnitState AppliesUnitState { get; } = UnitState.Stunned;

        public float ChannelTime { get; }

        public bool IsActivatesOnChannelStart { get; } = true;
    }
}