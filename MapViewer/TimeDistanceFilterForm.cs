using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Filters;

namespace MapViewer
{
    public partial class TimeDistanceFilterForm : Form
    {
        TrackGridData trackData;

        public TimeDistanceFilterForm(TrackGridData trackData)
        {
            InitializeComponent();
            this.trackData = trackData;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();

            save.Title = "Save Track Log File";
            save.Filter = "GPS Log files|*.log";
            save.FileName = trackData.FileName.TrimEnd('.', 'l', 'o', 'g') + "-Filtered.log";
            if (save.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(save.FileName))
                {
                    //don't allow overwrite???
                }
                else
                {
                    TimeDistanceFilter filter = new TimeDistanceFilter(trackData.Track, Convert.ToDouble(deltaDistanceNum.Value), Convert.ToDouble(deltaTimeNum.Value));
                    filter.SaveTrack(save.FileName);
                }
                    
                // TODO: automatically load new data to the map?
            }
        }
    }
}
