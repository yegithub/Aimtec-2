using System;
using Adept_AIO.Champions.Jinx.Core;
using Adept_AIO.Champions.Jinx.Drawings;
using Adept_AIO.Champions.Jinx.Update.Miscellaneous;
using Adept_AIO.Champions.Jinx.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Jinx
{
    internal class Jinx
    {
        public void Init()
        {
            var spellConfig = new SpellConfig();
            spellConfig.Load();

            var menuConfig = new MenuConfig();
            menuConfig.Attach();

            var combo = new Combo(spellConfig, menuConfig);
            var harass = new Harass(spellConfig, menuConfig);
            var laneclear = new LaneClear(menuConfig, spellConfig);
            var jungleclear = new JungleClear(menuConfig, spellConfig);

            var misc = new Misc(spellConfig, menuConfig);
            var gapcloser = new AntiGapcloser(spellConfig);
            var baseUlt = new BaseUlt(spellConfig, menuConfig);

            var manager = new Manager(combo, harass, laneclear, jungleclear);

            var drawManager = new DrawManager(menuConfig, new Dmg(spellConfig), spellConfig);

            Game.OnUpdate += manager.OnUpdate;
            Game.OnUpdate += misc.OnUpdate;
            Game.OnUpdate += baseUlt.OnUpdate;

            Teleport.OnTeleport += baseUlt.OnTeleport;

            Render.OnPresent += drawManager.OnPresent;
            Render.OnRender  += drawManager.OnRender;
            Render.OnRender += baseUlt.OnRender;

            Gapcloser.OnGapcloser += gapcloser.OnGapcloser;
        }
    }
}
