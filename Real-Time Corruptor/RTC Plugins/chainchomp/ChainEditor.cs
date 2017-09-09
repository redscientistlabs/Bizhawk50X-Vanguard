using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChainChompCorruptor;
using System.IO;

namespace ChainChomp
{
    public partial class ChainEditor : UserControl
    {
        public Chain chain;
        private TabControl chainTab;
        private TabPage parentTab;
        private string lastWorkingDir = ChainChompApplication.defaultChainPath;

        public ChainEditor(TabControl chainTabControl)
        {
            InitializeComponent();

            chain = new Chain();
            chainTab = chainTabControl;
            parentTab = chainTab.TabPages[chainTab.TabCount - 2];

            //set some defaults
            pluginDropDown.Items.AddRange(PluginManager.Current.GetCorruptorNames());
            if (pluginDropDown.Items.Count > 0)
            {
                pluginDropDown.SelectedIndex = 0;
            }

            if (ChainChompApplication.game != null)
            {
                endOffsetTicker.Value = ChainChompApplication.game.length;
            }

        }

        public ChainEditor(TabControl parentTabControl, string chainFilePath)
            : this(parentTabControl)
        {
            SetChain(new Chain(chainFilePath));
        }

        private void SetChain(Chain c)
        {
            ClearRack();
            chain = c;
            FillRack();

            parentTab.Text = chain.name;
        }

        // Chain Rack Methods
        private void ClearRack()
        {
            for (int i = rack.Controls.Count-1; i >= 0; i--)
            {
                rack.Controls[i].Hide();
                RemoveFromRack(rack.Controls[i]);
            }
        }


        private void FillRack()
        {
            chain.GetPlugins().ForEach(i => AddToRack(i));
            for (int i = 0; i < rack.Controls.Count; i++)
            {
                if (i % 2 == 0 && i/2 < chain.headerIndices.Count)
                {
                    (rack.Controls[i] as RackHeader).BGIndex = chain.headerIndices[i/2];
                }
            }
        }

        private void AddToRack(Plugin p)
        {
            Control c = (Control)p.corruptor.GetBackgroundContainer();
            RackHeader h = new RackHeader(p, rack);
            //bind custom events
            c.MouseClick += clickOnPlugin;
            h.MovePluginUp += movePluginUp;
            h.MovePluginDown += movePluginDown;

            //add to the panel
            rack.Controls.Add(h);
            rack.Controls.Add((Control)p.corruptor.GetUserControl());
        }

        private void RemoveFromRack(Control c)
        {
            rack.Controls.Remove(c);
        }

        // Chain IO
        private void openChainButton_Click(object sender, EventArgs e)
        {
            openChainDialog.InitialDirectory = lastWorkingDir;
            if (openChainDialog.ShowDialog() == DialogResult.OK)
            {
                //set chain
                SetChain(new Chain(openChainDialog.FileName));

                //set offsets
                startOffsetTicker.Value = chain.startOffset;
                endOffsetTicker.Value = chain.endOffset;

                //remember dir
                lastWorkingDir = Path.GetDirectoryName(openChainDialog.FileName);
            }
        }

        private void saveChainButton_Click(object sender, EventArgs e)
        {
            saveChainDialog.InitialDirectory = lastWorkingDir;
            if (saveChainDialog.ShowDialog() == DialogResult.OK)
            {
                //get header indices
                List<int> indices = new List<int>();
                for (int i = 0; i < rack.Controls.Count; i = i + 2)
                {
                    indices.Add((rack.Controls[i] as RackHeader).BGIndex);
                }

                chain.SaveChain(saveChainDialog.FileName, indices);
                parentTab.Text = chain.name;

                lastWorkingDir = Path.GetDirectoryName(saveChainDialog.FileName);
            }
        }

        // Chain Expansion
        private void pluginDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            IChainChompCorruptor c = PluginManager.Current.GetCorruptor((ComboBoxItem)pluginDropDown.SelectedItem);
            editorToolTip.SetToolTip(pluginDropDown, string.Format("{0} (Version {1} by {2})", c.Name, c.Version, c.Author));
        }

        private void addToChainButton_Click(object sender, EventArgs e)
        {
            if (pluginDropDown.SelectedItem != null)
            {
                AddToRack(chain.AddPlugin((int)(pluginDropDown.SelectedItem as ComboBoxItem).Value));
                //clicked the add button, so navigate to the new plugin
                rack.ScrollControlIntoView(rack.Controls[rack.Controls.Count - 1]);
                pluginDropDown.Focus();

            }
        }

        private void setToROMEnd_Click(object sender, EventArgs e)
        {
            if (ChainChompApplication.game != null)
            {
                endOffsetTicker.Value = ChainChompApplication.game.length;
            }
        }

        private void startOffsetTicker_ValueChanged(object sender, EventArgs e)
        {
            if (endOffsetTicker.Value < startOffsetTicker.Value)
            {
                startOffsetTicker.Value = endOffsetTicker.Value;
            }
            chain.startOffset = (int)startOffsetTicker.Value;
        }

        private void endOffsetTicker_ValueChanged(object sender, EventArgs e)
        {
            if (startOffsetTicker.Value > endOffsetTicker.Value)
            {
                startOffsetTicker.Value = endOffsetTicker.Value;
            }
            chain.endOffset = (int)endOffsetTicker.Value;
        }

        private void removeChainButton_Click(object sender, EventArgs e)
        {
            int i = chainTab.SelectedIndex - 1;
            chainTab.TabPages.Remove(parentTab);
            chainTab.SelectedIndex = i;
        }

        private void rack_ControlRemoved(object sender, ControlEventArgs e)
        {
            //ignore rack headers since they aren't stored in the chain
            if (e.Control.GetType() != typeof(RackHeader))
            {
                chain.RemoveCorruptor(e.Control);
            }
        }

        //custom event handlers
        public void movePluginUp(object sender, PluginMoveEventArgs e)
        {
            chain.ShiftPlugin(e.movingPlugin, -1);
        }

        public void movePluginDown(object sender, PluginMoveEventArgs e)
        {
            chain.ShiftPlugin(e.movingPlugin, 1);
        }

        private void clickOnPlugin(object sender, EventArgs e)
        {
            (sender as Control).Focus();
        }
        private void rack_Click(object sender, EventArgs e)
        {
            rack.Focus();
        }

        private void enableChainCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(enableChainCheckBox.Checked)
            {
                editorToolTip.SetToolTip(enableChainCheckBox, "This chain is enabled and will be run");
            }
            else
            {
                editorToolTip.SetToolTip(enableChainCheckBox, "This chain is disabled and will be ignored :(");
            }
        }

		public void ResizeRack(int height)
		{
			rack.Height = height;
			rack.AutoScrollMinSize = new Size(rack.AutoScrollMinSize.Width, height + 200);
		}
    }
}
