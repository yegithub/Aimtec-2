using Aimtec;
using Spell = Aimtec.SDK.Spell;

namespace Adept_BaseUlt.Manager
{
    using System;
    using System.Drawing;
    using System.Linq;
    using Local_SDK;
   
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util.Cache;

    internal class BaseUlt
    {
        private readonly Spell _ultimate;

        private readonly float _speed;
        private readonly float _width;
        private readonly float _delay;
        private readonly float _range;
        private readonly int _maxCollisionObjects;

        private int _timeUntilCasting = -1;

        private int _recallTick;
        private float _recallTime;
        private Obj_AI_Hero _target;

        public BaseUlt(float speed, float width, float delay, int maxCollisionObjects = int.MaxValue, float range = float.MaxValue)
        {
            _ultimate = new Spell(SpellSlot.R, _range);

            this._range = range;
            this._speed = speed;
            this._width = width;
            this._delay = delay;
            this._maxCollisionObjects = maxCollisionObjects;

            Global.Init();

            Teleport.OnTeleport += OnTeleport;
            Game.OnUpdate += OnUpdate;
            Render.OnRender += OnRender;
        }

     
        private void OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            if (!sender.IsEnemy
             || !sender.IsValid 
             || sender.IsDead
             || Game.TickCount - _ultimate.LastCastAttemptT <= 8000
             || args.Status == TeleportStatus.Unknown 
             || args.Status == TeleportStatus.Finish)
            {
                return;
            }

            if (args.Status == TeleportStatus.Abort)
            {
                Reset();
            }

            if (args.Type == TeleportType.Recall)
            {
                Set(args.Duration, Game.TickCount, (Obj_AI_Hero)sender);
            }
        }

        private void OnUpdate()
        {
            if (_target == null || !_target.IsValid || !_ultimate.Ready || _target.Health > Damage())
            {
                Reset();
                return;
            }

            var time = -(Game.TickCount - (_recallTick + _recallTime));
            var pos = GetFountainPos(_target);

            if (pos.Distance(Global.Player) > _range)
            {
                return;
            }

            var poly = new Geometry.Rectangle(Geometry.To2D(Global.Player.ServerPosition), Geometry.To2D(pos), _width);

            _timeUntilCasting = (int)(time - TravelTime(pos));

            if (GameObjects.EnemyHeroes.Count(x => poly.IsInside(Geometry.To2D(x.ServerPosition))) > _maxCollisionObjects || _timeUntilCasting > Game.Ping / 2f + 30)
            {
                return;
            }

            _ultimate.Cast(pos);
            _timeUntilCasting = -1;
            Reset();
        }

        private void OnRender()
        {
            if (_target == null || _timeUntilCasting == -1)
            {
                return;
            }

            var ts = TimeSpan.FromMilliseconds(_timeUntilCasting);
            Render.WorldToScreen(Global.Player.ServerPosition, out var player);
            Render.Text(new Vector2(player.X - 60, player.Y + 70), Color.Cyan, $"Ulting ({_target.ChampionName}) In {ts.Seconds}:{ts.Milliseconds / 10}");
        }

        private float TravelTime(Vector3 pos)
        {
            return Global.Player.Distance(pos) / _speed * 1000 + _delay + Game.Ping / 2f;
        }

        private float Damage()
        {
            if (_target == null)
            {
                return 0;
            }

            var hpReg = _target.BaseHPRegenRate;
            var dmg = (float)Global.Player.GetSpellDamage(_target, SpellSlot.R);
            return Math.Min(dmg, dmg + hpReg * TravelTime(GetFountainPos(_target)) / 1000);
        }

        private static Vector3 GetFountainPos(GameObject target)
        {
            switch (Game.MapId)
            {
                case GameMapId.SummonersRift:
                    return target.Team == GameObjectTeam.Order
                        ? new Vector3(396, 185.1325f, 462)
                        : new Vector3(14340, 171.9777f, 14390);

                case GameMapId.TwistedTreeline:
                    return target.Team == GameObjectTeam.Order
                        ? new Vector3(1058, 150.8638f, 7297)
                        : new Vector3(14320, 151.9291f, 7235);
            }
            return Vector3.Zero;
        }

        private void Set(float recall, int tickCount, Obj_AI_Hero target)
        {
            _recallTime = recall;
            _recallTick = tickCount;
            _target = target;
        }

        private void Reset()
        {
            _recallTime = 0;
            _recallTick = 0;
            _target = null;
            _timeUntilCasting = -1;
        }
    }
}
