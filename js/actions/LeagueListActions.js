var AppDispatcher = require('../dispatcher/AppDispatcher');
var LeagueListConstants = require('../constants/LeagueListConstants');

// Define action methods
var LeagueListActions = {

  // Receive inital product data
  receiveData: function(data) {
    AppDispatcher.handleAction({
      actionType: LeagueListConstants.RECEIVE_DATA,
      data: data
    })
  },

};

module.exports = LeagueListActions;
