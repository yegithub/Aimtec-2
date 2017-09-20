using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.Champions.Tristana.Drawings;
using Adept_AIO.Champions.Tristana.Update.Miscellaneous;
using Adept_AIO.Champions.Tristana.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;

namespace Adept_AIO.Champions.Tristana
{
    internal class Tristana
    {
        public void Init()
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

            Render.OnRender += drawManager.OnRender;
            Render.OnPresent += drawManager.OnPresent;

            Gapcloser.OnGapcloser += gapcloser.OnGapcloser;
        }
    }
}
