namespace Adept_BaseUlt.Local_SDK
{
    using System;
    using System.Collections.Generic;
    using Aimtec;

    public static class Teleport
    {
        public delegate void TeleportHandler(Obj_AI_Base sender, TeleportEventArgs args);

        internal static readonly Dictionary<int, TeleportEventArgs> TeleportDataNetId = new Dictionary<int, TeleportEventArgs>();

        static Teleport()
        {
            Obj_AI_Base.OnTeleport += OnUnitTeleport;
        }

        public static event TeleportHandler OnTeleport;

        private static void OnUnitTeleport(Obj_AI_Base sender, Obj_AI_BaseTeleportEventArgs e)
        {
            var eventArgs = new TeleportEventArgs
            {
                Status = TeleportStatus.Unknown,
                Type = TeleportType.Unknown
            };

            if (sender == null)
            {
                return;
            }

            if (!TeleportDataNetId.ContainsKey(sender.NetworkId))
            {
                TeleportDataNetId[sender.NetworkId] = eventArgs;
            }

            if (!string.IsNullOrEmpty(e.DisplayName))
            {
                eventArgs.Status = TeleportStatus.Start;
                eventArgs.Duration = GetDuration(e);
                eventArgs.Type = GetType(e);
                eventArgs.Start = Game.TickCount;

                TeleportDataNetId[sender.NetworkId] = eventArgs;
            }
            else
            {
                eventArgs = TeleportDataNetId[sender.NetworkId];
                eventArgs.Status = Game.TickCount - eventArgs.Start < eventArgs.Duration - 250 ? TeleportStatus.Abort : TeleportStatus.Finish;
            }

            OnTeleport?.Invoke(sender, eventArgs);
        }

        internal static TeleportType GetType(Obj_AI_BaseTeleportEventArgs args)
        {
            switch (args.DisplayName)
            {
                case "Recall": return TeleportType.Recall;
                case "Teleport": return TeleportType.Teleport;
                case "Gate": return TeleportType.TwistedFate;
                case "Shen": return TeleportType.Shen;
                default: return TeleportType.Recall;
            }
        }

        internal static int GetDuration(Obj_AI_BaseTeleportEventArgs args)
        {
            switch (GetType(args))
            {
                case TeleportType.Shen: return 3000;
                case TeleportType.Teleport: return 4500;
                case TeleportType.TwistedFate: return 1500;
                case TeleportType.Recall: return GetRecallDuration(args);
                default: return 3500;
            }
        }

        internal static int GetRecallDuration(Obj_AI_BaseTeleportEventArgs args)
        {
            switch (args.DisplayName.ToLower())
            {
                case "recall": return 8000;
                case "recallimproved": return 7000;
                case "odinrecall": return 4500;
                case "odinrecallimproved": return 4000;
                case "superrecall": return 4000;
                case "superrecallimproved": return 4000;
                default: return 8000;
            }
        }

        public class TeleportEventArgs : EventArgs
        {
            public int Start { get; internal set; }
            public int Duration { get; internal set; }
            public TeleportType Type { get; internal set; }
            public TeleportStatus Status { get; internal set; }
        }
    }

    public enum TeleportType
    {
        Recall,
        Teleport,
        TwistedFate,
        Shen,
        Unknown
    }

    public enum TeleportStatus
    {
        Start,
        Abort,
        Finish,
        Unknown
    }
}