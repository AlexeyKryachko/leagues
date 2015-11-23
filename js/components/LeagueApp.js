var React = require('react');
var LeagueStore = require('../stores/LeagueStore');
var LeagueList = require('./LeagueList');

// Method to retrieve state from Stores
function getCartState() {
  return {
	leagues: LeagueStore.getLeagues()    
  };
}

// Define main Controller View
var LeagueApp = React.createClass({

  // Get initial state from stores
  getInitialState: function() {
    return getCartState();
  },

  // Add change listeners to stores
  componentDidMount: function() {
    LeagueStore.addChangeListener(this._onChange);
  },

  // Remove change listeners from stores
  componentWillUnmount: function() {
    LeagueStore.removeChangeListener(this._onChange);
  },

  // Render our child components, passing state via props
  render: function() {
	  console.log('Leagues in LeagueApp: ', this.state.leagues)
  	return (
      <div className="flux-cart-app">
		<div>League app</div>
        <LeagueList leagues={this.state.leagues} />        		
      </div>
  	);
  },

  // Method to setState based upon Store changes
  _onChange: function() {
    this.setState(getCartState());
  }

});

module.exports = LeagueApp;
