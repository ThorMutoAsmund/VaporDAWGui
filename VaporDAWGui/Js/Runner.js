const DAW = require('./DAW');
const Util = require('./Util');
const Mixer = require('./Mixer');
const Sampler = require('./Sampler');

// Load samples
var preload = [
  Util.preloadSample('s1', 'test.wav')
];

// Generate song
var generateSong = (t0, len) => {
  var sampler = new Sampler();
  var samplerSetup = sampler.setup();
  sampler.prepare(
    { sample: 's1', offset: samplerSetup.params.offset.default }
  );
  var sampler2 = new Sampler();
  var sampler2Setup = sampler2.setup();
  sampler2.prepare(
    { sample: 's1', offset: 10 }
  );

  var mixer = new Mixer();
  var mixerSetup = mixer.setup();
  mixer.prepare(
    { inputs: [sampler, sampler2] }
  );

  var output = new Array(len);
  for (var t = t0, u = 0; t < t0+len; ++t) {
    output[u++] = mixer.work(t);
  }
  return output;
};

Promise.all(preload).
  then(() => generateSong(0,10)).
  then(console.log);
