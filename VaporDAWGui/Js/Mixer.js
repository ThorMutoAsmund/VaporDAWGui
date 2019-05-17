const DAW = require('./DAW');

class Mixer {
  constructor() {
  }

  setup() {
    return {
      params: [
      ],
      multiInputs: true,
      audible: true
    };
  }

  prepare(params) {
    this.inputs = params.inputs;
  }

  work(t) {
    var total = [0,0];
    this.inputs.forEach(input => {
      var v = input.work(t);  
      total[0] += v[0];
      total[1] += v[1];
    });
    return total;
  }
}

module.exports = Mixer;
