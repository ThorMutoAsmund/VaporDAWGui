const chunksize = 100;

class Smp
{
  constructor(a) {
    this.a = a;
    this.buf = new Array(chunksize);
  }
  work(t1, len) {
    for (var t = t1, b = 0; t < len; ++t) {
      this.buf[b++] = [t+this.a,t*this.a];
    }
    return this.buf;
  }
}
var inputs = [
  new Smp(2),
  new Smp(3),
  new Smp(4),
  new Smp(2),
  new Smp(3),
  new Smp(4),
  new Smp(2),
  new Smp(3),
  new Smp(4)
];

// var work = t =>  inputs.reduce((total, input) => {
//       var v = input.work(t);
//       return [total[0]+v[0], total[1]+v[1]];
//     }, [0,0]);

// 92ms
var buf = new Array(chunksize);
var work = (t1, len) => {
  var i = 0;
  inputs.forEach(input => {
    var ibuf = input.work(t1, len);
    if (i == 0) {
      for (var t = t1, b = 0; t < len; ++t) {
        buf[b][0] = ibuf[b][0];
        buf[b][1] = ibuf[b][1];
        b++;
      }
    }
    else {
      for (var t = t1, b = 0; t < len; ++t) {
        buf[b][0] += ibuf[b][0];
        buf[b][1] += ibuf[b][1];
        b++;
      }
    }
    i++;
  });
  return buf;
};

var hrstart = process.hrtime();
for (var t = 0; t < 100000; ++t) {
  var res = work(t);
}
hrend = process.hrtime(hrstart);

console.info('%ds %dms', hrend[0], hrend[1] / 1000000);


