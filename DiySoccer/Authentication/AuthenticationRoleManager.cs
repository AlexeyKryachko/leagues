using Authentication.Mongo;
using Interfaces.Users.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Authentication
{
    public class AuthenticationRoleManager : RoleManager<UserRoleDb>
    {
        public AuthenticationRoleManager(IRoleStore<UserRoleDb, string> roleStore)
            : base(roleStore)
        {
        }

        public static AuthenticationRoleManager Create(IdentityFactoryOptions<AuthenticationRoleManager> options, IOwinContext context)
        {
            var store = new MongoRoleStore(context.Get<ApplicationIdentityContext>().Roles);
            var manager = new AuthenticationRoleManager(store);

            return manager;
        }
    }
}