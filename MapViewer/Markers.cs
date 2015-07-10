using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GMap.NET;
using GMap.NET.WindowsForms.Markers;

namespace MapViewer
{
    /// <summary>
    /// Form allows for editing the marker dsiplayed on the map
    /// </summary>
    public partial class Markers : Form
    {
        GMarkerGoogleType marker;

        public Markers(GMarkerGoogleType marker)
        {
            InitializeComponent();
            this.marker = marker;
        }

        private void Markers_Load(object sender, EventArgs e)
        {
            GMarkerGoogleType[] values = (GMarkerGoogleType[])Enum.GetValues(typeof(GMarkerGoogleType));
            int verticalSpacing = radioButton1.Height + 10;
            int horizontalSpacing = radioButton1.Width + 20;
            int columns = 3;
            RadioButton button = new RadioButton();

            //the first enum is omitted because it is "none" and won't work with OpenMap
            for(int item = 1; item < values.Length; item++)
            {
                int i = item - 1;
                button = new RadioButton();
                button.Text = values[item].ToString();
                button.Location = new Point(radioButton1.Location.X + i%columns * horizontalSpacing, radioButton1.Location.Y + verticalSpacing * (i/columns));
                button.Parent = this.groupBox1;
                button.Tag = values[item];
                if (values[i] == marker)
                    button.Select();
            }

            this.Width = columns * button.Width + 50;
            this.Height = (values.Length / columns + 1) * (button.Height + 10);

            button1.Location = new Point(radioButton1.Location.X + (columns - 1) * horizontalSpacing, radioButton1.Location.Y + verticalSpacing * ((values.Length-2) / columns + 1));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RadioButton button = groupBox1.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked);
            marker = (GMarkerGoogleType)button.Tag;
            this.Close();
        }

        public GMarkerGoogleType Marker
        {
            get { return marker; }
        }
    }
}
