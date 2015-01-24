using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using Sah.Models;
using System.Collections.Generic;
using System.Web.Security;

namespace Sah.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<SahPodaci>(null);

                try
                {
                    using (var context = new SahPodaci())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Osoba", "idOsoba", "korisnickoIme", autoCreateTables: true);
                    RegisterRoles();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
            protected void RegisterRoles()
            {

                ISet<String> NeededRoles = new SortedSet<String>();
                NeededRoles.Add(UlogeKorisnika.admin.ToString());
                NeededRoles.Add(UlogeKorisnika.obican.ToString());

                String[] roles = Roles.GetAllRoles();
                foreach (String s in roles)
                    NeededRoles.Remove(s);

                foreach (String s in NeededRoles)
                    Roles.CreateRole(s);
            }
        }
    }
}
