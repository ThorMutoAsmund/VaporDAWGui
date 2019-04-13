using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporDAWGui
{
    public class Subscribable<TArg>
    {
        public event EventHandler<TArg> ValueChanged;

        private TArg _value;
        public TArg Value
        {
            get => this._value;
            set
            {
                if (!EqualityComparer<TArg>.Default.Equals(this._value, value))
                {
                    this._value = value;
                    this.ValueChanged?.Invoke(this, this._value);
                }
            }
        }

        public Subscribable(TArg defaultValue = default(TArg))
        {
            this._value = defaultValue;
        }
    }
}
