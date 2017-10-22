namespace Adept_AIO.Champions.Tristana
{
    using Aimtec;
    using Core;
    using Drawings;
    using Miscellaneous;
    using OrbwalkingEvents;
    using SDK.Delegates;
    using SDK.Unit_Extensions;

    class Tristana
    {
        public Tristana()
        {
            var menuConfig = new MenuConfig();

            var spellConfig = new SpellConfig();

            var dmg = new Dmg(spellConfig);

            var combo = new Combo(spellConfig, menuConfig, dmg);
            var harass = new Harass(spellConfig, menuConfig);
            var laneclear = new LaneClear(spellConfig, menuConfig);
            var jungleclear = new JungleClear(menuConfig, spellConfig);

            var manager = new Manager(combo, harass, laneclear, jungleclear);
            var killsteal = new Killsteal(menuConfig, spellConfig);
            var drawManager = new DrawManager(menuConfig, dmg, spellConfig);
            var gapcloser = new AntiGapcloser(spellConfig);

            Game.OnUpdate += manager.OnUpdate;
            Game.OnUpdate += killsteal.OnUpdate;

            Global.Orbwalker.PostAttack += manager.OnPostAttack;
            Global.Orbwalker.PreAttack += manager.OnPreAttack;

            Render.OnRender += drawManager.OnRender;
            Render.OnPresent += drawManager.OnPresent;

            Gapcloser.OnGapcloser += gapcloser.OnGapcloser;
        }
    }
}