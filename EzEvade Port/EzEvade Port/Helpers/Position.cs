﻿namespace EzEvade_Port.Helpers
{
    using System;
    using System.Collections.Generic;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;
    using Spells;
    using Utils;
    using Spell = Spells.Spell;

    public static class Position
    {
        private static Obj_AI_Hero myHero => ObjectManager.GetLocalPlayer();

        public static int CheckPosDangerLevel(this Vector2 pos, float extraBuffer)
        {
            var dangerlevel = 0;
            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;

                if (pos.InSkillShot(spell, ObjectCache.myHeroCache.boundingRadius + extraBuffer))
                {
                    dangerlevel += spell.Dangerlevel;
                }
            }
            return dangerlevel;
        }

        public static bool InSkillShot(this Vector2 position, Spell spell, float radius, bool predictCollision = true)
        {
            if (spell.SpellType == SpellType.Line)
            {
                var spellPos = spell.CurrentSpellPosition;
                var spellEndPos = predictCollision ? spell.GetSpellEndPosition() : spell.EndPos;

                var projection = position.ProjectOn(spellPos, spellEndPos);
                return projection.IsOnSegment && projection.SegmentPoint.Distance(position) <= spell.Radius + radius;
            }

            if (spell.SpellType == SpellType.Circular)
            {
                if (spell.Info.SpellName == "VeigarEventHorizon")
                {
                    return position.Distance(spell.EndPos) <= spell.Radius + radius - ObjectCache.myHeroCache.boundingRadius &&
                           position.Distance(spell.EndPos) >= spell.Radius + radius - ObjectCache.myHeroCache.boundingRadius - 125;
                }
                if (spell.Info.SpellName == "DariusCleave")
                {
                    return position.Distance(spell.EndPos) <= spell.Radius + radius - ObjectCache.myHeroCache.boundingRadius &&
                           position.Distance(spell.EndPos) >= spell.Radius + radius - ObjectCache.myHeroCache.boundingRadius - 220;
                }

                return position.Distance(spell.EndPos) <= spell.Radius + radius - ObjectCache.myHeroCache.boundingRadius;
            }

            if (spell.SpellType == SpellType.Arc)
            {
                if (position.isLeftOfLineSegment(spell.StartPos, spell.EndPos))
                {
                    return false;
                }

                var spellRange = spell.StartPos.Distance(spell.EndPos);
                var midPoint = spell.StartPos + spell.Direction * (spellRange / 2);

                return position.Distance(midPoint) <= spell.Radius + radius - ObjectCache.myHeroCache.boundingRadius;
            }

            if (spell.SpellType == SpellType.Cone)
            {
                return !position.isLeftOfLineSegment(spell.CnStart, spell.CnLeft) && !position.isLeftOfLineSegment(spell.CnLeft, spell.CnRight) &&
                       !position.isLeftOfLineSegment(spell.CnRight, spell.CnStart);
            }

            return false;
        }

        public static bool isLeftOfLineSegment(this Vector2 pos, Vector2 start, Vector2 end)
        {
            return (end.X - start.X) * (pos.Y - start.Y) - (end.Y - start.Y) * (pos.X - start.X) > 0;
        }

        public static float GetDistanceToTurrets(this Vector2 pos)
        {
            var minDist = float.MaxValue;

            foreach (var entry in ObjectCache.turrets)
            {
                var turret = entry.Value;
                if (turret == null || !turret.IsValid || turret.IsDead)
                {
                    DelayAction.Add(1, () => ObjectCache.turrets.Remove(entry.Key));
                    continue;
                }

                if (turret.IsAlly)
                {
                    continue;
                }

                var distToTurret = pos.Distance(turret.Position.To2D());

                minDist = Math.Min(minDist, distToTurret);
            }

            return minDist;
        }

        public static float GetDistanceToChampions(this Vector2 pos)
        {
            var minDist = float.MaxValue;

            foreach (var hero in GameObjects.EnemyHeroes)
            {
                if (hero != null && hero.IsValid && !hero.IsDead && hero.IsVisible)
                {
                    var heroPos = hero.ServerPosition.To2D();
                    var dist = heroPos.Distance(pos);

                    minDist = Math.Min(minDist, dist);
                }
            }

            return minDist;
        }

        public static bool HasExtraAvoidDistance(this Vector2 pos, float extraBuffer)
        {
            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;

                if (spell.SpellType == SpellType.Line)
                {
                    if (pos.InSkillShot(spell, ObjectCache.myHeroCache.boundingRadius + extraBuffer))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static float GetEnemyPositionValue(this Vector2 pos)
        {
            float posValue = 0;

            if (ObjectCache.menuCache.cache["PreventDodgingNearEnemy"].Enabled)
            {
                var minComfortDistance = ObjectCache.menuCache.cache["MinComfortZone"].As<MenuSlider>().Value;

                foreach (var hero in GameObjects.EnemyHeroes)
                {
                    if (hero != null && hero.IsValid && !hero.IsDead && hero.IsVisible)
                    {
                        var heroPos = hero.ServerPosition.To2D();
                        var dist = heroPos.Distance(pos);

                        if (minComfortDistance > dist)
                        {
                            posValue += 2 * (minComfortDistance - dist);
                        }
                    }
                }
            }

            return posValue;
        }

        public static float GetPositionValue(this Vector2 pos)
        {
            var posValue = pos.Distance(Game.CursorPos.To2D());

            if (ObjectCache.menuCache.cache["PreventDodgingUnderTower"].Enabled)
            {
                var turretRange = 875 + ObjectCache.myHeroCache.boundingRadius;
                var distanceToTurrets = pos.GetDistanceToTurrets();

                if (turretRange > distanceToTurrets)
                {
                    posValue += 5 * (turretRange - distanceToTurrets);
                }
            }

            return posValue;
        }

        public static bool CheckDangerousPos(this Vector2 pos, float extraBuffer, bool checkOnlyDangerous = false)
        {
            foreach (var entry in SpellDetector.Spells)
            {
                var spell = entry.Value;

                if (checkOnlyDangerous && spell.Dangerlevel < 3)
                {
                    continue;
                }

                if (pos.InSkillShot(spell, ObjectCache.myHeroCache.boundingRadius + extraBuffer))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Vector2> GetSurroundingPositions(int maxPosToCheck = 150, int posRadius = 25)
        {
            var positions = new List<Vector2>();

            var posChecked = 0;
            var radiusIndex = 0;

            var heroPoint = ObjectCache.myHeroCache.serverPos2D;
            var lastMovePos = Game.CursorPos.To2D();

            var posTable = new List<PositionInfo>();

            while (posChecked < maxPosToCheck)
            {
                radiusIndex++;

                var curRadius = radiusIndex * 2 * posRadius;
                var curCircleChecks = (int) Math.Ceiling(2 * Math.PI * curRadius / (2 * (double) posRadius));

                for (var i = 1; i < curCircleChecks; i++)
                {
                    posChecked++;
                    var cRadians = 2 * Math.PI / (curCircleChecks - 1) * i; //check decimals
                    var pos = new Vector2((float) Math.Floor(heroPoint.X + curRadius * Math.Cos(cRadians)), (float) Math.Floor(heroPoint.Y + curRadius * Math.Sin(cRadians)));

                    positions.Add(pos);
                }
            }

            return positions;
        }
    }
}