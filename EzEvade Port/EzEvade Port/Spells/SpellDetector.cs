﻿namespace EzEvade_Port.Spells
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Util.Cache;
    using Core;
    using Helpers;
    using SpecialSpells;
    using Utils;

    public class SpecialSpellEventArgs : EventArgs
    {
        public bool NoProcess { get; set; }
        public SpellData SpellData { get; set; }
    }

    class SpellDetector
    {
        public delegate void OnProcessDetectedSpellsHandler();

        public delegate void OnProcessSpecialSpellHandler(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args, SpellData spellData, SpecialSpellEventArgs specialSpellArgs);

        //public static event OnDeleteSpellHandler OnDeleteSpell;

        public static Dictionary<int, Spell> Spells = new Dictionary<int, Spell>();
        public static Dictionary<int, Spell> DrawSpells = new Dictionary<int, Spell>();
        public static Dictionary<int, Spell> DetectedSpells = new Dictionary<int, Spell>();

        public static Dictionary<string, ChampionPlugin> ChampionPlugins = new Dictionary<string, ChampionPlugin>();

        public static Dictionary<string, string> ChanneledSpells = new Dictionary<string, string>();

        public static Dictionary<string, SpellData> OnProcessTraps = new Dictionary<string, SpellData>();
        public static Dictionary<string, SpellData> OnProcessSpells = new Dictionary<string, SpellData>();
        public static Dictionary<string, SpellData> OnMissileSpells = new Dictionary<string, SpellData>();

        public static Dictionary<string, SpellData> WindupSpells = new Dictionary<string, SpellData>();

        private static int _spellIdCount;

        public static float LastCheckTime;
        public static float LastCheckSpellCollisionTime;

        //public static Menu menu;
        public static Menu TrapMenu;

        public SpellDetector(Menu mainMenu)
        {
            GameObject.OnCreate += SpellMissile_OnCreate;
            GameObject.OnDestroy += SpellMissile_OnDelete;

            Obj_AI_Base.OnProcessSpellCast += Game_ProcessSpell;

            Game.OnUpdate += Game_OnGameUpdate;

            Evade.SpellMenu = new Menu("Spells", "Spells");
            mainMenu.Add(Evade.SpellMenu);

            TrapMenu = new Menu("Traps", "Traps");
            mainMenu.Add(TrapMenu);

            LoadSpellDictionary();
            InitChannelSpells();
        }

        private static Obj_AI_Hero MyHero => ObjectManager.GetLocalPlayer();
        public static event OnProcessDetectedSpellsHandler OnProcessDetectedSpells;
        public static event OnProcessSpecialSpellHandler OnProcessSpecialSpell;

        private void SpellMissile_OnCreate(GameObject obj)
        {
            if (!obj.IsValid || obj.Type != GameObjectType.MissileClient)
            {
                return;
            }

            var missile = (MissileClient) obj;

            SpellData spellData;

            if (missile.SpellCaster != null && missile.SpellCaster.CheckTeam() && missile.SpellData.Name != null && OnMissileSpells.TryGetValue(missile.SpellData.Name.ToLower(), out spellData) &&
                missile.StartPosition != null && missile.EndPosition != null)
            {
                if (missile.StartPosition.Distance(MyHero.Position) < spellData.Range + 1000)
                {
                    var hero = missile.SpellCaster;

                    if (hero.IsVisible)
                    {
                        if (spellData.UsePackets)
                        {
                            CreateSpellData(hero, missile.StartPosition, missile.EndPosition, spellData, obj);
                            return;
                        }

                        var objectAssigned = false;

                        foreach (var entry in DetectedSpells)
                        {
                            var spell = entry.Value;

                            var dir = (missile.EndPosition.To2D() - missile.StartPosition.To2D()).Normalized();

                            if (spell.Info.MissileName.ToLower() == missile.SpellData.Name.ToLower()) // todo: fix urf spells
                            {
                                if (spell.HeroId == hero.NetworkId && dir.AngleBetween(spell.Direction) < 10)
                                {
                                    if (spell.Info.IsThreeWay == false && spell.Info.IsSpecial == false)
                                    {
                                        spell.SpellObject = obj;
                                        objectAssigned = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (objectAssigned == false)
                        {
                            CreateSpellData(hero, missile.StartPosition, missile.EndPosition, spellData, obj);
                        }
                    }
                    else
                    {
                        if (ObjectCache.MenuCache.Cache["DodgeFOWSpells"].Enabled)
                        {
                            CreateSpellData(hero, missile.StartPosition, missile.EndPosition, spellData, obj);
                        }
                    }
                }
            }
        }

        private void SpellMissile_OnDelete(GameObject obj)
        {
            if (!obj.IsValid || obj.Type != GameObjectType.MissileClient)
            {
                return;
            }

            var missile = (MissileClient) obj;

            foreach (var spell in Spells.Values.ToList().Where(s => s.SpellObject != null && s.SpellObject.NetworkId == obj.NetworkId)) //isAlive
            {
                if (!spell.Info.Name.Contains("_trap"))
                {
                    DelayAction.Add(1, () => DeleteSpell(spell.SpellId));
                }
            }
        }

        public void RemoveNonDangerousSpells()
        {
            foreach (var spell in Spells.Values.ToList().Where(s => s != null && s.GetSpellDangerLevel() < 3))
            {
                DelayAction.Add(1, () => DeleteSpell(spell.SpellId));
            }
        }

        private void Game_ProcessSpell(Obj_AI_Base hero, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            try
            {
                SpellData spellData;
                if (hero.CheckTeam() && OnProcessSpells.TryGetValue(args.SpellData.Name.ToLower(), out spellData))
                {
                    if (spellData.UsePackets == false)
                    {
                        var specialSpellArgs = new SpecialSpellEventArgs {SpellData = spellData};
                        OnProcessSpecialSpell?.Invoke(hero, args, spellData, specialSpellArgs);

                        // optional update from specialSpellArgs
                        spellData = specialSpellArgs.SpellData;

                        if (specialSpellArgs.NoProcess == false && spellData.NoProcess == false)
                        {
                            var foundMissile = false;

                            if (spellData.IsThreeWay == false && spellData.IsSpecial == false)
                            {
                                foreach (var entry in DetectedSpells)
                                {
                                    var spell = entry.Value;

                                    var dir = (args.End.To2D() - args.Start.To2D()).Normalized();
                                    if (spell.SpellObject != null)
                                    {
                                        if (spell.Info.SpellName.ToLower() == args.SpellData.Name.ToLower()) // todo: fix urf spells
                                        {
                                            if (spell.HeroId == hero.NetworkId && dir.AngleBetween(spell.Direction) < 10)
                                            {
                                                foundMissile = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (foundMissile == false)
                            {
                                CreateSpellData(hero, hero.ServerPosition, args.End, spellData);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void CreateSpellData(Obj_AI_Base hero,
                                           Vector3 spellStartPos,
                                           Vector3 spellEndPos,
                                           SpellData spellData,
                                           GameObject obj = null,
                                           float extraEndTick = 0.0f,
                                           bool processSpell = true,
                                           SpellType spellType = SpellType.None,
                                           bool checkEndExplosion = true,
                                           float spellRadius = 0)
        {
            if (checkEndExplosion && spellData.HasEndExplosion)
            {
                CreateSpellData(hero, spellStartPos, spellEndPos, spellData, obj, extraEndTick, false, spellData.SpellType, false);

                CreateSpellData(hero, spellStartPos, spellEndPos, spellData, obj, extraEndTick, true, SpellType.Circular, false);

                return;
            }

            if (spellStartPos.Distance(MyHero.Position) < spellData.Range + 1000)
            {
                var startPosition = spellStartPos.To2D();
                var endPosition = spellEndPos.To2D();
                var direction = (endPosition - startPosition).Normalized();
                var endTick = 0f;

                if (spellType == SpellType.None)
                {
                    spellType = spellData.SpellType;
                }

                if (spellData.FixedRange) //for diana q
                {
                    if (endPosition.Distance(startPosition) > spellData.Range)
                    {
                        endPosition = startPosition + direction * spellData.Range;
                    }
                }

                if (spellType == SpellType.Line)
                {
                    endTick = spellData.SpellDelay + spellData.Range / spellData.ProjectileSpeed * 1000;
                    endPosition = startPosition + direction * spellData.Range;

                    if (spellData.FixedRange) // for all lines
                    {
                        if (endPosition.Distance(startPosition) < spellData.Range)
                        {
                            endPosition = startPosition + direction * spellData.Range;
                        }
                    }

                    if (endPosition.Distance(startPosition) > spellData.Range)
                    {
                        endPosition = startPosition + direction * spellData.Range;
                    }

                    if (spellData.UseEndPosition)
                    {
                        var range = endPosition.Distance(startPosition);
                        endTick = spellData.SpellDelay + range / spellData.ProjectileSpeed * 1000;
                    }

                    if (obj != null)
                    {
                        endTick -= spellData.SpellDelay;
                    }
                }
                else if (spellType == SpellType.Circular)
                {
                    endTick = spellData.SpellDelay;

                    if (endPosition.Distance(startPosition) > spellData.Range)
                    {
                        endPosition = startPosition + direction * spellData.Range;
                    }

                    if (spellData.ProjectileSpeed == 0 && hero != null)
                    {
                        endPosition = hero.ServerPosition.To2D();
                    }
                    else if (spellData.ProjectileSpeed > 0)
                    {
                        endTick = endTick + 1000 * startPosition.Distance(endPosition) / spellData.ProjectileSpeed;

                        if (spellData.SpellType == SpellType.Line && spellData.HasEndExplosion)
                        {
                            if (!spellData.UseEndPosition)
                            {
                                endPosition = startPosition + direction * spellData.Range;
                            }
                        }
                    }
                }
                else if (spellType == SpellType.Arc)
                {
                    endTick = endTick + 1000 * startPosition.Distance(endPosition) / spellData.ProjectileSpeed;

                    if (obj != null)
                    {
                        endTick -= spellData.SpellDelay;
                    }
                }
                else if (spellType == SpellType.Cone)
                {
                    endPosition = startPosition + direction * spellData.Range;
                    endTick = spellData.SpellDelay;

                    if (endPosition.Distance(startPosition) > spellData.Range)
                    {
                        endPosition = startPosition + direction * spellData.Range;
                    }

                    if (spellData.ProjectileSpeed == 0 && hero != null)
                    {
                        endPosition = hero.ServerPosition.To2D();
                    }
                    else if (spellData.ProjectileSpeed > 0)
                    {
                        endTick = endTick + 1000 * startPosition.Distance(endPosition) / spellData.ProjectileSpeed;
                    }
                }
                else
                {
                    return;
                }

                if (spellData.Invert)
                {
                    var dir = (startPosition - endPosition).Normalized();
                    endPosition = startPosition + dir * startPosition.Distance(endPosition);
                }

                if (spellData.IsPerpendicular)
                {
                    startPosition = spellEndPos.To2D() - direction.Perpendicular() * spellData.SecondaryRadius;
                    endPosition = spellEndPos.To2D() + direction.Perpendicular() * spellData.SecondaryRadius;
                }

                endTick += extraEndTick;

                var newSpell = new Spell();
                newSpell.StartTime = Environment.TickCount;
                newSpell.EndTime = Environment.TickCount + endTick;
                newSpell.StartPos = startPosition;
                newSpell.EndPos = endPosition;
                newSpell.Height = spellEndPos.Z + spellData.ExtraDrawHeight;
                newSpell.Direction = direction;
                newSpell.Info = spellData;
                newSpell.SpellType = spellType;
                newSpell.Radius = spellRadius > 0 ? spellRadius : newSpell.GetSpellRadius();

                if (spellType == SpellType.Cone)
                {
                    newSpell.Radius = 100 + newSpell.Radius * 3; // for now.. eh
                    newSpell.CnStart = startPosition + direction;
                    newSpell.CnLeft = endPosition + direction.Perpendicular() * newSpell.Radius;
                    newSpell.CnRight = endPosition - direction.Perpendicular() * newSpell.Radius;
                }

                if (hero != null)
                {
                    newSpell.HeroId = hero.NetworkId;
                }

                if (obj != null)
                {
                    newSpell.SpellObject = obj;
                    newSpell.ProjectileId = obj.NetworkId;
                }

                var spellId = CreateSpell(newSpell, processSpell);

                if (extraEndTick != 1337f) // traps
                {
                    DelayAction.Add((int) (endTick + spellData.ExtraEndTime), () => DeleteSpell(spellId));
                }
            }
        }

        private void Game_OnGameUpdate()
        {
            UpdateSpells();

            if (Environment.TickCount - LastCheckSpellCollisionTime > 100)
            {
                CheckSpellCollision();
                LastCheckSpellCollisionTime = Environment.TickCount;
            }

            if (Environment.TickCount - LastCheckTime > 1)
            {
                //CheckCasterDead();                
                CheckSpellEndTime();
                AddDetectedSpells();
                LastCheckTime = Environment.TickCount;
            }
        }

        public static void UpdateSpells()
        {
            foreach (var spell in DetectedSpells.Values)
            {
                spell.UpdateSpellInfo();
            }
        }

        private void CheckSpellEndTime()
        {
            foreach (var entry in DetectedSpells)
            {
                var spell = entry.Value;
                if (spell.Info.SpellName.Contains("_trap"))
                {
                    continue;
                }

                foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(e => e.IsEnemy))
                {
                    if (hero.IsDead && spell.HeroId == hero.NetworkId)
                    {
                        if (spell.SpellObject == null)
                        {
                            DelayAction.Add(1, () => DeleteSpell(entry.Key));
                        }
                    }
                }

                if (spell.EndTime + spell.Info.ExtraEndTime < Environment.TickCount || CanHeroWalkIntoSpell(spell) == false)
                {
                    DelayAction.Add(1, () => DeleteSpell(entry.Key));
                }
            }
        }

        private static void CheckSpellCollision()
        {
            if (!ObjectCache.MenuCache.Cache["CheckSpellCollision"].Enabled)
            {
                return;
            }

            foreach (var entry in DetectedSpells)
            {
                if (entry.Value == null)
                {
                    return;
                }

                var spell = entry.Value;
                if (spell == null)
                {
                    return;
                }

                var collisionObject = spell.CheckSpellCollision();
                if (collisionObject != null)
                {
                    spell.PredictedEndPos = spell.GetSpellProjection(collisionObject.ServerPosition.To2D());

                    if (spell.CurrentSpellPosition.Distance(collisionObject.ServerPosition) < collisionObject.BoundingRadius + spell.Radius)
                    {
                        DelayAction.Add(1, () => DeleteSpell(entry.Key));
                    }
                }
            }
        }

        public static bool CanHeroWalkIntoSpell(Spell spell)
        {
            if (ObjectCache.MenuCache.Cache["AdvancedSpellDetection"].Enabled)
            {
                var heroPos = MyHero.Position.To2D();
                var extraDist = MyHero.Distance(ObjectCache.MyHeroCache.ServerPos2D);

                if (spell.SpellType == SpellType.Line)
                {
                    var walkRadius = ObjectCache.MyHeroCache.MoveSpeed * (spell.EndTime - Environment.TickCount) / 1000 + ObjectCache.MyHeroCache.BoundingRadius + spell.Info.Radius + extraDist + 10;
                    var spellPos = spell.CurrentSpellPosition;
                    var spellEndPos = spell.GetSpellEndPosition();

                    var projection = heroPos.ProjectOn(spellPos, spellEndPos);

                    return projection.SegmentPoint.Distance(heroPos) <= walkRadius;
                }
                if (spell.SpellType == SpellType.Circular)
                {
                    var walkRadius = ObjectCache.MyHeroCache.MoveSpeed * (spell.EndTime - Environment.TickCount) / 1000 + ObjectCache.MyHeroCache.BoundingRadius + spell.Info.Radius + extraDist + 10;

                    if (heroPos.Distance(spell.EndPos) < walkRadius)
                    {
                        return true;
                    }
                }
                else if (spell.SpellType == SpellType.Arc)
                {
                    var spellRange = spell.StartPos.Distance(spell.EndPos);
                    var midPoint = spell.StartPos + spell.Direction * (spellRange / 2);
                    var arcRadius = spell.Info.Radius * (1 + spellRange / 100);

                    var walkRadius = ObjectCache.MyHeroCache.MoveSpeed * (spell.EndTime - Environment.TickCount) / 1000 + ObjectCache.MyHeroCache.BoundingRadius + arcRadius + extraDist + 10;

                    if (heroPos.Distance(midPoint) < walkRadius)
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        private static void AddDetectedSpells()
        {
            var spellAdded = false;

            foreach (var entry in DetectedSpells)
            {
                var spell = entry.Value;
                if (spell.Info.SpellName.Contains("_trap"))
                {
                    // todo:
                }
                else
                {
                    EvadeHelper.FastEvadeMode = Evade.SpellMenu[spell.Info.CharName + spell.Info.SpellName + "Settings"][spell.Info.SpellName + "FastEvade"].Enabled;
                }

                float evadeTime, spellHitTime;
                spell.CanHeroEvade(MyHero, out evadeTime, out spellHitTime);

                spell.SpellHitTime = spellHitTime;
                spell.EvadeTime = evadeTime;

                var extraDelay = ObjectCache.GamePing + ObjectCache.MenuCache.Cache["ExtraPingBuffer"].As<MenuSlider>().Value;

                if (spell.SpellHitTime - extraDelay < 1500 && CanHeroWalkIntoSpell(spell))
                    //if(true)
                {
                    var newSpell = spell;
                    var spellId = spell.SpellId;

                    if (!DrawSpells.ContainsKey(spell.SpellId))
                    {
                        DrawSpells.Add(spellId, newSpell);
                    }

                    //var spellFlyTime = Evade.GetTickCount - spell.startTime;
                    if (spellHitTime < ObjectCache.MenuCache.Cache["SpellDetectionTime"].As<MenuSlider>().Value &&
                        !Evade.SpellMenu[spell.Info.CharName + spell.Info.SpellName + "Settings"][spell.Info.SpellName + "FastEvade"].Enabled)
                    {
                        continue;
                    }

                    if (Environment.TickCount - spell.StartTime < ObjectCache.MenuCache.Cache["ReactionTime"].As<MenuSlider>().Value &&
                        !Evade.SpellMenu[spell.Info.CharName + spell.Info.SpellName + "Settings"][spell.Info.SpellName + "FastEvade"].Enabled)
                    {
                        continue;
                    }

                    var dodgeInterval = ObjectCache.MenuCache.Cache["DodgeInterval"].As<MenuSlider>().Value;
                    if (Evade.LastPosInfo != null && dodgeInterval > 0)
                    {
                        var timeElapsed = Environment.TickCount - Evade.LastPosInfo.Timestamp;

                        if (dodgeInterval > timeElapsed && !Evade.SpellMenu[spell.Info.CharName + spell.Info.SpellName + "Settings"][spell.Info.SpellName + "FastEvade"].Enabled)
                        {
                            continue;
                        }
                    }

                    if (!Spells.ContainsKey(spell.SpellId))
                    {
                        if (!(Evade.IsDodgeDangerousEnabled() && newSpell.GetSpellDangerLevel() < 3) &&
                            Evade.SpellMenu[spell.Info.CharName + spell.Info.SpellName + "Settings"][newSpell.Info.SpellName + "DodgeSpell"].Enabled)
                        {
                            if (newSpell.SpellType == SpellType.Circular && !ObjectCache.MenuCache.Cache["DodgeCircularSpells"].Enabled)
                            {
                                continue;
                            }

                            var healthThreshold = Evade.SpellMenu[spell.Info.CharName + spell.Info.SpellName + "Settings"][spell.Info.SpellName + "DodgeIgnoreHP"].As<MenuSlider>().Value;
                            if (MyHero.HealthPercent() <= healthThreshold)
                            {
                                Spells.Add(spellId, newSpell);
                                spellAdded = true;
                            }
                        }
                    }

                    if (ObjectCache.MenuCache.Cache["CheckSpellCollision"].Enabled && spell.PredictedEndPos != Vector2.Zero)
                    {
                        spellAdded = false;
                    }
                }
            }

            if (spellAdded)
            {
                OnProcessDetectedSpells?.Invoke();
            }
        }

        private static int CreateSpell(Spell newSpell, bool processSpell = true)
        {
            var spellId = _spellIdCount++;
            newSpell.SpellId = spellId;

            newSpell.UpdateSpellInfo();
            DetectedSpells.Add(spellId, newSpell);

            if (processSpell)
            {
                CheckSpellCollision();
                AddDetectedSpells();
            }

            return spellId;
        }

        public static void DeleteSpell(int spellId)
        {
            Spells.Remove(spellId);
            DrawSpells.Remove(spellId);
            DetectedSpells.Remove(spellId);
        }

        public static int GetCurrentSpellId()
        {
            return _spellIdCount;
        }

        public static List<int> GetSpellList()
        {
            var spellList = new List<int>();

            foreach (var entry in Spells)
            {
                var spell = entry.Value;
                spellList.Add(spell.SpellId);
            }

            return spellList;
        }

        public static int GetHighestDetectedSpellId()
        {
            var highest = 0;

            foreach (var spell in Spells)
            {
                highest = Math.Max(highest, spell.Key);
            }

            return highest;
        }

        public static float GetLowestEvadeTime(out Spell lowestSpell)
        {
            var lowest = float.MaxValue;
            lowestSpell = null;

            foreach (var entry in Spells)
            {
                var spell = entry.Value;

                if (spell.SpellHitTime != float.MinValue)
                {
                    //Console.WriteLine("spellhittime: " + spell.spellHitTime);
                    lowest = Math.Min(lowest, spell.SpellHitTime - spell.EvadeTime);
                    lowestSpell = spell;
                }
            }

            return lowest;
        }

        public static Spell GetMostDangerousSpell(bool hasProjectile = false)
        {
            var maxDanger = 0;
            Spell maxDangerSpell = null;

            foreach (var spell in Spells.Values)
            {
                if (!hasProjectile || spell.Info.ProjectileSpeed > 0 && spell.Info.ProjectileSpeed != float.MaxValue)
                {
                    var dangerlevel = spell.Dangerlevel;

                    if (dangerlevel > maxDanger)
                    {
                        maxDanger = dangerlevel;
                        maxDangerSpell = spell;
                    }
                }
            }

            return maxDangerSpell;
        }

        public static void InitChannelSpells()
        {
            ChanneledSpells["drain"] = "FiddleSticks";
            ChanneledSpells["crowstorm"] = "FiddleSticks";
            ChanneledSpells["katarinar"] = "Katarina";
            ChanneledSpells["absolutezero"] = "Nunu";
            ChanneledSpells["galioidolofdurand"] = "Galio";
            ChanneledSpells["missfortunebullettime"] = "MissFortune";
            ChanneledSpells["meditate"] = "MasterYi";
            ChanneledSpells["malzaharr"] = "Malzahar";
            ChanneledSpells["reapthewhirlwind"] = "Janna";
            ChanneledSpells["karthusfallenone"] = "Karthus";
            ChanneledSpells["karthusfallenone2"] = "Karthus";
            ChanneledSpells["velkozr"] = "Velkoz";
            ChanneledSpells["xerathlocusofpower2"] = "Xerath";
            ChanneledSpells["zace"] = "Zac";
            ChanneledSpells["pantheon_heartseeker"] = "Pantheon";
            ChanneledSpells["jhinr"] = "Jhin";
            ChanneledSpells["odinrecall"] = "AllChampions";
            ChanneledSpells["recall"] = "AllChampions";
        }

        public static void LoadDummySpell(SpellData spell)
        {
            var menuName = spell.CharName + " (" + spell.SpellKey + ") Settings";

            var enableSpell = !spell.DefaultOff;
            var isnewSpell = spell.Name.Contains("[Beta]");

            var newSpellMenu = new Menu(spell.CharName + spell.SpellName + "Settings", menuName);

            //if (isnewSpell)
            //newSpellMenu.SetFontStyle(FontStyle.Regular, Color.SkyBlue);

            newSpellMenu.Add(new MenuBool(spell.SpellName + "DrawSpell", "Draw Spell"));

            var whichMenu = isnewSpell ? new MenuBool(spell.SpellName + "DodgeSpell", "Dodge Spell [Beta]", enableSpell) : new MenuBool(spell.SpellName + "DodgeSpell", "Dodge Spell", enableSpell);

            newSpellMenu.Add(whichMenu);
            newSpellMenu.Add(new MenuSlider(spell.SpellName + "SpellRadius", "Spell Radius", (int) spell.Radius, (int) spell.Radius - 100, (int) spell.Radius + 100));
            newSpellMenu.Add(new MenuBool(spell.SpellName + "FastEvade", "Force Fast Evade", spell.Dangerlevel == 4));

            newSpellMenu.Add(new MenuSlider(spell.SpellName + "DodgeIgnoreHP", "Dodge Only Below HP % <=", spell.Dangerlevel == 1 ? 90 : 100));

            newSpellMenu.Add(new MenuList(spell.SpellName + "DangerLevel", "Danger Level", new[] {"Low", "Normal", "High", "Extreme"}, spell.Dangerlevel - 1));

            Evade.SpellMenu.Add(newSpellMenu);

            //Evade.menu.Add(newSpellMenu);
            ObjectCache.MenuCache.AddMenuToCache(newSpellMenu);
        }

        //Credits to Kurisu
        public static object NewInstance(Type type)
        {
            var target = type.GetConstructor(Type.EmptyTypes);
            var dynamic = new DynamicMethod(string.Empty, type, new Type[0], target.DeclaringType);
            var il = dynamic.GetILGenerator();

            il.DeclareLocal(target.DeclaringType);
            il.Emit(OpCodes.Newobj, target);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            var method = (Func<object>) dynamic.CreateDelegate(typeof(Func<object>));
            return method();
        }

        private void LoadSpecialSpell(SpellData spell)
        {
            if (ChampionPlugins.ContainsKey(spell.CharName))
            {
                ChampionPlugins[spell.CharName].LoadSpecialSpell(spell);
            }

            ChampionPlugins["AllChampions"].LoadSpecialSpell(spell);
        }

        private void LoadSpecialSpellPlugins()
        {
            ChampionPlugins.Add("AllChampions", new AllChampions());

            foreach (var hero in Evade.DevModeOn ? GameObjects.Heroes : GameObjects.EnemyHeroes)
            {
                var championPlugin = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && t.Namespace == "zzzz.SpecialSpells" && t.Name == hero.ChampionName).ToList().FirstOrDefault();

                if (championPlugin != null)
                {
                    if (!ChampionPlugins.ContainsKey(hero.ChampionName))
                    {
                        ChampionPlugins.Add(hero.ChampionName, (ChampionPlugin) NewInstance(championPlugin));
                    }
                }
            }
        }

        private void LoadSpellDictionary()
        {
            LoadSpecialSpellPlugins();

            foreach (var hero in GameObjects.Heroes.Where(h => h.IsValid))
            {
                if (hero.IsMe || Evade.DevModeOn)
                {
                    foreach (var spell in SpellWindupDatabase.Spells.Where(s => s.CharName == hero.ChampionName))
                    {
                        if (!WindupSpells.ContainsKey(spell.SpellName))
                        {
                            WindupSpells.Add(spell.SpellName, spell);
                        }
                    }
                }

                if (hero.Team != MyHero.Team || Evade.DevModeOn)
                {
                    foreach (var spell in SpellDatabase.Spells.Where(s => s.CharName == hero.ChampionName || s.CharName == "AllChampions"))
                    {
                        if (spell.HasTrap && spell.ProjectileSpeed > 3000)
                        {
                            if (spell.CharName == "AllChampions")
                            {
                                var spellexists = hero.SpellBook.Spells.Where(s => s != null && s.Name == spell.SpellName).FirstOrDefault();
                                if (spellexists != null)
                                {
                                    var slot = spellexists.Slot;
                                    if (slot == SpellSlot.Unknown)
                                    {
                                        continue;
                                    }
                                }
                            }

                            if (!OnProcessSpells.ContainsKey(spell.SpellName.ToLower() + "trap"))
                            {
                                if (string.IsNullOrEmpty(spell.TrapBaseName))
                                {
                                    spell.TrapBaseName = spell.SpellName + "1";
                                }

                                if (string.IsNullOrEmpty(spell.TrapTroyName))
                                {
                                    spell.TrapTroyName = spell.SpellName + "2";
                                }

                                OnProcessTraps.Add(spell.TrapBaseName.ToLower(), spell);
                                OnProcessTraps.Add(spell.TrapTroyName.ToLower(), spell);
                                OnProcessSpells.Add(spell.SpellName.ToLower() + "trap", spell);

                                LoadSpecialSpell(spell);

                                var menuName = spell.CharName + " (" + spell.SpellKey + ") Settings";

                                var enableSpell = !spell.DefaultOff;
                                var trapSpellName = spell.SpellName + "_trap";

                                var newSpellMenu = new Menu(spell.CharName + trapSpellName + "Settings", menuName);
                                newSpellMenu.Add(new MenuBool(trapSpellName + "DrawSpell", "Draw Trap"));
                                newSpellMenu.Add(new MenuBool(trapSpellName + "DodgeSpell", "Dodge Trap [Beta]", enableSpell));
                                newSpellMenu.Add(new MenuSlider(trapSpellName + "SpellRadius", "Trap Radius", (int) spell.Radius, (int) spell.Radius - 100, (int) spell.Radius + 100));
                                newSpellMenu.Add(new MenuSlider(trapSpellName + "DodgeIgnoreHP", "Dodge Only Below HP % <=", Math.Max(0, spell.Dangerlevel - 1) == 1 ? 90 : 100));
                                newSpellMenu.Add(new MenuList(trapSpellName + "DangerLevel", "Danger Level", new[] {"Low", "Normal", "High"}, Math.Max(0, spell.Dangerlevel - 1)));

                                TrapMenu.Add(newSpellMenu);
                            }
                        }
                    }

                    foreach (var spell in SpellDatabase.Spells.Where(s => s.CharName == hero.ChampionName || s.CharName == "AllChampions"))
                    {
                        Console.WriteLine(spell.SpellName);
                        if (spell.HasTrap && spell.ProjectileSpeed < 3000 || !spell.HasTrap)
                        {
                            if (spell.SpellType != SpellType.Circular && spell.SpellType != SpellType.Line && spell.SpellType != SpellType.Arc && spell.SpellType != SpellType.Cone)
                            {
                                continue;
                            }

                            if (spell.CharName == "AllChampions")
                            {
                                var spellexists = hero.SpellBook.Spells.Where(s => s != null && s.Name == spell.SpellName).FirstOrDefault();
                                if (spellexists != null)
                                {
                                    var slot = spellexists.Slot;
                                    if (slot == SpellSlot.Unknown)
                                    {
                                        continue;
                                    }
                                }
                            }

                            if (!OnProcessSpells.ContainsKey(spell.SpellName.ToLower()))
                            {
                                if (string.IsNullOrEmpty(spell.MissileName))
                                {
                                    spell.MissileName = spell.SpellName;
                                }

                                OnProcessSpells.Add(spell.SpellName.ToLower(), spell);
                                OnMissileSpells.Add(spell.MissileName.ToLower(), spell);

                                if (spell.ExtraSpellNames != null)
                                {
                                    foreach (var spellName in spell.ExtraSpellNames)
                                    {
                                        OnProcessSpells.Add(spellName.ToLower(), spell);
                                    }
                                }

                                if (spell.ExtraMissileNames != null)
                                {
                                    foreach (var spellName in spell.ExtraMissileNames)
                                    {
                                        OnMissileSpells.Add(spellName.ToLower(), spell);
                                    }
                                }

                                LoadSpecialSpell(spell);

                                var menuName = spell.CharName + " (" + spell.SpellKey + ") Settings";

                                var enableSpell = !spell.DefaultOff;
                                var isnewSpell = spell.Name.Contains("[Beta]") || spell.SpellType == SpellType.Cone;

                                var newSpellMenu = new Menu(spell.CharName + spell.SpellName + "Settings", menuName);

                                //if (isnewSpell)
                                //    newSpellMenu.SetFontStyle(FontStyle.Regular, Color.SkyBlue);

                                newSpellMenu.Add(new MenuBool(spell.SpellName + "DrawSpell", "Draw Spell"));

                                var isBetaDodge = isnewSpell
                                                      ? new MenuBool(spell.SpellName + "DodgeSpell", "Dodge Spell [Beta]", enableSpell)
                                                      : new MenuBool(spell.SpellName + "DodgeSpell", "Dodge Spell", enableSpell);

                                newSpellMenu.Add(isBetaDodge);

                                newSpellMenu.Add(new MenuSlider(spell.SpellName + "SpellRadius", "Spell Radius", (int) spell.Radius, (int) spell.Radius - 100, (int) spell.Radius + 100));
                                newSpellMenu.Add(new MenuBool(spell.SpellName + "FastEvade", "Force Fast Evade", spell.Dangerlevel == 4));

                                newSpellMenu.Add(new MenuSlider(spell.SpellName + "DodgeIgnoreHP", "Dodge Only Below HP % <=", spell.Dangerlevel == 1 ? 90 : 100));

                                newSpellMenu.Add(new MenuList(spell.SpellName + "DangerLevel", "Danger Level", new[] {"Low", "Normal", "High", "Extreme"}, spell.Dangerlevel - 1));

                                Evade.SpellMenu.Add(newSpellMenu);
                            }
                        }
                    }
                }
            }
        }
    }
}