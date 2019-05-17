
class Smp
{
  constructor(a) {
    this.a = a;
  }
  work(t) {
    return [t+this.a,t*this.a];
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
var work = t => {
  var total = [0,0];
  inputs.forEach(input => {
    var v = input.work(t);  
    total[0] += v[0];
    total[1] += v[1];
  });
  return total;
};

var hrstart = process.hrtime();
for (var t = 0; t < 100000; ++t) {
  var res = work(t);
}
hrend = process.hrtime(hrstart);

console.info('%ds %dms', hrend[0], hrend[1] / 1000000);


