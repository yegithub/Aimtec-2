using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Util;
using Aimtec.SDK.Util.Cache;

namespace Adept_Tracker
{
    class SpellTracker
    {
        #region Constants

        /// <summary>
        ///     The box height
        /// </summary>
        private const int BoxHeight = 105;

        /// <summary>
        ///     The box spacing
        /// </summary>
        private const int BoxSpacing = 25;

        /// <summary>
        ///     The box width
        /// </summary>
        private const int BoxWidth = 235;

        /// <summary>
        ///     The color indicator width
        /// </summary>
        private const int ColorIndicatorWidth = 10;

        /// <summary>
        ///     The countdown
        /// </summary>
        private const int Countdown = 10;

        /// <summary>
        ///     The move right speed
        /// </summary>
        private const int MoveRightSpeed = 1500;

        #endregion

        #region Properties

        private List<Card> Cards { get; } = new List<Card>();
    
        private Dictionary<string, List<SpellSlot>> ChampionSpells { get; } = new Dictionary<string, List<SpellSlot>>();
        private Dictionary<string, List<Texture>> Textures { get; } = new Dictionary<string, List<Texture>>();

        private Vector2 Padding { get; } = new Vector2(10, 5);

        private Menu Menu { get; set; }

        /// <summary>
        ///     Gets or sets the start x.
        /// </summary>
        /// <value>
        ///     The start x.
        /// </value>
        private int StartX => this.Menu["XPos"].Value;

        /// <summary>
        ///     Gets or sets the start y.
        /// </summary>
        /// <value>
        ///     The start y.
        /// </value>
        private int StartY => this.Menu["YPos"].Value;

        #endregion

        #region Public Methods and Operators

        public SpellTracker()
        {
            if (Game.MapId == GameMapId.HowlingAbyss)
            {
                return;
            }

            GetSpells();
            CreateMenu();

            Game.OnUpdate += OnUpdate;
            Render.OnPresent += OnPresent;
            Teleport.OnTeleport += OnTeleport;
            BuffManager.OnRemoveBuff += OnRemoveBuff;
            JungleTracker.CampDied += OnCampDied;
        }

        public void GetSpells()
        {
            if (GameObjectsBig.EnemyHeroes.FirstOrDefault(x => x.ChampionName == "Nami") == null) // Just for test card. Bug: Doesn't show image.
            {
                if (!this.ChampionSpells.ContainsKey("Nami"))
                {
                    this.ChampionSpells["Nami"] = new List<SpellSlot>();
                }
                this.ChampionSpells["Nami"].Add(SpellSlot.Q);

                var bitmap = Utility.GetBitMap("NamiQ");
                if (bitmap != null)
                {
                    var tex = new Texture(Utility.ResizeImage(bitmap, new Size(64, 64)));

                    if (!this.Textures.ContainsKey("NamiQ"))
                    {
                        this.Textures["NamiQ"] = new List<Texture>();
                    }

                    Textures["NamiQ"].Add(tex);
                }
            }

            foreach (var unit in GameObjects.EnemyHeroes)
            {
                foreach (var spell in unit.SpellBook.Spells)
                {
                    if (spell.Slot != SpellSlot.R)
                    {
                        continue;
                    }

                    if (!this.ChampionSpells.ContainsKey(unit.ChampionName))
                    {
                        this.ChampionSpells[unit.ChampionName] = new List<SpellSlot>();
                    }
                    this.ChampionSpells[unit.ChampionName].Add(spell.Slot);

                    var bitmap = Utility.GetBitMap(spell.Name);
                    if (bitmap == null)
                    {
                        continue;
                    }

                    var tex = new Texture(Utility.ResizeImage(bitmap, new Size(64, 64)));

                    if (!this.Textures.ContainsKey(spell.Name))
                    {
                        this.Textures[spell.Name] = new List<Texture>();
                    }

                    Textures[spell.Name].Add(tex);
                }
            }
        }

