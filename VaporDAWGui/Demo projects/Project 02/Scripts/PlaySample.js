
var setup = { inputs: [ { name: "Sample", default: true } ];

function generate() {
    return (t, inputs, options) => SampleAtTime(inputs.Sample, t);
}

module.exports = { setup, generate };