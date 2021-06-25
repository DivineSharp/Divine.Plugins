using System;
using System.Linq;

using Divine.Entity;
using Divine.Entity.Entities.Abilities.Components;
using Divine.GameConsole;
using Divine.Helpers;
using Divine.Order;
using Divine.Order.EventArgs;
using Divine.Order.Orders.Components;
using Divine.Service;
using Divine.Update;

namespace Divine.Plugin
{
    internal sealed class Bootstrap : BootstrapperTEST
    {
        public Bootstrap()
        {
            //Console.WriteLine("Bootstrap Divine.Plugin: " + Environment.CurrentManagedThreadId);
        }

        protected override void OnActivateUnsafe()
        {
            //Console.WriteLine("OnActivateUnsafe Divine.Plugin: " + Environment.CurrentManagedThreadId);
        }

        protected override void OnDeactivate()
        {
            //Console.WriteLine("OnDeactivate Divine.Plugin: " + Environment.CurrentManagedThreadId);
        }

        protected override void OnPreCache()
        {
            //Console.WriteLine("OnPreCache Divine.Plugin: " + Environment.CurrentManagedThreadId);
        }

        protected override void OnPreCacheUnsafe()
        {
            //Console.WriteLine("OnPreCacheUnsafe Divine.Plugin: " + Environment.CurrentManagedThreadId);
        }

        protected override void OnPreDeactivate()
        {
            //Console.WriteLine("OnPreDeactivate Divine.Plugin: " + Environment.CurrentManagedThreadId);
        }

        protected override void OnActivate()
        {
            //Console.WriteLine("Activate Divine.Plugin: " + Environment.CurrentManagedThreadId);

            GameConsoleManager.SetValue("dota_creeps_no_spawning", 1);
        }

        protected override void OnPreActivate()
        {
            Console.WriteLine("------------------- Divine.Plugin -------------------");

            UpdateManager.Update += UpdateManager_Update;

            /*var list = new HashSet<string>();

            foreach (var item in new DirectoryInfo(@"C:\Users\RoccoZero\Documents\GitHub\Divine").GetFiles("*.cs", SearchOption.AllDirectories))
            {
                try
                {
                    list.Add(item.DirectoryName[@"C:\Users\RoccoZero\Documents\GitHub\".Length..].Replace("\\", "."));
                }
                catch
                {
                }
            }

            var sb = new StringBuilder();

            foreach (var item in list)
            {
                sb.Append("using ");
                sb.Append(item);
                sb.Append(";\\n");
                sb.Append("    ");
            }

            Console.WriteLine(sb);*/
        }

        private void UpdateManager_Update()
        {
            if (MultiSleeper<int>.Sleeping(777))
            {
                return;
            }

            MultiSleeper<int>.Sleep(777, 2000);

            for (int i = 0; i < 3; i++)
            {
                //EntityManager.LocalHero.Spellbook.Spell2.Cast();
            }
            

            //Console.WriteLine(EntityManager.LocalHero == null);

            //EntityManager.LocalHero.Move(GameManager.MousePosition);
        }
    }
}