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
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

using GPS_Application;

namespace MapViewer
{
    public partial class mainForm : Form
    {
        Dictionary<GpsTrack, GMapOverlay> overlays = new Dictionary<GpsTrack, GMapOverlay>();

        public mainForm()
        {
            InitializeComponent();
        }

        private void gMap_Load(object sender, EventArgs e)
        {
            gMap.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;

            //GMaps.Instance.Mode = AccessMode.ServerOnly;
            //GMaps.Instance.Mode = AccessMode.CacheOnly;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            gMap.Position = new PointLatLng(0, 0);
            gMap.OnMarkerClick += new MarkerClick(gMap_OnMarkerClick);

            //bug in GMap places point at wrong position if it is the first
            //GMapOverlay overlay = new GMapOverlay("Defauly bug fix");
            //overlay.Markers.Add(new GMarkerGoogle(new PointLatLng(0, 0), GMarkerGoogleType.yellow_pushpin));
            //gMap.Overlays.Add(overlay);
            //not working as fix
            
            gMap.Refresh();
        }

        void SelectTracks()
        {
            OpenFileDialog d = new OpenFileDialog();

            d.Title = "Open Log Files";
            d.Filter = "GPS Log files|*.log";
            d.Multiselect = true;
            if (d.ShowDialog() == DialogResult.OK)
            {
                LoadTracks(d.FileNames);
            }
        }

        private void LoadTracks(string[] fileNames)
        {
            GpsLogParser p;

            foreach (string file in fileNames)
            {
                p = new GpsLogParser(file);
                GpsTrack track = p.ReadGpsLog();
                TrackGridData data = new TrackGridData(track, file);
                AddTrackToGrid(data);
            }
        }

        void AddTrackToGrid(TrackGridData trackData)
        {
            DataGridViewRow row = (DataGridViewRow)tracksGridView.RowTemplate.Clone();
            row.CreateCells(tracksGridView);
           
            row.Cells[Display.Index].Value = false;
            row.Cells[displayFormat.Index].Value = "Edit";
            row.Cells[startTime.Index].Value = trackData.Track.StartDate;
            row.Cells[endTime.Index].Value = trackData.Track.EndDate;
            row.Cells[minLatitude.Index].Value = trackData.Track.MinLatitude;
            row.Cells[maxLatitude.Index].Value = trackData.Track.MaxLatitude;
            row.Cells[minLongitude.Index].Value = trackData.Track.MinLongitude;
            row.Cells[maxLongitude.Index].Value = trackData.Track.MaxLongitude;

            row.Tag = trackData;
 
            tracksGridView.Rows.Add(row); //false, track.StartDate, track.EndDate, track.MinLatitude, track.MaxLatitude, track.MinLongitude, track.MaxLongitude);
            UpdateGridSize();
        }

        void UpdateGridSize()
        {
            int tracksGridViewHeadderHeight = 40;
            tracksGridView.Height = tracksGridView.Rows.Count * tracksGridView.Rows[0].Height + tracksGridViewHeadderHeight;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Need to decide on a different way and place for displaying this without too much crowding</remarks>
        void AddTrackPointsToGrid()
        {
            //foreach (GpsPoint point in track.Points)
            //    tracksGridView.Rows.Add(point.Time, point.Longitude.Value, point.Latitude.Value);
        }

        /// <summary>
        /// Adds an overlay of markers to the map.  Attempts to retreive the overlay data if it already exists
        /// otherwise the overlay is created and displayed on the map.
        /// </summary>
        /// <param name="trackData">Track data to be displayed</param>
        void AddMarkerOverlay(TrackGridData trackData)
        {
            GMapOverlay markersOverlay;
            if (!overlays.TryGetValue(trackData.Track, out markersOverlay))
            {
                markersOverlay = new GMapOverlay();

                foreach (GpsPoint point in trackData.Track.Points)
                    AddMarker(markersOverlay, point, trackData.Marker);

                overlays[trackData.Track] = markersOverlay;
            }

            gMap.Overlays.Add(markersOverlay);
            gMap.Refresh();

            //bug in GMap places new points at wrong position
            double zoom = gMap.Zoom;
            gMap.Zoom = zoom-1;
            gMap.Zoom = zoom;
        }

        void AddMarker(GMapOverlay markersOverlay, GpsPoint point, GMarkerGoogleType mark)
        {

            GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(point.Latitude.Value, point.Longitude.Value),
              mark);
            markersOverlay.Markers.Add(marker);

        }