        private void OnRemoveBuff(Obj_AI_Base sender, Buff buff)
        {
            if (!sender.IsEnemy)
            {
                return;
            }

            if (buff.Name.Equals("rebirthready", StringComparison.CurrentCultureIgnoreCase))
            {
                var card = new Card
                {
                    EndTime = Game.ClockTime + 240,
                    EndMessage = "Ready",
                    FriendlyName = "Rebirth",
                    StartTime = Game.ClockTime,
                    Name = "Rebirthready"
                };
                this.Cards.Add(card);
            }

            if (buff.Name.Equals("zacrebirthready", StringComparison.InvariantCultureIgnoreCase))
            {
                var card = new Card
                {
                    EndTime = Game.ClockTime + 300,
                    EndMessage = "Ready",
                    FriendlyName = "Cell Division",
                    StartTime = Game.ClockTime,
                    Name = "Zacrebirthready"
                };

                this.Cards.Add(card);
            }
        }

        private void OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            try
            {
                if (sender.IsAlly || !Menu["DrawTeleport"].Enabled)
                {
                    return;
                }

                var hero = sender as Obj_AI_Hero;

                if (hero == null)
                {
                    return;
                }

                if (args.Type == TeleportType.Teleport &&
                    (args.Status == TeleportStatus.Abort || args.Status == TeleportStatus.Finish))
                {
                    var time = Game.ClockTime;
                    DelayAction.Queue(250, () =>
                    {
                        var cd = args.Status == TeleportStatus.Finish ? 300 : 200;
                        var card = new Card
                        {
                            EndTime = time + cd,
                            EndMessage = "Ready",
                            FriendlyName = $"{hero.ChampionName} Teleport",
                            StartTime = Game.ClockTime,
                            Name = "Teleport"
                        };
                        this.Cards.Add(card);

                    }, new CancellationToken(false));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"An error occurred: '{0}'", e);
                throw;
            }
        }

