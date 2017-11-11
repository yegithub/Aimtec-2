namespace Adept_AIO.SDK.Delegates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Menu;
    using Aimtec.SDK.Menu.Components;

    public delegate void OnGapcloserEvent(Obj_AI_Hero target, GapcloserArgs args);

    public enum SpellType
    {
        Melee = 0,
        Dash = 1,
        SkillShot = 2,
        Targeted = 3
    }

    struct SpellData
    {
        public string ChampionName { get; set; }
        public string SpellName { get; set; }
        public SpellSlot Slot { get; set; }
        public SpellType SpellType { get; set; }
    }

    public class GapcloserArgs
    {
        internal Obj_AI_Hero Unit { get; set; }
        public SpellSlot Slot { get; set; }
        public string SpellName { get; set; }
        public SpellType Type { get; set; }
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }
        public int StartTick { get; set; }
        public int EndTick { get; set; }
        public int DurationTick { get; set; }
        public bool HaveShield { get; set; }
    }

    public static class Gapcloser
    {
        public static Dictionary<int, GapcloserArgs> Gapclosers = new Dictionary<int, GapcloserArgs>();
        internal static List<SpellData> Spells = new List<SpellData>();

        public static Menu Menu;

        static Gapcloser()
        {
            Initialize();
        }

        public static int MinSearchRange { get; set; } = 550;
        public static int MaxSearchRange { get; set; } = 700;
        public static int DefalutHPercent { get; set; } = 100;
        public static event OnGapcloserEvent OnGapcloser;

        private static void Initialize()
        {
            Spells.Add(new SpellData
            {
                ChampionName = "Aatrox",
                Slot = SpellSlot.Q,
                SpellName = "aatroxq",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ahri",
                Slot = SpellSlot.R,
                SpellName = "ahritumble",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Akali",
                Slot = SpellSlot.R,
                SpellName = "akalishadowdance",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Alistar",
                Slot = SpellSlot.W,
                SpellName = "headbutt",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Azir",
                Slot = SpellSlot.E,
                SpellName = "azire",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Caitlyn",
                Slot = SpellSlot.E,
                SpellName = "caitlynentrapment",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Camille",
                Slot = SpellSlot.E,
                SpellName = "camillee",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Camille",
                Slot = SpellSlot.E,
                SpellName = "camilleedash2",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Corki",
                Slot = SpellSlot.W,
                SpellName = "carpetbomb",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Diana",
                Slot = SpellSlot.R,
                SpellName = "dianateleport",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ekko",
                Slot = SpellSlot.E,
                SpellName = "ekkoeattack",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Elise",
                Slot = SpellSlot.Q,
                SpellName = "elisespiderqcast",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Elise",
                Slot = SpellSlot.E,
                SpellName = "elisespideredescent",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ezreal",
                Slot = SpellSlot.E,
                SpellName = "ezrealarcaneshift",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Fiora",
                Slot = SpellSlot.Q,
                SpellName = "fioraq",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Fizz",
                Slot = SpellSlot.Q,
                SpellName = "fizzpiercingstrike",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Galio",
                Slot = SpellSlot.E,
                SpellName = "galioe",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                Slot = SpellSlot.E,
                SpellName = "gnarbige",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gnar",
                Slot = SpellSlot.E,
                SpellName = "gnare",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Gragas",
                Slot = SpellSlot.E,
                SpellName = "gragase",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Graves",
                Slot = SpellSlot.E,
                SpellName = "gravesmove",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Hecarim",
                Slot = SpellSlot.R,
                SpellName = "hecarimult",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Illaoi",
                Slot = SpellSlot.W,
                SpellName = "illaoiwattack",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Irelia",
                Slot = SpellSlot.Q,
                SpellName = "ireliagatotsu",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "JarvanIV",
                Slot = SpellSlot.Q,
                SpellName = "jarvanivdragonstrike",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Jax",
                Slot = SpellSlot.Q,
                SpellName = "jaxleapstrike",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Jayce",
                Slot = SpellSlot.Q,
                SpellName = "jaycetotheskies",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Kassadin",
                Slot = SpellSlot.R,
                SpellName = "riftwalk",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Katarina",
                Slot = SpellSlot.E,
                SpellName = "katarinae",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Kayn",
                Slot = SpellSlot.Q,
                SpellName = "kaynq",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Khazix",
                Slot = SpellSlot.E,
                SpellName = "khazixe",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Khazix",
                Slot = SpellSlot.E,
                SpellName = "khazixelong",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Kindred",
                Slot = SpellSlot.Q,
                SpellName = "kindredq",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Leblanc",
                Slot = SpellSlot.W,
                SpellName = "leblancslide",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Leblanc",
                Slot = SpellSlot.W,
                SpellName = "leblancslidem",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "LeeSin",
                Slot = SpellSlot.Q,
                SpellName = "blindmonkqtwo",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Leona",
                Slot = SpellSlot.E,
                SpellName = "leonazenithblade",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Lucian",
                Slot = SpellSlot.E,
                SpellName = "luciane",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Malphite",
                Slot = SpellSlot.R,
                SpellName = "ufslash",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "MasterYi",
                Slot = SpellSlot.Q,
                SpellName = "alphastrike",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "MonkeyKing",
                Slot = SpellSlot.E,
                SpellName = "monkeykingnimbus",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Nautilus",
                Slot = SpellSlot.Q,
                SpellName = "nautilusq",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Nidalee",
                Slot = SpellSlot.W,
                SpellName = "pounce",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Pantheon",
                Slot = SpellSlot.W,
                SpellName = "pantheon_leapbash",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Poppy",
                Slot = SpellSlot.E,
                SpellName = "poppyheroiccharge",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Quinn",
                Slot = SpellSlot.E,
                SpellName = "quinne",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Rakan",
                Slot = SpellSlot.W,
                SpellName = "rakanw",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "RekSai",
                Slot = SpellSlot.E,
                SpellName = "reksaieburrowed",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Renekton",
                Slot = SpellSlot.E,
                SpellName = "renektonsliceanddice",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Renekton",
                Slot = SpellSlot.E,
                SpellName = "renektonpreexecute",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Renekton",
                Slot = SpellSlot.E,
                SpellName = "renektonsuperexecute",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Rengar",
                Slot = SpellSlot.Unknown,
                SpellName = "rengarpassivebuffdash",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Rengar",
                Slot = SpellSlot.Unknown,
                SpellName = "rengarpassivebuffdashaadummy",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Riven",
                Slot = SpellSlot.Q,
                SpellName = "riventricleave",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Riven",
                Slot = SpellSlot.E,
                SpellName = "rivenfeint",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Sejuani",
                Slot = SpellSlot.Q,
                SpellName = "sejuaniarcticassault",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Shen",
                Slot = SpellSlot.E,
                SpellName = "shene",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Shyvana",
                Slot = SpellSlot.R,
                SpellName = "shyvanatransformcast",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Talon",
                Slot = SpellSlot.Q,
                SpellName = "talonq",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Talon",
                Slot = SpellSlot.E,
                SpellName = "taloncutthroat",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Tristana",
                Slot = SpellSlot.W,
                SpellName = "rocketjump",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Tryndamere",
                Slot = SpellSlot.E,
                SpellName = "slashcast",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Vi",
                Slot = SpellSlot.Q,
                SpellName = "viq",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Vayne",
                Slot = SpellSlot.Q,
                SpellName = "vaynetumble",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Warwick",
                Slot = SpellSlot.R,
                SpellName = "warwickr",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "XinZhao",
                Slot = SpellSlot.E,
                SpellName = "xenzhaosweep",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Yasuo",
                Slot = SpellSlot.E,
                SpellName = "yasuodashwrapper",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Zac",
                Slot = SpellSlot.E,
                SpellName = "zace",
                SpellType = SpellType.SkillShot
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Zed",
                Slot = SpellSlot.R,
                SpellName = "zedr",
                SpellType = SpellType.Targeted
            });

            Spells.Add(new SpellData
            {
                ChampionName = "Ziggs",
                Slot = SpellSlot.W,
                SpellName = "ziggswtoggle",
                SpellType = SpellType.SkillShot
            });
        }

        public static void Attach(Menu mainMenu, string menuName)
        {
            if (ObjectManager.Get<Obj_AI_Hero>().All(x => !x.IsEnemy))
            {
                return;
            }

            Menu = new Menu("", menuName)
            {
                new MenuBool("Enabled", "Enabled"),
                new MenuSeperator("Seperator1")
            };
            mainMenu.Add(Menu);

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy))
            {
                var heroMenu = new Menu(enemy.ChampionName, enemy.ChampionName)
                {
                    new MenuBool(enemy.ChampionName + ".Enabled", "Enabled"),
                    new MenuSlider(enemy.ChampionName + ".Distance", "If Target Distance To Player <= x", MinSearchRange, 1, MaxSearchRange),
                    new MenuSlider(enemy.ChampionName + ".HPercent", "When Player HealthPercent <= x%", DefalutHPercent, 1, 101)
                };
                Menu.Add(heroMenu);

                if (enemy.IsMelee)
                {
                    heroMenu.Add(new MenuSliderBool(enemy.ChampionName + ".Melee", "Anti Melee Attack| Player HP <= x%", true, DefalutHPercent / 2, 1, 99));
                }

                foreach (var spell in Spells.Where(x => x.ChampionName == enemy.ChampionName))
                {
                    heroMenu.Add(new MenuBool(enemy.ChampionName + "." + spell.SpellName.ToLower(), "Spell: " + spell.Slot + " (" + spell.SpellName + ")"));
                }
            }

            Game.OnUpdate += OnUpdate;
            //GameObject.OnCreate += OnCreate;
            Obj_AI_Base.OnProcessAutoAttack += OnProcessAutoAttack;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Obj_AI_Base.OnNewPath += OnNewPath;
        }

        private static void OnCreate(GameObject sender)
        {
            //special dash (like rengar, khazix, ziggs)
        }

        private static void OnProcessAutoAttack(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null || sender.Type != GameObjectType.obj_AI_Hero || !sender.IsEnemy || !sender.IsMelee)
            {
                return;
            }

            if (string.IsNullOrEmpty(args.SpellData.Name) || args.Target == null || !args.Target.IsMe ||
                Menu[sender.UnitSkinName.ToLower()][sender.UnitSkinName.ToLower() + ".Melee"] == null ||
                !Menu[sender.UnitSkinName.ToLower()][sender.UnitSkinName.ToLower() + ".Melee"].Enabled)
            {
                return;
            }

            if (!Gapclosers.ContainsKey(sender.NetworkId))
            {
                Gapclosers.Add(sender.NetworkId, new GapcloserArgs());
            }

            Gapclosers[sender.NetworkId].Unit = (Obj_AI_Hero) sender;
            Gapclosers[sender.NetworkId].Slot = SpellSlot.Unknown;
            Gapclosers[sender.NetworkId].Type = SpellType.Melee;
            Gapclosers[sender.NetworkId].SpellName = args.SpellData.Name;
            Gapclosers[sender.NetworkId].StartPosition = args.Start;
            Gapclosers[sender.NetworkId].EndPosition = args.End;
            Gapclosers[sender.NetworkId].StartTick = Environment.TickCount;
        }

        private static void OnNewPath(Obj_AI_Base sender, Obj_AI_BaseNewPathEventArgs args)
        {
            if (sender == null || sender.Type != GameObjectType.obj_AI_Hero || !sender.IsEnemy)
            {
                return;
            }

            if (sender.UnitSkinName == "Vi" || sender.UnitSkinName == "Sion" || sender.UnitSkinName == "Kayn" || sender.UnitSkinName == "Fizz")
            {
                // Vi R
                // Sion R
                // Kayn R
                // Fizz E
                return;
            }

            if (!Gapclosers.ContainsKey(sender.NetworkId))
            {
                Gapclosers.Add(sender.NetworkId, new GapcloserArgs());
            }

            if (args.IsDash)
            {
                Gapclosers[sender.NetworkId].Unit = (Obj_AI_Hero) sender;
                Gapclosers[sender.NetworkId].Slot = SpellSlot.Unknown;
                Gapclosers[sender.NetworkId].Type = SpellType.Dash;
                Gapclosers[sender.NetworkId].SpellName = sender.UnitSkinName + "_Dash";
                Gapclosers[sender.NetworkId].StartPosition = sender.ServerPosition;
                Gapclosers[sender.NetworkId].EndPosition = args.Path.Last();
                Gapclosers[sender.NetworkId].StartTick = Environment.TickCount;
                Gapclosers[sender.NetworkId].EndTick =
                    (int) (Gapclosers[sender.NetworkId].EndPosition.DistanceSqr(Gapclosers[sender.NetworkId].StartPosition) / args.Speed * args.Speed * 1000) +
                    Gapclosers[sender.NetworkId].StartTick;
                Gapclosers[sender.NetworkId].DurationTick = Gapclosers[sender.NetworkId].EndTick - Gapclosers[sender.NetworkId].StartTick;
                Gapclosers[sender.NetworkId].HaveShield = HaveShiledBuff(sender);
            }
        }

        private static void OnUpdate()
        {
            if (Gapclosers.Values.Any(x => Environment.TickCount - x.StartTick > 900 + Game.Ping))
            {
                Gapclosers.Clear();
            }

            if (OnGapcloser == null || Menu["Enabled"] == null || !Menu["Enabled"].Enabled)
            {
                return;
            }

            foreach (var args in Gapclosers.Where(x => x.Value.Unit.IsValidTarget() && Menu[x.Value.Unit.ChampionName] != null &&
                                                       Menu[x.Value.Unit.ChampionName][x.Value.Unit.ChampionName + ".Enabled"].Enabled))
            {
                if (args.Value.Type == SpellType.SkillShot)
                {
                    if (args.Value.Unit.ServerPosition.DistanceSqr(ObjectManager.GetLocalPlayer().ServerPosition) <=
                        Menu[args.Value.Unit.ChampionName][args.Value.Unit.ChampionName + ".Distance"].Value *
                        Menu[args.Value.Unit.ChampionName][args.Value.Unit.ChampionName + ".Distance"].Value)
                    {
                        if (ObjectManager.GetLocalPlayer().HealthPercent() <= Menu[args.Value.Unit.ChampionName][args.Value.Unit.ChampionName + ".HPercent"].Value)
                        {
                            OnGapcloser(args.Value.Unit, args.Value);
                        }
                    }
                }
                else if (args.Value.Type == SpellType.Dash)
                {
                    if (args.Value.Type == SpellType.Dash && args.Value.EndPosition.DistanceSqr(ObjectManager.GetLocalPlayer().ServerPosition) <=
                        Menu[args.Value.Unit.ChampionName][args.Value.Unit.ChampionName + ".Distance"].Value *
                        Menu[args.Value.Unit.ChampionName][args.Value.Unit.ChampionName + ".Distance"].Value)
                    {
                        if (ObjectManager.GetLocalPlayer().HealthPercent() <= Menu[args.Value.Unit.ChampionName][args.Value.Unit.ChampionName + ".HPercent"].Value)
                        {
                            OnGapcloser(args.Value.Unit, args.Value);
                        }
                    }
                }
                else if (args.Value.Type == SpellType.Targeted)
                {
                    if (ObjectManager.GetLocalPlayer().HealthPercent() <= Menu[args.Value.Unit.ChampionName][args.Value.Unit.ChampionName + ".HPercent"].Value)
                    {
                        OnGapcloser(args.Value.Unit, args.Value);
                    }
                }
                else if (args.Value.Type == SpellType.Melee)
                {
                    if (ObjectManager.GetLocalPlayer().HealthPercent() <= Menu[args.Value.Unit.ChampionName][args.Value.Unit.ChampionName + ".Melee"].As<MenuSliderBool>().Value)
                    {
                        OnGapcloser(args.Value.Unit, args.Value);
                    }
                }
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null || args.Sender == null || args.SpellData == null || !sender.IsValid || sender.Type != GameObjectType.obj_AI_Hero || !sender.IsEnemy ||
                string.IsNullOrEmpty(args.SpellData.Name) || args.SpellData.Name.ToLower().Contains("attack") || args.SpellData.Name.ToLower().Contains("crit"))
            {
                return;
            }

            if (Spells.All(x => !string.Equals(x.SpellName, args.SpellData.Name, StringComparison.CurrentCultureIgnoreCase)) ||
                !Menu[sender.UnitSkinName.ToLower()][sender.UnitSkinName.ToLower() + "." + args.SpellData.Name.ToLower()].Enabled)
            {
                return;
            }

            if (!Gapclosers.ContainsKey(sender.NetworkId))
            {
                Gapclosers.Add(sender.NetworkId, new GapcloserArgs());
            }

            Gapclosers[sender.NetworkId].Unit = (Obj_AI_Hero) sender;
            Gapclosers[sender.NetworkId].Slot = args.SpellSlot;
            Gapclosers[sender.NetworkId].Type = args.Target != null ? SpellType.Targeted : SpellType.SkillShot;
            Gapclosers[sender.NetworkId].SpellName = args.SpellData.Name;
            Gapclosers[sender.NetworkId].StartPosition = args.Start;
            Gapclosers[sender.NetworkId].EndPosition = args.End;
            Gapclosers[sender.NetworkId].StartTick = Environment.TickCount;
            Gapclosers[sender.NetworkId].HaveShield = HaveShiledBuff(sender);
        }

        private static bool HaveShiledBuff(Obj_AI_Base target)
        {
            if (target == null || target.IsDead || !target.IsValidTarget())
            {
                return false;
            }

            if (target.HasBuff("BlackShield"))
            {
                return true;
            }

            if (target.HasBuff("bansheesveil"))
            {
                return true;
            }

            if (target.HasBuff("SivirE"))
            {
                return true;
            }

            if (target.HasBuff("NocturneShroudofDarkness"))
            {
                return true;
            }

            return target.HasBuff("itemmagekillerveil") || target.HasBuffOfType(BuffType.SpellShield);
        }
    }
}