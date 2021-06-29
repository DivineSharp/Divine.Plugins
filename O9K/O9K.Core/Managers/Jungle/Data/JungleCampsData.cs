namespace O9K.Core.Managers.Jungle.Data
{
    using Camp;

    using Divine;

    using SharpDX;

    internal class JungleCampsData
    {
        public JungleCamp[] GetJungleCamps()
        {
            return new[]
            {
                // radiant
                new JungleCamp
                {
                    Id = 1,
                    Team = Team.Radiant,
                    IsLarge = true,
                    CreepsPosition = new Vector3(-3840, 1125, 256),
                    DrawPosition = new Vector3(-3824, 944, 256),
                    StackTime = 54
                },
                new JungleCamp
                {
                    Id = 2,
                    Team = Team.Radiant,
                    IsAncient = true,
                    CreepsPosition = new Vector3(-4928, -96, 256),
                    DrawPosition = new Vector3(-4726, -128, 256),
                    StackTime = 56,
                },
                new JungleCamp
                {
                    Id = 3,
                    Team = Team.Radiant,
                    IsSmall = true,
                    CreepsPosition = new Vector3(-2432, -640, 128),
                    DrawPosition = new Vector3(-2402, -583, 128),
                    StackTime = 55,
                },
                new JungleCamp
                {
                    Id = 4,
                    Team = Team.Radiant,
                    IsMedium = true,
                    CreepsPosition = new Vector3(928, -2496, 256),
                    DrawPosition = new Vector3(896, -2528, 256),
                    StackTime = 55,
                },
                new JungleCamp
                {
                    Id = 5,
                    Team = Team.Radiant,
                    IsLarge = true,
                    CreepsPosition = new Vector3(-1848, -4216, 128),
                    DrawPosition = new Vector3(-1848, -4176, 128),
                    StackTime = 55,
                },
                new JungleCamp
                {
                    Id = 6,
                    Team = Team.Radiant,
                    IsMedium = true,
                    CreepsPosition = new Vector3(-48, -4448, 256),
                    DrawPosition = new Vector3(0, -4352, 256),
                    StackTime = 54,
                },
                new JungleCamp
                {
                    Id = 7,
                    Team = Team.Radiant,
                    IsLarge = true,
                    CreepsPosition = new Vector3(2624, -4160, 256),
                    DrawPosition = new Vector3(2528, -4160, 256),
                    StackTime = 55,
                },
                new JungleCamp
                {
                    Id = 8,
                    Team = Team.Radiant,
                    IsSmall = true,
                    CreepsPosition = new Vector3(3456, -4384, 128),
                    DrawPosition = new Vector3(3584, -4640, 128),
                    StackTime = 55,
                },
                new JungleCamp
                {
                    Id = 9,
                    Team = Team.Radiant,
                    IsLarge = true,
                    CreepsPosition = new Vector3(4560, -3440, 128),
                    DrawPosition = new Vector3(4560, -3600, 128),
                    StackTime = 55,
                },

                // dire
                new JungleCamp
                {
                    Id = 10,
                    Team = Team.Dire,
                    IsLarge = true,
                    CreepsPosition = new Vector3(-4340, 3440, 128),
                    DrawPosition = new Vector3(-4336, 3696, 128),
                    StackTime = 54,
                },
                new JungleCamp
                {
                    Id = 11,
                    Team = Team.Dire,
                    IsSmall = true,
                    CreepsPosition = new Vector3(-3008, 4768, 128),
                    DrawPosition = new Vector3(-3360, 4896, 128),
                    StackTime = 55,
                },
                new JungleCamp
                {
                    Id = 12,
                    Team = Team.Dire,
                    IsMedium = true,
                    CreepsPosition = new Vector3(-2016, 4656, 256),
                    DrawPosition = new Vector3(-2016, 4512, 256),
                    StackTime = 55,
                },
                new JungleCamp
                {
                    Id = 13,
                    Team = Team.Dire,
                    IsLarge = true,
                    CreepsPosition = new Vector3(64, 3344, 256),
                    DrawPosition = new Vector3(-16, 3392, 256),
                    StackTime = 54,
                },
                new JungleCamp
                {
                    Id = 14,
                    Team = Team.Dire,
                    IsMedium = true,
                    CreepsPosition = new Vector3(-1472, 3264, 256),
                    DrawPosition = new Vector3(-1344, 3488, 256),
                    StackTime = 55,
                },
                new JungleCamp
                {
                    Id = 15,
                    Team = Team.Dire,
                    IsLarge = true,
                    CreepsPosition = new Vector3(1208, 3415, 128),
                    DrawPosition = new Vector3(1164, 3432, 128),
                    StackTime = 53,
                },
                new JungleCamp
                {
                    Id = 16,
                    Team = Team.Dire,
                    IsSmall = true,
                    CreepsPosition = new Vector3(1968, -400, 128),
                    DrawPosition = new Vector3(1998, -280, 128),
                    StackTime = 55,
                },
                new JungleCamp
                {
                    Id = 17,
                    Team = Team.Dire,
                    IsLarge = true,
                    CreepsPosition = new Vector3(4352, 48, 256),
                    DrawPosition = new Vector3(4256, 96, 384),
                    StackTime = 55,
                },
                new JungleCamp
                {
                    Id = 18,
                    Team = Team.Dire,
                    IsAncient = true,
                    CreepsPosition = new Vector3(3392, -1408, 256),
                    DrawPosition = new Vector3(3392, -1216, 256),
                    StackTime = 55,
                },
            };
        }
    }
}