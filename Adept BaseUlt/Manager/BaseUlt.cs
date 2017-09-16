using System.Threading;
using Aimtec.SDK.Util;

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

        private Vector3 _lastSeenPosition;
        private Vector3 _predictedPosition;
        private Vector3 _castPos;

        private int _recallStartTick;
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

            lastEnemyChecked = new List<Obj_AI_Hero>();
            foreach (var enemies in GameObjects.EnemyHeroes)
            {
                lastEnemyChecked.Add(enemies);
            }

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

            Menu.Add(new MenuSeperator("no"));
            Menu.Add(new MenuSlider("Distance", "Max Distance | RandomUlt", 2000, 500, 4000));
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
                Set(args.Duration, Game.TickCount, (Obj_AI_Hero)sender);
            }
        }

        private readonly List<Obj_AI_Hero> lastEnemyChecked;

        private void OnUpdate()
        {
            if (Menu["RandomUlt"].Enabled)
            {

                foreach (var enemy in lastEnemyChecked.Where(x =>
                    x.IsFloatingHealthBarActive && !x.IsDead && x.IsValidTarget()))
                {
                    if (Game.TickCount - _lastSeenTick <= Game.Ping / 2f)
                    {
                        return;
                    }

                    _lastSeenTick = Game.TickCount;
                    _lastSeenPosition = enemy.ServerPosition;
                }
            }

            if (_target == null
             || !Menu[_target.ChampionName].Enabled
             || !_target.IsValid
             || !_ultimate.Ready
             || _target.Health > Damage())
            {
                return;
            }

            if (Menu["RandomUlt"].Enabled)
            {
                var enemy = lastEnemyChecked.FirstOrDefault(x => x.NetworkId == _target.NetworkId);
                if (enemy == null)
                {
                    return;
                }

                var direction = _target.ServerPosition + (_target.ServerPosition - enemy.ServerPosition).Normalized();
                _predictedPosition = enemy.ServerPosition.Extend(direction, (_recallStartTick - _lastSeenTick) / 1000f * enemy.MoveSpeed);

                var distance = (_recallStartTick - _lastSeenTick) / 1000f * _target.MoveSpeed;

                _castPos = _lastSeenPosition.Extend(_predictedPosition, distance);

                if (distance > Menu["Distance"].Value)
                {
                    return;
                }

                Console.WriteLine("RANDOM ULT SUCCESS");
                CastUlt(_castPos);
            }
            else
            {
                _timeUntilCastingUlt = GetCastTime(GetFountainPos(_target));

                if (_timeUntilCastingUlt <= Game.Ping)
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

            DelayAction.Queue(1500, () =>
            {
                _lastSeenPosition = Vector3.Zero;
                _predictedPosition = Vector3.Zero;
                _castPos = Vector3.Zero;
            }, new CancellationToken(false));

            _ultimate.Cast(pos);

            Reset();
        }

        private int GetCastTime(Vector3 pos)
        {
            return (int)(-(Game.TickCount - (_recallStartTick + _recallTime)) - TravelTime(pos));
        }

        private void OnRender()
        {
            if (_lastSeenPosition != Vector3.Zero && _castPos != Vector3.Zero)
            {
                Render.Circle(_lastSeenPosition, 50, 100, Color.White);
                Render.Circle(_castPos, _width, 100, Color.Red);

                Render.WorldToScreen(_castPos, out var castVector2);
                Render.Text(castVector2, Color.White, "Random Ult");

                Render.WorldToScreen(_lastSeenPosition, out var lsV2);
                Render.WorldToScreen(_castPos, out var ppV2);
                Render.Line(lsV2, ppV2, Color.Orange);
            }

            if (_timeUntilCastingUlt == -1 || _target == null)
            {
                return;
            }
            var ts = TimeSpan.FromMilliseconds(_timeUntilCastingUlt);
            var percent = (_recallStartTick - Game.TickCount + _recallTime) / _recallTime;
         
            var xpos = 650;

            Render.Line(xpos,
                        80,
                        xpos + 200,
                        80,
                        18, false, Color.LightSlateGray);

            Render.Line(xpos, 
                        80, 
                        xpos + 200 * percent, 
                        80, 
                        16, false, Color.LightSeaGreen);


            Render.Text(xpos + 100, 70, Color.White, _target.ChampionName);
            Render.WorldToScreen(Global.Player.ServerPosition, out var player);

            Render.Text(new Vector2(player.X - 60, player.Y + 70), Color.Cyan,
                $"Ulting ({_target.ChampionName}) In {ts.Seconds}:{ts.Milliseconds / 10}");
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

        private void Set(float recall, int tickCount, Obj_AI_Hero target, int timeUntilCastingUlt = -1)
        {
            _recallTime = recall;
            _recallStartTick = tickCount;
            _target = target;
            _timeUntilCastingUlt = timeUntilCastingUlt;
        }

        private void Reset()
        {
            _recallTime = 0;
            _recallStartTick = 0;
            _target = null;
            _timeUntilCastingUlt = -1;
        }
    }
}