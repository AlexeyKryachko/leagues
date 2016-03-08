using Autofac;
using Implementations.Games.BuisnessLogic;
using Implementations.Games.DataAccess;
using Implementations.Teams.BuisnessLogic;
using Implementations.Teams.DataAccess;
using Implementations.Users.DataAccess;
using Interfaces.Games.BuisnessLogic;
using Interfaces.Games.DataAccess;
using Interfaces.Teams.BuisnessLogic;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Dependences
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(TeamsManager)).As<ITeamsManager>();
            builder.RegisterType(typeof(TeamsRepository)).As<ITeamsRepository>();
            
            builder.RegisterType(typeof(UsersRepository)).As<IUsersRepository>();

            builder.RegisterType(typeof(GamesManager)).As<IGamesManager>();
            builder.RegisterType(typeof(GamesRepository)).As<IGamesRepository>();
        }
    }
}
