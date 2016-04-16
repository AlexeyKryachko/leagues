using Autofac;
using Implementations.Authenticate.BuisnessLogic;
using Implementations.Core.Medias.BuisnessLogic;
using Implementations.Core.Medias.DataAccess;
using Implementations.Games.BuisnessLogic;
using Implementations.Games.DataAccess;
using Implementations.Leagues.BuisnessLogic;
using Implementations.Leagues.DataAccess;
using Implementations.Teams.BuisnessLogic;
using Implementations.Teams.DataAccess;
using Implementations.Users.BuisnessLogic;
using Implementations.Users.DataAccess;
using Interfaces.Authenticate.BuisnessLogic;
using Interfaces.Core.Services.Medias.BuisnessLogic;
using Interfaces.Core.Services.Medias.DataAccess;
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
            builder.RegisterType(typeof(PlayersRepository)).As<IPlayersRepository>();
            builder.RegisterType(typeof(UsersManager)).As<IUsersManager>();

            builder.RegisterType(typeof(GamesManager)).As<IGamesManager>();
            builder.RegisterType(typeof(GamesRepository)).As<IGamesRepository>();

            builder.RegisterType(typeof(LeaguesManager)).As<ILeaguesManager>();
            builder.RegisterType(typeof(LeaguesRepository)).As<ILeaguesRepository>();

            builder.RegisterType(typeof(AuthenticateManager)).As<IAuthenticateManager>();

            builder.RegisterType(typeof(MediaManager)).As<IMediaManager>();
            builder.RegisterType(typeof(MediaRepository)).As<IMediaRepository>();
        }
    }
}
