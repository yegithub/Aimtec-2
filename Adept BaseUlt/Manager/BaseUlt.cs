namespace Adept_BaseUlt.Manager
{
    using System.Collections.Generic;
    using Aimtec;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Spell = Aimtec.SDK.Spell;
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

        private int _timeUntilCastingUlt = -1;

        private int _lastSeenTick;
        private int _lastSeenId;

        private Vector3 _lastSeenPosition;
        private Vector3 _predictedPosition;
        private Vector3 _castPos;

        private int _recallTick;
        private float _recallTime;

        private Obj_AI_Hero _target;

        public BaseUlt(float speed, float width, float delay, int maxCollisionObjects = int.MaxValue,
            float range = float.MaxValue)
        {
            _ultimate = new Spell(SpellSlot.R, _range);

            this._range = range;
            this._speed = speed;
            this._width = width;
            this._delay = delay;
            this._maxCollisionObjects = maxCollisionObjects;

            AttatchMenu();
            Global.Init();

            hotfixDirection = new List<Obj_AI_Hero>();

            Teleport.OnTeleport += OnTeleport;
            Game.OnUpdate += OnUpdate;
            Render.OnRender += OnRender;
        }

        private static Menu Menu;

        private static void AttatchMenu()
        {
            Menu = new Menu("hello", "Adept - BaseUlt", true);
            Menu.Attach();

            Menu.Add(new MenuBool("RandomUlt", "Use RandomUlt").SetToolTip(
                "Will GUESS the enemy position and ult there"));

            Menu.Add(new MenuSeperator("yes", "Whitelist"));

            foreach (var hero in GameObjects.EnemyHeroes)
            {
                Menu.Add(new MenuBool(hero.ChampionName, "ULT: " + hero.ChampionName));
            }
        }

        private void OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            if (!sender.IsEnemy
                || !sender.IsValid
                || sender.IsDead)
            {
                return;
            }

            if (args.Status == TeleportStatus.Abort || args.Status == TeleportStatus.Unknown ||
                args.Status == TeleportStatus.Finish)
            {
                Reset();
            }

            else if (args.Type == TeleportType.Recall)
            {
                Set(args.Duration, Game.TickCount, (Obj_AI_Hero) sender);
            }
        }

        private readonly List<Obj_AI_Hero> hotfixDirection;
        private int hotfixTick;

        private void OnUpdate()
        {
            if (Menu["RandomUlt"].Enabled)
            {
                foreach (var enemy in GameObjects.EnemyHeroes.Where(x =>
                    x.IsFloatingHealthBarActive && !x.IsDead && x.IsValidTarget()))
                {
                    if (Game.TickCount - hotfixTick <= 80)
                    {
                        continue;
                    }

                    _lastSeenTick = Game.TickCount;
                    _lastSeenId = enemy.NetworkId;
                    _lastSeenPosition = enemy.ServerPosition;

                    hotfixDirection.Add(enemy);
                    hotfixTick = Game.TickCount;
                }

                if (hotfixDirection.Count >= 5)
                {
                    hotfixDirection.Clear();
                }
            }

            if (_target == null || !Menu[_target.ChampionName].Enabled || !_target.IsValid || !_ultimate.Ready ||
                _target.Health > Damage())
            {
                return;
            }

            var fountainPos = GetFountainPos(_target);

            if (Menu["RandomUlt"].Enabled)
            {
                var hotfixID = hotfixDirection.FirstOrDefault(x => x.NetworkId == _target.NetworkId);
                if (hotfixID == null)
                {
                    return;
                }

                var direction = _target.ServerPosition +
                                (_target.ServerPosition - hotfixID.ServerPosition).Normalized();
                _predictedPosition = _target.ServerPosition.Extend(direction,
                    _target.MoveSpeed * ((_lastSeenTick - _recallTick) / 1000f));

                var distance = (_recallTick - _lastSeenTick) / 1000f * _target.MoveSpeed;
                _castPos = _lastSeenPosition.Extend(_predictedPosition, distance);

                if (distance > 2200 || Global.Player.Distance(fountainPos) < Global.Player.Distance(_castPos))
                {
                    return;
                }

                Console.WriteLine("RANDOM ULT SUCCESS");
                CastUlt(_castPos);
            }
            else
            {
                _timeUntilCastingUlt = GetCastTime(fountainPos);

                if (_timeUntilCastingUlt <= Game.Ping / 2f + 30)
                {
                    CastUlt(GetFountainPos(_target));
                }
            }
        }

        private void CastUlt(Vector3 pos)
        {
            var rectangle =
                new Geometry.Rectangle(Geometry.To2D(Global.Player.ServerPosition), Geometry.To2D(pos), _width);

            if (GameObjects.EnemyHeroes.Count(x => rectangle.IsInside(Geometry.To2D(x.ServerPosition))) >
                _maxCollisionObjects || pos.Distance(Global.Player) > _range)
            {
                return;
            }

            Console.WriteLine($"BASEULT SUCCESS | {_target.ChampionName}");
            _ultimate.Cast(pos);
            Reset();
        }

        private int GetCastTime(Vector3 pos)
        {
            return (int) (-(Game.TickCount - (_recallTick + _recallTime)) - TravelTime(pos));
        }

        private void OnRender()
        {
            if (_target == null)
            {
                return;
            }

            if (_timeUntilCastingUlt != -1)
            {
                var ts = TimeSpan.FromMilliseconds(_timeUntilCastingUlt);
                Render.WorldToScreen(Global.Player.ServerPosition, out var player);
                Render.Text(new Vector2(player.X - 60, player.Y + 70), Color.Cyan,
                    $"Ulting ({_target.ChampionName}) In {ts.Seconds}:{ts.Milliseconds / 10}");
            }

            if (!_castPos.IsZero)
            {
                Render.Circle(_castPos, 50, 100, Color.Red);
            }

            if (!_lastSeenPosition.IsZero)
            {
                Render.Circle(_lastSeenPosition, 50, 100, Color.White);
            }

            if (_castPos != Vector3.Zero && _lastSeenPosition != Vector3.Zero)
            {
                Render.WorldToScreen(_lastSeenPosition, out var lsV2);
                Render.WorldToScreen(_castPos, out var ppV2);
                Render.Line(lsV2, ppV2, Color.Orange);
            }
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
            var dmg = (float) Global.Player.GetSpellDamage(_target, SpellSlot.R);
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

        private void Set(float recall, int tickCount, Obj_AI_Hero target, int timeUntilCastingUlt = -1)
        {
            _recallTime = recall;
            _recallTick = tickCount;
            _target = target;
            _timeUntilCastingUlt = timeUntilCastingUlt;
        }

        private void Reset()
        {
            _recallTime = 0;
            _recallTick = 0;
            _target = null;
            _timeUntilCastingUlt = -1;
        }
    }
}