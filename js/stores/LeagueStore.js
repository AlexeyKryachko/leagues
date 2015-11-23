var AppDispatcher = require('../dispatcher/AppDispatcher');
var EventEmitter = require('events').EventEmitter;
var LeagueListConstants = require('../constants/LeagueListConstants');
var _ = require('underscore');

// Define initial data points
var _leagues = {}

// Add product to cart
/*function add(sku, update) {
  update.quantity = sku in _products ? _products[sku].quantity + 1 : 1;
  _products[sku] = _.extend({}, _products[sku], update)
}

// Set cart visibility
function setCartVisible(cartVisible) {
  _cartVisible = cartVisible;
}

// Remove item from cart
function removeItem(sku) {
  delete _products[sku];
}*/

// Extend Cart Store with EventEmitter to add eventing capabilities
var LeagueStore = _.extend({}, EventEmitter.prototype, {

  // Return cart items
  getLeagues: function() {
    return _leagues;
  },
  getLeague: function(leagueId) {
    return _findWhere(_leagues, { id: leagueId });
  },

  // Return # of items in cart
  /*getCartCount: function() {
    return Object.keys(_products).length;
  },

  // Return cart cost total
  getCartTotal: function() {
    var total = 0;
    for(product in _products){
      if(_products.hasOwnProperty(product)){
        total += _products[product].price * _products[product].quantity;
      }
    }
    return total.toFixed(2);
  },

  // Return cart visibility state
  getCartVisible: function() {
    return _cartVisible;
  },*/

  // Emit Change event
  emitChange: function() {
    this.emit('change');
  },

  // Add change listener
  addChangeListener: function(callback) {
    this.on('change', callback);
  },

  // Remove change listener
  removeChangeListener: function(callback) {
    this.removeListener('change', callback);
  }

});

// Register callback with AppDispatcher
AppDispatcher.register(function(payload) {
  var action = payload.action;
  var text;

  console.log('[AppDispatcher] Action: ', action);
  switch(action.actionType) {

    // Respond to CART_ADD action
    case LeagueListConstants.RECEIVE_DATA:
      _leagues = action.data
      break;

    default:
      return true;
  }

  // If action was responded to, emit change event
  LeagueStore.emitChange();

  return true;

});

module.exports = LeagueStore;
