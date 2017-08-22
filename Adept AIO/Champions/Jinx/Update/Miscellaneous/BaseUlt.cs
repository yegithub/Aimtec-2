using System;
using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.Jinx.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;


namespace Adept_AIO.Champions.Jinx.Update.Miscellaneous
{
    internal class BaseUlt
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
        private float recallTime;
        private Obj_AI_Hero Target;
        
        private float TravelTime(Vector3 pos)
        {
            return Global.Player.Distance(pos) / SpellConfig.R.Speed * 1000 + 550;
        }

        private void SetRecall(float recall, int tickCount, Obj_AI_Hero target)
        {
            recallTime = recall;
            recallTick = tickCount;
            Target = target;
        }
        public void OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            if (args.Status == TeleportStatus.Abort || sender.IsMe || sender.IsAlly || !MenuConfig.Killsteal["BaseUlt"].Enabled || args.Type != TeleportType.Recall)
            {
                return;
            }

            SetRecall(args.Duration, Game.TickCount, (Obj_AI_Hero) sender);
            Console.WriteLine(sender.UnitSkinName + " Is Recalling");
        }
     
        public void OnUpdate()
        {
            if (!MenuConfig.Killsteal["BaseUlt"].Enabled || Target == null || !SpellConfig.R.Ready)
            {
                return;
            }

            var time = -(Game.TickCount - (recallTick + recallTime));
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
                SetRecall(0, 0, null);
            }
            else
            {
                SetRecall(0, 0, null);
            }
        }

        public void OnRender()
        {
            if (Target == null || !MenuConfig.Killsteal["BaseUlt"].Enabled || !MenuConfig.Drawings["Status"].Enabled)
            {
                return;
            }

            var ts = TimeSpan.FromMilliseconds(TimeUntilCasting);

            Vector2 xd;
            Render.WorldToScreen(Global.Player.ServerPosition, out xd);
            Render.Text(new Vector2(xd.X - 35, xd.Y + 20), Color.White, "Ulting " + Target.ChampionName + " In " + $"{ts.Seconds:00}:{ts.Milliseconds:00}");
        }
    }
}