        private void OnPresent()
        {
            if (!Menu["DrawCards"].Enabled)
            {
                return;
            }

            var i = 0;

            foreach (var enemy in GameObjects.EnemyHeroes)
            {
                if (!this.ChampionSpells.TryGetValue(enemy.ChampionName, out var slots) || !this.Menu[$"Track.{enemy.ChampionName}"].Enabled)
                {
                    continue;
                }

                foreach (var spell in slots.Select(x => enemy.GetSpell(x)).Where(x =>
                    x.Level > 0 && x.CooldownEnd > 0 && x.CooldownEnd - Game.ClockTime <= Countdown))
                {
                    if (spell.CooldownEnd - Game.ClockTime <= -3 && this.StartX + (int)((-(spell.CooldownEnd - Game.ClockTime) - 3) * MoveRightSpeed) >= Render.Width + i * MoveRightSpeed)
                    {
                        continue;
                    }

                    var remainingTime = spell.CooldownEnd - Game.ClockTime;
                    var spellReady = remainingTime <= 0;

                    var remainingTimePretty = remainingTime > 0 ? remainingTime.ToString("N1") : "Ready";

                    var indicatorColor = spellReady ? Color.LawnGreen : Color.Yellow;

                    // We only need to calculate the y axis since the boxes stack vertically
                    var boxY = this.StartY - i * BoxSpacing - i * BoxHeight;
                    var boxX = this.StartX;

                    if (remainingTime <= -3)
                    {
                        boxX += (int)((-remainingTime - 3) * MoveRightSpeed);
                    }

                    var lineStart = new Vector2(boxX, boxY);

                    Utility.DrawBox(lineStart, ColorIndicatorWidth, BoxHeight, indicatorColor, 0, new Color());

                    // Draw the black rectangle
                    var boxStart = new Vector2(boxX + ColorIndicatorWidth, boxY);
                    Utility.DrawBox(boxStart, BoxWidth - ColorIndicatorWidth, BoxHeight, Color.Black, 0, new Color());

                    // Draw spell name
                    var spellNameStart = boxStart + this.Padding;

                    var textSize = MiscUtils.MeasureText($"{enemy.ChampionName} {spell.Slot}");
                    var iconStart = spellNameStart + new Vector2(0, textSize[1] - 50);

                    Render.Text($"{enemy.ChampionName} {spell.Slot}", new Vector2(iconStart.X, iconStart.Y - 25), RenderTextFlags.Center, Color.White);

                    foreach (var texture in Textures[spell.Name])
                    {
                        texture.Draw(new Vector2(iconStart.X, iconStart.Y));
                    }

                    // draw countdown, add [icon size + padding]
                    var countdownStart = iconStart + new Vector2(51 + 22, 5);
                    Render.Text(remainingTimePretty, new Vector2((int)countdownStart.X, (int)countdownStart.Y - 5), RenderTextFlags.Center, Color.White);

                    // Draw progress bar :(
                    var countdownSize = MiscUtils.MeasureText(remainingTimePretty);
                    var progressBarStart = countdownStart + new Vector2(0, countdownSize[1] + 9);
                    const int progressBarFullSize = 125;
                    var progressBarActualSize = (Countdown - remainingTime) / Countdown * progressBarFullSize;

                    if (progressBarActualSize > progressBarFullSize) // broken
                    {
                        progressBarActualSize = progressBarFullSize;
                    }

                    // MAGICERINO
                    Utility.DrawBox(progressBarStart, progressBarFullSize, 15, Color.Black, 1, Color.LawnGreen);
                    Utility.DrawBox(
                        progressBarStart + new Vector2(3, 8),
                        (int)progressBarActualSize,
                        15 - 5,
                        Color.LawnGreen,
                        0,
                        new Color());

                    i++;
                }
            }

            foreach (var card in this.Cards.Where(x => x.EndTime - Game.ClockTime <= Countdown))
            {
                // draw spell
                var remainingTime = card.EndTime - Game.ClockTime;
                var spellReady = remainingTime <= 0;

                var remainingTimePretty = remainingTime > 0 ? remainingTime.ToString("N1") : card.EndMessage;

                var indicatorColor = spellReady ? Color.LawnGreen : Color.Yellow;

                // We only need to calculate the y axis since the boxes stack vertically
                var boxY = this.StartY - i * BoxSpacing - i * BoxHeight;
                var boxX = this.StartX;

                if (remainingTime <= -3)
                {
                    boxX += (int)((-remainingTime - 3) * MoveRightSpeed);
                }

                var lineStart = new Vector2(boxX, boxY);

                Utility.DrawBox(lineStart, ColorIndicatorWidth, BoxHeight, indicatorColor, 0, new Color());

                // Draw the black rectangle
                var boxStart = new Vector2(boxX + ColorIndicatorWidth, boxY);
                Utility.DrawBox(boxStart, BoxWidth - ColorIndicatorWidth, BoxHeight, Color.Black, 0, new Color());

                // Draw spell name
                var spellNameStart = boxStart + this.Padding;
               
                // draw icon
                var textSize = MiscUtils.MeasureText(card.FriendlyName);
                var iconStart = spellNameStart + new Vector2(0, textSize[1] - 50);
                Render.Text(card.FriendlyName, new Vector2(iconStart.X, iconStart.Y - 25), RenderTextFlags.Center, Color.White);

                foreach (var texture in Textures[card.Name])
                {
                    texture.Draw(new Vector3(-1 * iconStart, 0));
                }

                // draw countdown, add [icon size + padding]
                var countdownStart = iconStart + new Vector2(51 + 22, 5);
                Render.Text(remainingTimePretty, new Vector2((int)countdownStart.X, (int)countdownStart.Y - 5) ,RenderTextFlags.Center, Color.White);

                // Draw progress bar :(
                var countdownSize = MiscUtils.MeasureText(remainingTimePretty);
                var progressBarStart = countdownStart + new Vector2(0, countdownSize[1]);
                const int progressBarFullSize = 125;
                var cooldown = card.EndTime - card.StartTime;
                var progressBarActualSize = (cooldown - remainingTime) / cooldown * progressBarFullSize;

                if (progressBarActualSize > progressBarFullSize)
                {
                    progressBarActualSize = progressBarFullSize;
                }

                // MAGICERINO
                Utility.DrawBox(progressBarStart, progressBarFullSize, 15, Color.Black, 1, Color.LawnGreen);
                Utility.DrawBox(
                    progressBarStart + new Vector2(3, 8),
                    (int)progressBarActualSize,
                    15 - 5,
                    Color.LawnGreen,
                    0,
                    new Color());

                i++;
            }
        }

