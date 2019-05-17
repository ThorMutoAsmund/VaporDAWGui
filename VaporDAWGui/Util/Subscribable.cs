using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporDAWGui
{
    public class Subscribable<TArg>
    {
        public event Action<TArg> ValueChanged;

        public TArg Value
        {
            get => this._value;
            set
            {
                if (!EqualityComparer<TArg>.Default.Equals(this._value, value))
                {
                    this._value = value;
                    this.ValueChanged?.Invoke(value);
                }
            }
        }

        private TArg defaultValue;
        private TArg _value;

        public Subscribable(TArg defaultValue = default(TArg))
        {
            this.defaultValue = defaultValue;
            this._value = defaultValue;
        }

        public void Clear()
        {
            this.Value = this.defaultValue;
        }
    }
}
