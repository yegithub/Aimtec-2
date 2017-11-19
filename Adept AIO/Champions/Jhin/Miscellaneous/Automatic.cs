namespace Adept_AIO.Champions.Jhin.Miscellaneous
{
    using System;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;
    using GameObjects = Aimtec.SDK.Util.Cache.GameObjects;

    class Automatic
    {
        public Automatic()
        {
            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate()
        {
            try
            {
                if (Global.Player.IsRecalling() || Global.Orbwalker.IsWindingUp)
                {
                    return;
                }
             
                Global.Orbwalker.MovingEnabled = Global.Player.SpellBook.GetSpell(SpellSlot.R).Name != "JhinRShot";
               
                if (SpellManager.W.Ready &&
                    MenuConfig.Misc["W"].Enabled)
                {
                    var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellManager.W.Range) && x.HasBuff("jhinespotteddebuff"));
                    if (target != null && target.Distance(Global.Player) > Global.Player.AttackRange + 200)
                    {
                        SpellManager.CastW(target);
                    }
                }

                if (!SpellManager.E.Ready)
                {
                    return;
                }

                var tp = GameObjects.Get<Obj_AI_Minion>().FirstOrDefault(x => x.IsEnemy && x.Distance(Global.Player) <= SpellManager.E.Range && x.HasBuff("teleport_target"));
                if (tp != null)
                {
                    SpellManager.E.Cast(tp.ServerPosition);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
