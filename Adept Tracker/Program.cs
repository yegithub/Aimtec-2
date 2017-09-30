namespace Adept_Tracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var spellTracker = new SpellTracker();
            var jungleCamp = new JungleTracker();
            jungleCamp.Load();
        }
    }
}
