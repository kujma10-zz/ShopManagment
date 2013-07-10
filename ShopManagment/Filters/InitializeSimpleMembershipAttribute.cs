using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using ShopManagment.Models;
using System.Web.Security;

namespace ShopManagment.Filters
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
                Database.SetInitializer<ShopEntities>(null);
                try
                {
                    using (var context = new ShopEntities())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                        WebSecurity.InitializeDatabaseConnection("Membership", "Admins", "ID", "Username", autoCreateTables: true);
                        if (!Roles.RoleExists("StorageOperator"))
                            Roles.CreateRole("StorageOperator");
                        if (!Roles.RoleExists("ShopOperator"))
                            Roles.CreateRole("ShopOperator");
                        if (!Roles.RoleExists("ShopManager"))
                            Roles.CreateRole("ShopManager");
                        if (!WebSecurity.UserExists("kote"))
                            WebSecurity.CreateUserAndAccount("kote", "123456");
                        if (!WebSecurity.UserExists("kikola"))
                            WebSecurity.CreateUserAndAccount("kikola", "123456");
                        if (!WebSecurity.UserExists("jilberta"))
                            WebSecurity.CreateUserAndAccount("jilberta", "123456");
                        if (!WebSecurity.UserExists("shota"))
                            WebSecurity.CreateUserAndAccount("shota", "123456");
                        if (!WebSecurity.UserExists("misha"))
                            WebSecurity.CreateUserAndAccount("misha", "123456");
                        if (!Roles.IsUserInRole("kote", "StorageOperator"))
                            Roles.AddUserToRole("kote", "StorageOperator");
                        if (!Roles.IsUserInRole("kikola", "ShopOperator"))
                            Roles.AddUserToRole("kikola", "ShopOperator");
                        if (!Roles.IsUserInRole("jilberta", "ShopManager"))
                            Roles.AddUserToRole("jilberta", "ShopManager");
                        if (!Roles.IsUserInRole("shota", "ShopOperator"))
                            Roles.AddUserToRole("shota", "ShopOperator");
                        if (!Roles.IsUserInRole("misha", "ShopOperator"))
                            Roles.AddUserToRole("misha", "ShopOperator");

                    }


                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
