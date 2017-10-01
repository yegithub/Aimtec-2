using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aimtec;

namespace Adept_Tracker
{
    internal static class Utility
    {
        static Utility()
        {
            if (!Directory.Exists(ResourcePath))
            {
                Directory.CreateDirectory(ResourcePath);
            }
        }

        internal static string ResourcePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Adept Tracker");

        public static Dictionary<string, Bitmap> BitMapCache = new Dictionary<string, Bitmap>();

        public static string GetPathToResource(string name)
        {
            return Path.Combine(ResourcePath, $"{name}.bmp");
        }

        public static Bitmap LoadFromFile(string name)
        {
            var path = GetPathToResource(name);

            return File.Exists(path) ? new Bitmap(path) : null;
        }

        public static Bitmap GetCached(string name)
        {
            if (BitMapCache.ContainsKey(name))
            {
                return BitMapCache[name];
            }

            var bmp = LoadFromFile(name);
            if (bmp != null)
            {
                BitMapCache[name] = bmp;
                return bmp;
            }

            return null;
        }

        public static Bitmap GetBitMap(string name)
        {
            var cached = GetCached(name);

            if (cached != null)
            {
                return cached;
            }

            var bmp = DownloadBitMap(name);
            BitMapCache[name] = bmp;
            bmp.Save(GetPathToResource(name));

            return bmp;
        }

        public static Bitmap DownloadBitMap(string name)
        {
            try
            {
                Write($"Downloading new data | {Game.Version}", ConsoleColor.Red);
                var request = WebRequest.Create($"http://ddragon.leagueoflegends.com/cdn/7.19.1/img/spell/{name}.png");
                var response = request.GetResponse();
                var responseStream = response.GetResponseStream();

                if (responseStream != null)
                {
                    var bitmap = new Bitmap(responseStream);
                    return bitmap;
                }
            }

            catch (Exception e)
            {
                Console.Write(e);
                Console.WriteLine($"Caused by {name}");
                return null;
            }

            return null;
        }

        public static Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            try
            {
                var b = new Bitmap(size.Width, size.Height);
                using (var g = Graphics.FromImage(b))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                }
                return b;
            }
            catch (Exception e)
            {
                Console.WriteLine("Bitmap could not be resized " + e);
                return imgToResize;
            }
        }

        public static void DrawBox(
            Vector2 position,
            int width,
            int height,
            Color color,
            int borderwidth,
            Color borderColor)
        {
            Render.Line(position.X, position.Y, position.X + width, position.Y, height, true, color);

            if (borderwidth <= 0)
            {
                return;
            }

            Render.Line(position.X, position.Y, position.X + width, position.Y, borderwidth, true, borderColor);
            Render.Line(
                position.X,
                position.Y + height,
                position.X + width,
                position.Y + height,
                borderwidth,
                true,
                borderColor);

            Render.Line(position.X, position.Y + 1, position.X, position.Y + height + 1, borderwidth, true, borderColor);
            Render.Line(
                position.X + width,
                position.Y + 1,
                position.X + width,
                position.Y + height + 1,
                borderwidth,
                true,
                borderColor);
        }


        public static void Write(string message, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine("[" + DateTime.Now + "] " + message);
            Console.ResetColor();
        }
    }
}
