/*
 * Functions used by plugins
 */

const Util = require('./Util');

const getSample = id =>
  Util.getPreloadedSample(id);
  
module.exports = { 
  getSample 
}