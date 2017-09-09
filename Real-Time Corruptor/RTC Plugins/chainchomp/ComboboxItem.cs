using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainChomp
{
    class ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public ComboBoxItem(string text)
        {
            Text = text;
            Value = null;
        }

        public ComboBoxItem(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
