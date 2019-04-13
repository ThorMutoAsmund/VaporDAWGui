
function tremolo(t, in, options) {
  return in.output(t).map(s => s * Math.sin(t *2 * Math.PI / options.speed));
}

module.exports = tremolo;