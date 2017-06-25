using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.ComponentModel;

namespace RTC
{

    public class LabelPassthrough : Label
    {

        protected override void OnPaint(PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics, this.Text.ToString(), this.Font, ClientRectangle, ForeColor);
        }

    }

    public class MenuButton : Button
    {
        [DefaultValue(null)]
        public ContextMenuStrip Menu { get; set; }

        public void SetMenu(ContextMenuStrip _menu)
        {
            Menu = _menu;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if (Menu != null && mevent.Button == MouseButtons.Left)
            {
                Menu.Show(this, mevent.Location);
            }
        }

        /*
        protected override void OnPaint(PaintEventArgs pevent)
        {

            base.OnPaint(pevent);
            
            int arrowX = ClientRectangle.Width - 14;
            int arrowY = ClientRectangle.Height / 2 - 1;

            Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
            Point[] arrows = new Point[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
            pevent.Graphics.FillPolygon(brush, arrows);
             
        }
        */
    }

}
