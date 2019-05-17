/*
 * Functions not required by plugins
 */
 
const fs = require('fs');
const WavDecoder = require("wav-decoder");
 
var preloadedSamples = [];

const readFile = filePath =>
  new Promise((resolve, reject) => {
    fs.readFile(filePath, (err, buffer) => {
      return err ? reject(err) : resolve(buffer);
    });
  });

const readWav = filePath =>
  readFile(filePath).then(buffer => WavDecoder.decode(buffer));

const preloadSample = (id, filePath) =>
  readWav(filePath).then(sample => {
    preloadedSamples[id] = sample;
  });

const getPreloadedSample = id => {
  return preloadedSamples[id];
}

module.exports = {
  readWav,
  preloadSample,
  getPreloadedSample 
}