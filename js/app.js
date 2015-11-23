var React = require('react');

var ReactRouter = require('react-router');
var Router = ReactRouter.Router;
var Route = ReactRouter.Route;

var createHistory = require('history').createHistory;
var useBasename = require('history').useBasename;

var LeagueAPI = require('./utils/LeagueAPI')
var LeagueApp = require('./components/LeagueApp');

console.log('React')

const history = useBasename(createHistory)({
  basename: '/begin'
})

// Load Mock API Call
LeagueAPI.getData();

// Render FluxCartApp Controller View
/*React.render(
  <LeagueApp />,
  document.getElementById('flux-league-list')
);*/


React.render((
  <Router history={history}>
	<Route path="/" component={LeagueApp}></Route>  
  </Router>
), document.getElementById('flux-league-list'))
