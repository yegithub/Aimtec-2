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
        private readonly SpellConfig _spellConfig;
        private readonly MenuConfig _menuConfig;

        public BaseUlt(SpellConfig spellConfig, MenuConfig menuConfig)
        {
            _spellConfig = spellConfig;
            _menuConfig = menuConfig;
        }

        private int _timeUntilCasting;
        private int _recallTick;
        private float _recallTime;
        private Obj_AI_Hero _target;
        
        private float TravelTime(Vector3 pos)
        {
            return Global.Player.Distance(pos) / _spellConfig.R.Speed * 1000 + 550;
        }

        private void SetRecall(float recall, int tickCount, Obj_AI_Hero target)
        {
            _recallTime = recall;
            _recallTick = tickCount;
            _target = target;
        }
        public void OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            if (args.Status == TeleportStatus.Abort || sender.IsMe || sender.IsAlly || !_menuConfig.Killsteal["BaseUlt"].Enabled || args.Type != TeleportType.Recall)
            {
                return;
            }

            SetRecall(args.Duration, Game.TickCount, (Obj_AI_Hero) sender);
            Console.WriteLine(sender.UnitSkinName + " Is Recalling");
        }
     
        public void OnUpdate()
        {
            if (!_menuConfig.Killsteal["BaseUlt"].Enabled || _target == null || !_spellConfig.R.Ready)
            {
                return;
            }

            var time = -(Game.TickCount - (_recallTick + _recallTime));
            var pos = Mixed.GetFountainPos(_target);
            var poly = new Geometry.Rectangle(Geometry.To2D(Global.Player.ServerPosition), Geometry.To2D(pos), _spellConfig.R.Width);

            _timeUntilCasting = (int) (time - TravelTime(pos));

            if (GameObjects.EnemyHeroes.Any(x => poly.IsInside(Geometry.To2D(x.ServerPosition))) && _target.Health < Global.Player.GetSpellDamage(_target, SpellSlot.R) * 1.15f) // Bug: Sort of broken? Not sure.
            {
                if (time - TravelTime(pos) > Game.Ping / 2f + 30)
                {
                    return;
                }

                _spellConfig.R.Cast(pos);
                SetRecall(0, 0, null);
            }
            else
            {
                SetRecall(0, 0, null);
            }
        }

        public void OnRender()
        {
            if (_target == null || !_menuConfig.Killsteal["BaseUlt"].Enabled || !_menuConfig.Drawings["Status"].Enabled)
            {
                return;
            }

            var ts = TimeSpan.FromMilliseconds(_timeUntilCasting);

            Vector2 xd;
            Render.WorldToScreen(Global.Player.ServerPosition, out xd);
            Render.Text(new Vector2(xd.X - 35, xd.Y + 20), Color.White, "Ulting " + _target.ChampionName + " In " + $"{ts.Seconds:00}:{ts.Milliseconds:00}");
        }
    }
}