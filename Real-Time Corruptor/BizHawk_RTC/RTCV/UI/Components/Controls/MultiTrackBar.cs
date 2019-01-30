using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.UI.Components.Controls
{
    public partial class MultiTrackBar : UserControl
    {
        public event EventHandler<ValueUpdateEventArgs> ValueChanged;
        public virtual void OnValueChanged(ValueUpdateEventArgs e) => ValueChanged?.Invoke(this, e);

        private bool GeneralUpdateFlag = false; //makes other events ignore firing

        [Description("Net value of the control (displayed in numeric box)"), Category("Data")]
        public long Value { get; set; } = 1;

        private bool FirstLoadDone = false;
        private long _Maximum = 500000;
        [Description("Maximum value of the control"), Category("Data")]
        public long Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                _Maximum = value;
                if (FirstLoadDone)
                    tbControlValue_ValueChanged(null, null);
            }
        }

        [Description("Displayed label of the control"), Category("Data")]
        public string LabelText { get { return lbControlName.Text; } set { lbControlName.Text = value; } }

        [Description("Let the NumericBox override the maximum value"), Category("Data")]
        public bool UncapNumericBox { get; set; } = false;

        private Timer updater;
        private int updateThreshold = 250;

        private List<MultiTrackBar> slaveComps = new List<MultiTrackBar>();
        private MultiTrackBar _parent = null;

        private decimal A; //A value for quadratic function Y = AX² used to scale components

        public MultiTrackBar()
        {
            InitializeComponent();

            updater = new Timer();
            updater.Interval = updateThreshold;
            updater.Tick += Updater_Tick;

            //Calculate A for quadratic function
            decimal max_X = Convert.ToDecimal(65536);
            decimal max_X_Squared = max_X * max_X;
            A = Maximum / max_X_Squared;
        }


        /*
        Y = AX²
        5000000 = A * 4294967296

        A = 4294967296
        A = (65536*65536)/Maximum
        */

        private long tbValueToNmValueQuadScale(long tbValue)
        {
            decimal max_X = Convert.ToDecimal(tbControlValue.Maximum);
            decimal max_X_Squared = max_X * max_X;
            decimal A = Maximum / max_X_Squared;

            decimal X = Convert.ToDecimal(tbValue);
            decimal Y = A * (X * X);

            decimal Floored_Y = Math.Floor(Y);
            return Convert.ToInt64(Floored_Y);
        }

        private int nmValueToTbValueQuadScale(decimal Y)
        {
            decimal max_X = Convert.ToDecimal(tbControlValue.Maximum);
            decimal max_X_Squared = max_X * max_X;
            decimal A = Maximum / max_X_Squared;

            decimal X = DecSqrt(Y / A);

            decimal Floored_X = Math.Floor(X);
            return Convert.ToInt32(Floored_X);
        }


        public static decimal DecSqrt(decimal x)
        {
            return (decimal)Math.Sqrt((double)x);
        }

        private void Updater_Tick(object sender, EventArgs e)
        {
            updater.Stop();
            OnValueChanged(new ValueUpdateEventArgs(Value));
            
        }

        public void registerSlave(MultiTrackBar comp)
        {
            slaveComps.Add(comp);
            comp._parent = this;

        }

        private void MultiTrackBar_Comp_Load(object sender, EventArgs e)
        {
            FirstLoadDone = true;
        }

        private void UpdateAllControls(long nmValue, long tbValue, Control setter)
        {
            GeneralUpdateFlag = true;

            if (setter != this)
            {

                if (setter != tbControlValue)
                {
                    if (tbValue > 65536)
                        tbControlValue.Value = Convert.ToInt32(65536);
                    else
                        tbControlValue.Value = Convert.ToInt32(tbValue);
                }

                if (setter != nmControlValue)
                    if (nmValue > Maximum && !UncapNumericBox)
                        nmControlValue.Value = Convert.ToInt32(Maximum);
                    else
                        nmControlValue.Value = nmValue;

                foreach (var slave in slaveComps)
                    slave.UpdateAllControls(nmValue, tbValue, this);

                if (_parent != null)
                    _parent.UpdateAllControls(nmValue, tbValue, setter);

            }

            GeneralUpdateFlag = false;
        }

        private void PropagateValue(long nmValue, long tbValue, Control setter)
        {
            UpdateAllControls(nmValue, tbValue, setter);

            Value = nmValue;
            updater.Stop();
            updater.Start();
        }

        private void tbControlValue_ValueChanged(object sender, EventArgs e)
        {
            if (GeneralUpdateFlag)
                return;

            int tbValue = tbControlValue.Value;
            long nmValue = tbValueToNmValueQuadScale(tbValue);

            PropagateValue(nmValue, tbValue, tbControlValue);
        }

        private void nmControlValue_ValueChanged(object sender, EventArgs e)
        {
            if (GeneralUpdateFlag)
                return;

            long nmValue = Convert.ToInt64(nmControlValue.Value);
            int tbValue = nmValueToTbValueQuadScale(nmControlValue.Value);

            PropagateValue(nmValue, tbValue, nmControlValue);
        }
    }

    internal class NoFocusTrackBar : System.Windows.Forms.TrackBar
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public extern static int SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        private static int MakeParam(int loWord, int hiWord)
        {
            return (hiWord << 16) | (loWord & 0xffff);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            SendMessage(this.Handle, 0x0128, MakeParam(1, 0x1), 0);
        }
    }

    public class ValueUpdateEventArgs : EventArgs
    {
        public long value;

        public ValueUpdateEventArgs(long _value)
        {
            value = _value;
        }
    }
}
