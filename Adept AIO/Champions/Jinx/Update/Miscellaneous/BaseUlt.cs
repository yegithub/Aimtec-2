using System;
using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.Jinx.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;


namespace Adept_AIO.Champions.Jinx.Update.Miscellaneous
{
    class BaseUlt
    {
        private readonly SpellConfig SpellConfig;
        private readonly MenuConfig MenuConfig;

        public BaseUlt(SpellConfig spellConfig, MenuConfig menuConfig)
        {
            SpellConfig = spellConfig;
            MenuConfig = menuConfig;
        }

        private int TimeUntilCasting;
        private int recallTick;
        private Obj_AI_Hero Target;
        private string recallName;

        private float TravelTime(Vector3 pos)
        {
            return Global.Player.Distance(pos) / SpellConfig.R.Speed * 1000 + 550;
        }

        private static bool IsRecalling(Obj_AI_BaseTeleportEventArgs args)
        {
            return args.Sender != Global.Player && args.Name.ToLower().Contains("recall");
        }

        private int GetRecallDuration()
        {
            switch (recallName)
            {
                case "recall":
                    return 8000;
                case "RecallImproved":
                    return 7000;
                case "OdinRecall":
                    return 4500;
                case "OdinRecallImproved":
                case "SuperRecall":
                case "SuperRecallImproved":
                    return 4000;
            }
            return 8000;
        }

        private void SetRecall(string recallName, int tickCount, Obj_AI_Hero target)
        {
            this.recallName = recallName;
            recallTick = tickCount;
            Target = target;
        }

        public void OnTeleport(Obj_AI_Base sender, Obj_AI_BaseTeleportEventArgs args)
        {
            if (sender == Global.Player || !MenuConfig.Killsteal["BaseUlt"].Enabled || !IsRecalling(args))
            {
                return;
            }

            SetRecall(args.Name, Game.TickCount, (Obj_AI_Hero) args.Sender);
            Console.WriteLine(args.Sender.UnitSkinName + " Is Recalling");
        }

        public void OnUpdate()
        {
            if (!MenuConfig.Killsteal["BaseUlt"].Enabled || Target == null)
            {
                return;
            }

            var time = -(Game.TickCount - (recallTick + GetRecallDuration()));
            var pos = Mixed.GetFountainPos(Target);
            var poly = new Geometry.Rectangle(Geometry.To2D(Global.Player.ServerPosition), Geometry.To2D(pos), SpellConfig.R.Width);

            TimeUntilCasting = (int) (time - TravelTime(pos));

            if (GameObjects.EnemyHeroes.Any(x => poly.IsInside(Geometry.To2D(x.ServerPosition))) && Target.Health < Global.Player.GetSpellDamage(Target, SpellSlot.R) * 1.15f) // Bug: Sort of broken? Not sure.
            {
                if (time - TravelTime(pos) > Game.Ping / 2f + 30)
                {
                    return;
                }
                SpellConfig.R.Cast(pos);
                SetRecall("", 0, null);
            }
            else
            {
                SetRecall("", 0, null);
            }
        }

        public void OnRender()
        {
            if (Target != null && MenuConfig.Killsteal["BaseUlt"].Enabled && MenuConfig.Drawings["Status"].Enabled)
            {
                Vector2 xd;
                Render.WorldToScreen(Global.Player.ServerPosition, out xd);
                Render.Text(xd, Color.White, "Ulting " + Target.ChampionName + " In " + TimeUntilCasting / 1000);
            }
        }
    }
}