        private void OnUpdate()
        {
            this.Cards.RemoveAll(x =>
                x.EndTime - Game.ClockTime <= -3 &&
                this.StartX + (int) ((-(x.EndTime - Game.ClockTime) - 3) * MoveRightSpeed) >=
                Render.Width + this.Cards.Count * MoveRightSpeed);
        }

        private void OnCampDied(object o, JungleTracker.JungleCamp e)
        {
            if (!Menu["DrawJungle"].Enabled || !e.MobNames.Any(x => x.ToLower().Contains("baron") || x.ToLower().Contains("dragon")))
            {
                return;
            }

            var card = new Card
            {
                EndTime = e.NextRespawnTime,
                EndMessage = "Respawn",
                FriendlyName = e.MobNames.Any(x => x.ToLower().Contains("dragon")) ? "Dragon" : "Baron",
                StartTime = Game.ClockTime
            };

            card.Name = card.FriendlyName;
            this.Cards.Add(card);
        }

        private void CreateMenu()
        {
            Menu = new Menu("AdeptTracker", "Adept Tracker", true);
            Menu.Attach();

            Menu.Add(new MenuSlider("XPos", "X Position", Render.Width - BoxWidth, 0, Render.Width));
            Menu.Add(new MenuSlider("YPos", "Y Position", Render.Height - BoxHeight * 4, 0, Render.Height));

            Menu.Add(new MenuBool("DrawCards", "Draw Cards"));
            //Menu.Add(new MenuBool("DrawJungle", "Draw Jungle"));
            Menu.Add(new MenuBool("DrawTeleport", "Draw Teleports"));
            Menu.Add(new MenuBool("AddTestCard", "Draw Test Card", false));

            Menu.Add(new MenuSeperator("endmylife", "Whitelist"));
            foreach (var enemy in GameObjects.EnemyHeroes)
            {
                Menu.Add(new MenuBool($"Track.{enemy.UnitSkinName}", "Track " + enemy.ChampionName));
            }

            Menu["AddTestCard"].OnValueChanged += (sender, args) =>
            {
                if (sender.Enabled == false)
                {
                    return;
                }

                Cards.Add(new Card
                {
                    EndMessage = "Ready",
                    EndTime = Game.ClockTime + 11,
                    FriendlyName = $"Sample Text",
                    StartTime = Game.ClockTime,
                    Name = "NamiQ"
                });
            };
        }

        #endregion
    }

    class Card
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the end message.
        /// </summary>
        /// <value>
        ///     The end message.
        /// </value>
        public string EndMessage { get; set; }

        /// <summary>
        ///     Gets or sets the end time.
        /// </summary>
        /// <value>
        ///     The end time.
        /// </value>
        public float EndTime { get; set; }

        /// <summary>
        ///     Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        ///     The name of the friendly.
        /// </value>
        public string FriendlyName { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the start time.
        /// </summary>
        /// <value>
        ///     The start time.
        /// </value>
        public float StartTime { get; set; }

        #endregion
    }
}
