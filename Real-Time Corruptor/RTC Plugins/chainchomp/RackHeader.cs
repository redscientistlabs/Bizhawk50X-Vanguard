using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ChainChomp
{
    public partial class RackHeader : UserControl
    {
        private Plugin plugin;
        private FlowLayoutPanel rack;
        private string lastPresetDir = ChainChompApplication.factoryPresetsPath;
        private string selectedPreset;

        //chain-reorder events
        public EventHandler<PluginMoveEventArgs> MovePluginUp;
        public EventHandler<PluginMoveEventArgs> MovePluginDown;

        //background niceties
        private static Bitmap[] backgrounds = new Bitmap[] 
        { 
            ChainChomp.Properties.Resources.headerDarkBlue,
            ChainChomp.Properties.Resources.headerMedBlue,
            ChainChomp.Properties.Resources.headerBlue,
            ChainChomp.Properties.Resources.headerCyan,
            ChainChomp.Properties.Resources.headerTeal,
            ChainChomp.Properties.Resources.headerGreen,
            ChainChomp.Properties.Resources.headerLightGreen,            
            ChainChomp.Properties.Resources.headerLime,
            ChainChomp.Properties.Resources.headerYellow,
            ChainChomp.Properties.Resources.headerAmber,
            ChainChomp.Properties.Resources.headerOrange,
            ChainChomp.Properties.Resources.headerOrangeRed,
            ChainChomp.Properties.Resources.headerRed,
            ChainChomp.Properties.Resources.headerMagenta,
            ChainChomp.Properties.Resources.headerPink,
            ChainChomp.Properties.Resources.headerPurple,
            ChainChomp.Properties.Resources.headerGrape
        };
        private int bgIndex;
        public int BGIndex
        {
            get
            {
                return bgIndex;
            }
            set
            {
                headerTableLayout.BackgroundImage = backgrounds[value];
                bgIndex = value;
            }
        }
        private static Random rnd = new Random();

        public RackHeader()
        {
            InitializeComponent();
            BGIndex = rnd.Next(0, backgrounds.Count());

            //hook usercontrol
            positionSwitch1.UpButtonClick += new EventHandler(positionSwitch1_UpButton);
            positionSwitch1.DownButtonClick += new EventHandler(positionSwitch1_DownButton);

        }

        public RackHeader(Plugin p, FlowLayoutPanel r)
            : this()
        {
            plugin = p;
            rack = r;

            //set dialog file info
            savePresetDialog.DefaultExt = p.corruptor.PresetExt;
            openPresetDialog.DefaultExt = p.corruptor.PresetExt;

            string filter = string.Format("{0} preset files|*.{1}|All files|*.*", p.corruptor.Name, p.corruptor.PresetExt);
            savePresetDialog.Filter = filter;
            openPresetDialog.Filter = filter;

            lastPresetDir += p.corruptor.Name;
            savePresetDialog.InitialDirectory = lastPresetDir;
            openPresetDialog.InitialDirectory = lastPresetDir;

            FillPresetList();
        }

        private void loadPresetButton_Click(object sender, EventArgs e)
        {
            if (openPresetDialog.ShowDialog() == DialogResult.OK)
            {
                LoadPreset(openPresetDialog.FileName);
                selectedPreset = Path.GetFileNameWithoutExtension(openPresetDialog.FileName);
                RestoreSelectedPreset();
            }
        }

        private void LoadPreset(string path)
        {
            plugin.LoadSettings(path, true);
            lastPresetDir = Path.GetDirectoryName(path);
        }

        private void savePresetButton_Click(object sender, EventArgs e)
        {
            if (savePresetDialog.ShowDialog() == DialogResult.OK)
            {
                plugin.SaveSettings(savePresetDialog.FileName);
                lastPresetDir = Path.GetDirectoryName(savePresetDialog.FileName);
                FillPresetList();
                selectedPreset = Path.GetFileNameWithoutExtension(savePresetDialog.FileName);
                RestoreSelectedPreset();
            }
        }

        private void removePluginButton_Click(object sender, EventArgs e)
        {
            int index = rack.Controls.IndexOf((Control)plugin.corruptor.GetUserControl());
            rack.Controls.RemoveAt(index);
            rack.Controls.RemoveAt(index-1);
        }

        private void presetsList_DropDown(object sender, EventArgs e)
        {
            FillPresetList();
        }

        private void FillPresetList()
        {
            
            presetsList.Items.Clear();

            IEnumerable<string> presets = Directory.EnumerateFiles(lastPresetDir, "*" + plugin.corruptor.PresetExt);
            presets.ToList().ForEach(i => presetsList.Items.Add(new ComboBoxItem(Path.GetFileNameWithoutExtension(i), i)));
            
        }

        private void RestoreSelectedPreset()
        {

            ComboBoxItem[] items = new ComboBoxItem[presetsList.Items.Count];
            presetsList.Items.CopyTo(items, 0);
            ComboBoxItem previousSel = items.FirstOrDefault(i => i.Text == selectedPreset);
            if (previousSel != null)
            {
                presetsList.SelectedItem = previousSel;
            }
        }

        private void presetsList_DropDownClosed(object sender, EventArgs e)
        {
            RestoreSelectedPreset();
        }

        private void presetsList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBoxItem item = (presetsList.SelectedItem as ComboBoxItem);
            selectedPreset = item.Text;
            LoadPreset((string)item.Value);

        }

        private void positionSwitch1_DownButton(object sender, EventArgs e)
        {
            //reorder rack
            Control item = (Control)plugin.corruptor.GetUserControl();
            int i = rack.Controls.IndexOf(item);
            if (i < rack.Controls.Count - 1)
            {
                rack.Controls.SetChildIndex(item, i + 2);
                i = rack.Controls.IndexOf(this);
                rack.Controls.SetChildIndex(this, i + 2);
                rack.ScrollControlIntoView(item);
            }
            //raise re-order event for chain
            if (MovePluginDown != null)
            {
                MovePluginDown(this, new PluginMoveEventArgs(plugin));
            }
        }

        private void positionSwitch1_UpButton(object sender, EventArgs e)
        {
            Control item = (Control)plugin.corruptor.GetUserControl();
            int i = rack.Controls.IndexOf(this);
            if (i > 0)
            {
                rack.Controls.SetChildIndex(this, i - 2);
                i = rack.Controls.IndexOf(item);
                rack.Controls.SetChildIndex(item, i - 2);
                rack.ScrollControlIntoView(item);
            }
        }

        private void hiddenButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                BGIndex = BGIndex + 1 < backgrounds.Count() ? BGIndex + 1 : 0;
            }
            else if (e.Button == MouseButtons.Right)
            {
                BGIndex = BGIndex - 1 > -1 ? BGIndex - 1 : backgrounds.Count() - 1;
            }
            rack.Focus();
        }

    }
}
