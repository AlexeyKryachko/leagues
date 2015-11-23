module.exports = {
  // Load Mock Product Data Into localStorage
  init: function() {
    localStorage.clear();
    localStorage.setItem('leagues', JSON.stringify([
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
    ]));
  }

};