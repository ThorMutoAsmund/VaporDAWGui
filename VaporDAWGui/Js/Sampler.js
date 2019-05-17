const DAW = require('./DAW');

class Sampler {
  constructor() {
    this.sample = null;
  }

  setup() {
    return {
      params: {
        sample: { type: 'id', required: true },
        offset: { type: 'int', default: 0 }
      },
      multiInputs: false,
      audible: true
    };
  }

  prepare(params) {
    this.offset = params.offset;
    this.sample = DAW.getSample(params.sample);
  }

  work(t) {
    return [this.sample.channelData[0][t + this.offset], 
      this.sample.channelData[1][t + this.offset]];
  }
}

module.exports = Sampler;
