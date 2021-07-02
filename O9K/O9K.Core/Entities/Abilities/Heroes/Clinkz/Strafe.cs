namespace O9K.Core.Entities.Abilities.Heroes.Clinkz
{
    using Base;
    using Base.Types;

    using Divine;

    using Metadata;

    [AbilityId(AbilityId.clinkz_strafe)]
    public class Strafe : ActiveAbility, IBuff
    {
        public Strafe(Ability baseAbility)
            : base(baseAbility)
        {

        }

        public string BuffModifierName { get; } = "modifier_clinkz_strafe";

        public bool BuffsAlly { get; } = false;

        public bool BuffsOwner { get; } = true;
    }
}
