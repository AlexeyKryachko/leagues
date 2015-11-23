var React = require('react');

var LeagueList = React.createClass({

  render: function() {
    var self = this;
	var leagues = this.props.leagues;
	console.log('Leagues in LeagueList: ', leagues)
    return (
      <div className="flux-league">
        <div className="mini-league">          
          <ul>
            {Object.keys(leagues).map(function(league){
              return (
                <li key={league}>
                  <h1 className="name">{leagues[league].name}</h1>
				  <p className="description">{leagues[league].decription}</p>
                  <img src={leagues[league].image} ></img>
                </li>
              )
            })}
          </ul>
        </div>        
      </div>
    );
  },

});

module.exports = LeagueList;