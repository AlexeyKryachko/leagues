using Autofac;
using Implementations.Games.BuisnessLogic;
using Implementations.Games.DataAccess;
using Implementations.Leagues.BuisnessLogic;
using Implementations.Leagues.DataAccess;
using Implementations.Teams.BuisnessLogic;
using Implementations.Teams.DataAccess;
using Implementations.Users.BuisnessLogic;
using Implementations.Users.DataAccess;
using Interfaces.Core.Authentication;
using Interfaces.Games.BuisnessLogic;
using Interfaces.Games.DataAccess;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Leagues.DataAccess;
using Interfaces.Teams.BuisnessLogic;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.BuisnessLogic;
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
            builder.RegisterType(typeof(UsersManager)).As<IUsersManager>();

            builder.RegisterType(typeof(GamesManager)).As<IGamesManager>();
            builder.RegisterType(typeof(GamesRepository)).As<IGamesRepository>();

            builder.RegisterType(typeof(LeaguesManager)).As<ILeaguesManager>();
            builder.RegisterType(typeof(LeaguesRepository)).As<ILeaguesRepository>();

            builder.RegisterType(typeof(Implementations.Core.Authentication.AuthenticationManager)).As<IAuthenticationManager>();
        }
    }
}