        private void gMap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            object identityData = item.Tag;

            // load the menus with marker data.
            //command1.Tag = identityData;
            //command2.Tag = identityData;

            if (identityData != null && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
            //    markerMenu.Show(gmap, e.Location);
            }
        }

        private void gpsTracksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectTracks();
            tracksGridView.Visible = true;
        }

        /// <summary>
        /// Filtering can only be applied to 1 track at a time, so this disables that option in the menu as necessary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tracksGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (tracksGridView.SelectedRows.Count == 1)
            {
                filterToolStripMenuItem.Enabled = true;
                timeDistanceFilterToolStripMenuItem.Enabled = true;
            }
            else
            {
                filterToolStripMenuItem.Enabled = false;
                timeDistanceFilterToolStripMenuItem.Enabled = false;
            }
        }

        private void timeDistanceFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TimeDistanceFilterForm f = new TimeDistanceFilterForm((TrackGridData)tracksGridView.SelectedRows[0].Tag);
            var result = f.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Helps prevent rendering problems with large data sets and over eager clicking by a user</remarks>
        private void tracksGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // End of edition on each click on column of checkbox
            if (e.ColumnIndex == Display.Index && e.RowIndex != -1)
            {
                tracksGridView.EndEdit();
            }
        }

        /// <summary>
        /// Currently the only cell value that the user can modify is the check box that determines if the track data is displayed on the map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tracksGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == Display.Index && e.RowIndex != -1)
            {
                DataGridViewRow row = tracksGridView.Rows[e.RowIndex];
                TrackGridData trackData = (TrackGridData)row.Tag;
                
                if ((bool)row.Cells[Display.Index].Value)
                    AddMarkerOverlay(trackData);
                else
                    gMap.Overlays.Remove(overlays[trackData.Track]);
            }

            gMap.Refresh();
        }

        /// <summary>
        /// Currently only handles clicks to the "Format Edit" button for changing the parker image of a track.  
        /// Launches a new window for selecting the marker type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tracksGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                TrackGridData data = (TrackGridData)senderGrid.Rows[e.RowIndex].Tag;
                using (var f = new Markers(data.Marker))
                {
                    var result = f.ShowDialog();
                    if (result == DialogResult.OK && f.Marker != data.Marker)
                    {
                        TrackGridData newData = new TrackGridData(data.Track, data.FileName);
                        newData.Marker = f.Marker;
                        senderGrid.Rows[e.RowIndex].Tag = newData;

                        UpdateTrackOverlay(newData);
                    }
                }
            }
        }

        /// <summary>
        /// Modifies existing overlays with new data (currently only in the form of a different marker.
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>Read-write settings for the map data means that if the data is currently displayed on the
        /// map the only way to modify it is to delete it and make a new overlay with updated data.</remarks>
        void UpdateTrackOverlay(TrackGridData data)
        {
            GMapOverlay overlay;
            if (overlays.TryGetValue(data.Track, out overlay))
            {
                // a replacement overlay is not generated until it is needed for display.
                overlays.Remove(data.Track);

                // if the data exists as an overlay on the map it means it is currently being displayed and will need to be redrawn 
                if (gMap.Overlays.Contains(overlay))
                {
                    gMap.Overlays.Remove(overlay);
                    AddMarkerOverlay(data);
                }
            }
        }
    }

    public struct TrackGridData
    {
        GpsTrack track;
        public GMarkerGoogleType Marker;
        string fileName;

        public TrackGridData(GpsTrack track, string file)
        {
            this.track = track;
            this.fileName = file;
            this.Marker = GMarkerGoogleType.blue_small;
        }

        //public GMarkerGoogleType Marker
        //{
        //    get { return marker; }
        //    set { this.marker = value; }
        //}

        public GpsTrack Track
        { get { return this.track; } }

        public string FileName
        { get { return this.fileName; } }
    }

    /// TODO: everything
    /// <summary>
    /// Merge the checkbox and button into one cell
    /// </summary>
    class TrackGridDisplayColumn : DataGridViewColumn
    {
        CheckBox box;
        Button button;

        public TrackGridDisplayColumn()
        {
            box = new CheckBox();
            box.Checked = false;

            button = new Button();
            button.Text = "...";
        }
    }
}
