var LeagueListActions = require('../actions/LeagueListActions');

module.exports = {

  // Load mock product data from localStorage into ProductStore via Action
  getData: function() {
    var data = [
      {
        id: '1',
        name: 'SPB League 2015',
        image: 'scotch-beer.png',
        description: 'SPB League 2015 decription.'
      },
	  {
        id: '2',
        name: 'Minsk League 2015',
        image: 'scotch-beer.png',
        description: 'Minsk League 2015 decription.'
      },
	  {
        id: '2',
        name: 'Moscow League 2015',
        image: 'scotch-beer.png',
        description: 'Moscow League 2015 decription.'
      }
    ];
    LeagueListActions.receiveData(data);
  }
};