using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Update.Ward_Manager;
using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.LeeSin
{
    using Drawings;
    using Update.Miscellaneous;
    using Update.OrbwalkingEvents.Combo;
    using Update.OrbwalkingEvents.Harass;
    using Update.OrbwalkingEvents.Insec;
    using Update.OrbwalkingEvents.JungleClear;
    using Update.OrbwalkingEvents.LaneClear;
    using Update.OrbwalkingEvents.LastHit;
    using Update.OrbwalkingEvents.WardJump;
    using SDK.Extensions;

    internal class LeeSin
    {
        public void Init()
        {
            var spellConfig = new SpellConfig();
            spellConfig.Load();

            var wardtracker = new WardTracker(spellConfig);
            var wardmanager = new WardManager(wardtracker);
            var wardjump = new WardJump(wardtracker, wardmanager, spellConfig);

            var combo = new Combo(wardmanager, spellConfig);
            var insec = new Insec(wardtracker, wardmanager, spellConfig);
            var harass = new Harass(wardmanager, spellConfig);
            var jungle = new JungleClear(wardmanager, spellConfig);
            var lane = new LaneClear(spellConfig);
            var lasthit = new Lasthit(spellConfig);
            var killsteal = new Killsteal(spellConfig);
            var drawManager = new DrawManager(spellConfig);

            var mainmenu = new Menu("main", "Adept AIO", true);
            mainmenu.Attach();

            var insecMode = new OrbwalkerMode("Insec", KeyCode.T, null, insec.OnKeyPressed);
            var wardjumpMode = new OrbwalkerMode("Wardjump", KeyCode.G, null, wardjump.OnKeyPressed);
            var kickFlashMode = new OrbwalkerMode("Kick Flash", KeyCode.A, null, insec.Kick);

            insecMode.MenuItem.OnValueChanged += (sender, args) => insec.Enabled = args.GetNewValue<MenuKeyBind>().Enabled;
            wardjumpMode.MenuItem.OnValueChanged += (sender, args) => wardjump.Enabled = args.GetNewValue<MenuKeyBind>().Enabled;
            kickFlashMode.MenuItem.OnValueChanged += (sender, args) => insec.KickFlashEnabled = args.GetNewValue<MenuKeyBind>().Enabled;

            GlobalExtension.Orbwalker.AddMode(insecMode);
            GlobalExtension.Orbwalker.AddMode(wardjumpMode);
            GlobalExtension.Orbwalker.AddMode(kickFlashMode);
            GlobalExtension.Orbwalker.Attach(mainmenu);

            var insecMenu = new Menu("Insec", "Insec");
            var insecObject = new MenuBool("Object", "Use Q On Minions").SetToolTip("Uses Q to gapclose to every minion");
            var insecQLast = new MenuBool("Last", "Use Q After Insec").SetToolTip("Only possible if no minions near target");
            var insecPosition = new MenuList("Position", "Insec Position", new[] {"Ally Turret", "Ally Hero"}, 0);
            var insecKick = new MenuList("Kick", "Kick Type: ", new[] {"Flash R", "R Flash"}, 1);

            insecObject.OnValueChanged += (sender, args) => insec.ObjectEnabled = args.GetNewValue<MenuBool>().Enabled;
            insecQLast.OnValueChanged += (sender, args) => insec.QLast = args.GetNewValue<MenuBool>().Enabled;
            insecPosition.OnValueChanged += (sender, args) => insec.InsecPositionValue = args.GetNewValue<MenuList>().Value;
            insecKick.OnValueChanged += (sender, args) => insec.InsecKickValue = args.GetNewValue<MenuList>().Value;

            insec.ObjectEnabled = insecObject.Enabled;
            insec.QLast = insecQLast.Enabled;
            insec.InsecPositionValue = insecPosition.Value;
            insec.InsecKickValue = insecKick.Value;

            insecMenu.Add(insecObject);
            insecMenu.Add(insecQLast);
            insecMenu.Add(insecPosition);
            insecMenu.Add(insecKick);
            mainmenu.Add(insecMenu);

            var comboMenu = new Menu("Combo", "Combo");
            var comboTurret = new MenuBool("Turret", "Don't Q2 Into Turret");
            var comboQ    = new MenuBool("Q", "Use Q");
            var comboQ2   = new MenuBool("Q2", "Use Q2");
            var comboW    = new MenuBool("W", "Use W");
            var comboWard = new MenuBool("Ward", "Use Wards");
            var comboE    = new MenuBool("E", "Use E");

            comboTurret.OnValueChanged += (sender, args) => combo.TurretCheckEnabled = args.GetNewValue<MenuBool>().Value;
            comboQ.OnValueChanged      += (sender, args) => combo.Q1Enabled   = args.GetNewValue<MenuBool>().Value;
            comboQ2.OnValueChanged     += (sender, args) => combo.Q2Enabled   = args.GetNewValue<MenuBool>().Value;
            comboW.OnValueChanged      += (sender, args) => combo.WEnabled    = args.GetNewValue<MenuBool>().Value;
            comboWard.OnValueChanged   += (sender, args) => combo.WardEnabled = args.GetNewValue<MenuBool>().Value;
            comboE.OnValueChanged      += (sender, args) => combo.EEnabled    = args.GetNewValue<MenuBool>().Value;

            combo.TurretCheckEnabled = comboTurret.Enabled;
            combo.Q1Enabled = comboQ.Enabled;
            combo.Q2Enabled = comboQ2.Enabled;
            combo.WEnabled = comboW.Enabled;
            combo.WardEnabled = comboWard.Enabled;
            combo.EEnabled = comboE.Enabled;

            comboMenu.Add(comboTurret);
            comboMenu.Add(comboQ);
            comboMenu.Add(comboQ2);
            comboMenu.Add(comboW);
            comboMenu.Add(comboE);
            comboMenu.Add(comboWard);
            mainmenu.Add(comboMenu);

            var harassMenu = new Menu("Harass", "Harass");
            var harassQ = new MenuBool("Q", "Use Q");
            var harassQ2 = new MenuBool("Q2", "Use Q2");
            var harassMode = new MenuList("Mode", "W Mode: ", new[] {"Away", "W Self"}, 0);
            var harassE = new MenuBool("E", "Use E");
            var harassE2 = new MenuBool("E2", "Use E2");

            harassQ.OnValueChanged += (sender, args) => harass.Q1Enabled = args.GetNewValue<MenuBool>().Enabled;
            harassQ2.OnValueChanged += (sender, args) => harass.Q2Enabled = args.GetNewValue<MenuBool>().Enabled;
            harassMode.OnValueChanged += (sender, args) => harass.Mode = args.GetNewValue<MenuList>().Value;
            harassE.OnValueChanged += (sender, args) => harass.EEnabled = args.GetNewValue<MenuBool>().Enabled;
            harassE2.OnValueChanged += (sender, args) => harass.E2Enabled = args.GetNewValue<MenuBool>().Enabled;

            harass.Q1Enabled = harassQ.Enabled;
            harass.Q2Enabled = harassQ2.Enabled;
            harass.Mode = harassMode.Value;
            harass.EEnabled = harassE.Enabled;
            harass.E2Enabled = harassE2.Enabled;

            harassMenu.Add(harassQ);
            harassMenu.Add(harassQ2);
            harassMenu.Add(harassMode);
            harassMenu.Add(harassE);
            harassMenu.Add(harassE2);
            mainmenu.Add(harassMenu);

            var jungleMenu = new Menu("Jungle", "Jungle");
            var jungleSteal = new MenuBool("Steal", "Steal Legendary").SetToolTip("Will Q2 -> Smite -> W");
            var jungleSmite = new MenuBool("Smite", "Smite Big Mobs");
            var jungleBlue = new MenuBool("Blue", "Smite Blue Buff");
            var jungleQ = new MenuBool("Q", "Q");
            var jungleW = new MenuBool("W", "W");
            var jungleE = new MenuBool("E", "E");

            jungleSteal.OnValueChanged += (sender, args) => jungle.StealEnabled = args.GetNewValue<MenuBool>().Enabled;
            jungleSmite.OnValueChanged += (sender, args) => jungle.Q1Enabled = args.GetNewValue<MenuBool>().Enabled;
            jungleBlue.OnValueChanged += (sender, args) => jungle.BlueEnabled = args.GetNewValue<MenuBool>().Enabled;
            jungleQ.OnValueChanged += (sender, args) => jungle.Q1Enabled = args.GetNewValue<MenuBool>().Enabled;
            jungleW.OnValueChanged += (sender, args) => jungle.WEnabled = args.GetNewValue<MenuBool>().Enabled;
            jungleE.OnValueChanged += (sender, args) => jungle.EEnabled = args.GetNewValue<MenuBool>().Enabled;

            jungle.StealEnabled = jungleSteal.Enabled;
            jungle.SmiteEnabled = jungleSmite.Enabled;
            jungle.BlueEnabled = jungleBlue.Enabled;
            jungle.Q1Enabled = jungleQ.Enabled;
            jungle.WEnabled = jungleW.Enabled;
            jungle.EEnabled = jungleE.Enabled;

            jungleMenu.Add(jungleSmite);
            jungleMenu.Add(jungleBlue);
            jungleMenu.Add(jungleQ);
            jungleMenu.Add(jungleW);
            jungleMenu.Add(jungleE);
            mainmenu.Add(jungleMenu);

            var laneMenu = new Menu("Lane", "Lane");
            var laneCheck = new MenuBool("Check", "Don't Clear When Enemies Nearby");
            var laneQ = new MenuBool("Q", "Q");
            var laneW = new MenuBool("W", "W");
            var laneE = new MenuBool("E", "E");

            laneCheck.OnValueChanged += (sender, args) => lane.CheckEnabled = args.GetNewValue<MenuBool>().Enabled;
            laneQ.OnValueChanged += (sender, args) => lane.Q1Enabled = args.GetNewValue<MenuBool>().Enabled;
            laneW.OnValueChanged += (sender, args) => lane.WEnabled = args.GetNewValue<MenuBool>().Enabled;
            laneE.OnValueChanged += (sender, args) => lane.EEnabled = args.GetNewValue<MenuBool>().Enabled;

            lane.CheckEnabled = laneCheck.Enabled;
            lane.Q1Enabled = laneQ.Enabled;
            lane.WEnabled = laneW.Enabled;
            lane.EEnabled = laneE.Enabled;

            laneMenu.Add(laneCheck);
            laneMenu.Add(laneQ);
            laneMenu.Add(laneW);
            laneMenu.Add(laneE);
            mainmenu.Add(laneMenu);

            var lasthitMenu = new Menu("Lasthit", "Lasthit");
            var lasthitEnabled = new MenuBool("Enabled", "Enabled");

            lasthitEnabled.OnValueChanged += (sender, args) => lasthit.Enabled = args.GetNewValue<MenuBool>().Enabled;
            lasthitMenu.Add(lasthitEnabled);
            mainmenu.Add(lasthitMenu);

            var ksMenu = new Menu("Killsteal", "Killsteal");
            var ksIgnite = new MenuBool("Ignite", "Ignite");
            var ksSmite = new MenuBool("Smite", "Smite");
            var ksQ = new MenuBool("Q", "Q");
            var ksE = new MenuBool("E", "E");
            var ksR = new MenuBool("R", "R");

            ksIgnite.OnValueChanged += (sender, args) => killsteal.IgniteEnabled = args.GetNewValue<MenuBool>().Enabled;
            ksSmite.OnValueChanged += (sender, args) => killsteal.SmiteEnabled   = args.GetNewValue<MenuBool>().Enabled;
            ksQ.OnValueChanged += (sender, args) => killsteal.QEnabled = args.GetNewValue<MenuBool>().Enabled;
            ksE.OnValueChanged += (sender, args) => killsteal.EEnabled = args.GetNewValue<MenuBool>().Enabled;
            ksR.OnValueChanged += (sender, args) => killsteal.REnabled = args.GetNewValue<MenuBool>().Enabled;

            killsteal.IgniteEnabled = ksIgnite.Enabled;
            killsteal.SmiteEnabled = ksSmite.Enabled;
            killsteal.QEnabled = ksQ.Enabled;
            killsteal.EEnabled = ksE.Enabled;
            killsteal.REnabled = ksR.Enabled;

            ksMenu.Add(ksIgnite);
            ksMenu.Add(ksSmite);
            ksMenu.Add(ksQ);
            ksMenu.Add(ksE);
            ksMenu.Add(ksR);
            mainmenu.Add(ksMenu);

            var drawMenu = new Menu("Draw", "Drawings");
            var drawSegments = new MenuSlider("Segments", "Segments", 200, 100, 300).SetToolTip("Smoothness of the circles. Less equals more FPS.");
            var drawPosition = new MenuBool("Position", "Insec Position");
            var drawQ = new MenuBool("Q", "Q Range");

            drawManager.QEnabled = drawQ.Enabled;
            drawManager.SegmentsValue = drawSegments.Value;
            drawManager.PositionEnabled = drawPosition.Enabled;

            drawSegments.OnValueChanged += (sender, args) => drawManager.SegmentsValue = args.GetNewValue<MenuSlider>().Value;
            drawPosition.OnValueChanged += (sender, args) => drawManager.PositionEnabled = args.GetNewValue<MenuBool>().Enabled;
            drawQ.OnValueChanged += (sender, args) => drawManager.QEnabled = args.GetNewValue<MenuBool>().Enabled;

            drawMenu.Add(drawSegments);
            drawMenu.Add(drawPosition);
            drawMenu.Add(drawQ);
            mainmenu.Add(drawMenu);
         
            var manager = new Manager(combo, harass, insec, jungle, lane, lasthit);
          
            Game.OnUpdate += manager.OnUpdate;
            Game.OnUpdate += killsteal.OnUpdate;

            GlobalExtension.Orbwalker.PostAttack += manager.PostAttack;

            Render.OnRender += drawManager.RenderManager;

            Obj_AI_Base.OnProcessSpellCast += insec.OnProcessSpellCast;
            Obj_AI_Base.OnProcessSpellCast += spellConfig.OnProcessSpellCast;

            GameObject.OnCreate += wardtracker.OnCreate;
        }
    }
}