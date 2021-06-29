namespace O9K.AIO.Heroes.Kunkka
{
    using AIO.Modes.KeyPress;
    using AIO.Modes.Permanent;

    using Base;

    using Core.Entities.Metadata;
    using Core.Managers.Context;

    using Divine;

    using Modes;

    [HeroId(HeroId.npc_dota_hero_kunkka)]
    internal class KunkkaBase : BaseHero
    {
        private readonly AutoReturnMode autoReturnMode;

        private readonly TorrentStackMode torrentStackMode;

        public KunkkaBase()
        {
            this.autoReturnMode = new AutoReturnMode(
                this,
                new PermanentModeMenu(this.Menu.RootMenu, "Auto return", "Auto use \"X return\""));
            this.torrentStackMode = new TorrentStackMode(this, new KeyPressModeMenu(this.Menu.RootMenu, "Stack camps"));
        }

        public override void Dispose()
        {
            base.Dispose();
            this.autoReturnMode.Dispose();
            this.torrentStackMode.Dispose();
        }

        protected override void DisableCustomModes()
        {
            this.autoReturnMode.Disable();
            this.torrentStackMode.Disable();
        }

        protected override void EnableCustomModes()
        {
            this.autoReturnMode.Enable();
            this.torrentStackMode.Enable();
        }
    }
}