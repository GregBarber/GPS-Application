namespace MapViewer
{
    partial class mainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tracksGridView = new System.Windows.Forms.DataGridView();
            this.Display = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.displayFormat = new System.Windows.Forms.DataGridViewButtonColumn();
            this.startTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minLatitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxLatitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minLongitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxLongitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gpsTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeDistanceFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gMap = new GMap.NET.WindowsForms.GMapControl();
            ((System.ComponentModel.ISupportInitialize)(this.tracksGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 28);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(860, 439);
            this.webBrowser1.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 28);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(230, 439);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            this.splitter1.Visible = false;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(32, 61);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(121, 97);
            this.treeView1.TabIndex = 6;
            // 
            // tracksGridView
            // 
            this.tracksGridView.AllowUserToAddRows = false;
            this.tracksGridView.AllowUserToDeleteRows = false;
            this.tracksGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tracksGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Display,
            this.displayFormat,
            this.startTime,
            this.endTime,
            this.minLatitude,
            this.maxLatitude,
            this.minLongitude,
            this.maxLongitude});
            this.tracksGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tracksGridView.Location = new System.Drawing.Point(230, 317);
            this.tracksGridView.Name = "tracksGridView";
            this.tracksGridView.RowTemplate.Height = 24;
            this.tracksGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tracksGridView.Size = new System.Drawing.Size(630, 150);
            this.tracksGridView.TabIndex = 7;
            this.tracksGridView.Visible = false;
            this.tracksGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tracksGridView_CellContentClick);
            this.tracksGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tracksGridView_CellMouseUp);
            this.tracksGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.tracksGridView_CellValueChanged);
            this.tracksGridView.SelectionChanged += new System.EventHandler(this.tracksGridView_SelectionChanged);
            // 
            // Display
            // 
            this.Display.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Display.HeaderText = "Display";
            this.Display.Name = "Display";
            this.Display.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Display.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Display.Width = 79;
            // 
            // displayFormat
            // 
            this.displayFormat.HeaderText = "Format";
            this.displayFormat.Name = "displayFormat";
            this.displayFormat.Text = "...";
            // 
            // startTime
            // 
            this.startTime.HeaderText = "Start Time";
            this.startTime.Name = "startTime";
            this.startTime.ReadOnly = true;
            // 
            // endTime
            // 
            this.endTime.HeaderText = "End Time";
            this.endTime.Name = "endTime";
            // 
            // minLatitude
            // 
            this.minLatitude.HeaderText = "Min Latitude";
            this.minLatitude.Name = "minLatitude";
            this.minLatitude.ReadOnly = true;
            // 
            // maxLatitude
            // 
            this.maxLatitude.HeaderText = "Max Latitude";
            this.maxLatitude.Name = "maxLatitude";
            // 
            // minLongitude
            // 
            this.minLongitude.HeaderText = "Min Longitude";
            this.minLongitude.Name = "minLongitude";
            this.minLongitude.ReadOnly = true;
            // 
            // maxLongitude
            // 
            this.maxLongitude.HeaderText = "Max Longitude";
            this.maxLongitude.Name = "maxLongitude";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.filterToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(860, 28);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gpsTracksToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(54, 24);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // gpsTracksToolStripMenuItem
            // 
            this.gpsTracksToolStripMenuItem.Name = "gpsTracksToolStripMenuItem";
            this.gpsTracksToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
            this.gpsTracksToolStripMenuItem.Text = "Gps Track(s)";
            this.gpsTracksToolStripMenuItem.Click += new System.EventHandler(this.gpsTracksToolStripMenuItem_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeDistanceFilterToolStripMenuItem});
            this.filterToolStripMenuItem.Enabled = false;
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(54, 24);
            this.filterToolStripMenuItem.Text = "Filter";
            // 
            // timeDistanceFilterToolStripMenuItem
            // 
            this.timeDistanceFilterToolStripMenuItem.Enabled = false;
            this.timeDistanceFilterToolStripMenuItem.Name = "timeDistanceFilterToolStripMenuItem";
            this.timeDistanceFilterToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.timeDistanceFilterToolStripMenuItem.Text = "Time-Distance Filter";
            this.timeDistanceFilterToolStripMenuItem.Click += new System.EventHandler(this.timeDistanceFilterToolStripMenuItem_Click);
            // 
            // gMap
            // 
            this.gMap.Bearing = 0F;
            this.gMap.CanDragMap = true;
            this.gMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMap.GrayScaleMode = false;
            this.gMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMap.LevelsKeepInMemmory = 5;
            this.gMap.Location = new System.Drawing.Point(230, 28);
            this.gMap.MarkersEnabled = true;
            this.gMap.MaxZoom = 20;
            this.gMap.MinZoom = 2;
            this.gMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMap.Name = "gMap";
            this.gMap.NegativeMode = false;
            this.gMap.PolygonsEnabled = true;
            this.gMap.RetryLoadTile = 0;
            this.gMap.RoutesEnabled = true;
            this.gMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMap.ShowTileGridLines = false;
            this.gMap.Size = new System.Drawing.Size(630, 289);
            this.gMap.TabIndex = 9;
            this.gMap.Zoom = 0D;
            this.gMap.Load += new System.EventHandler(this.gMap_Load);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 467);
            this.Controls.Add(this.gMap);
            this.Controls.Add(this.tracksGridView);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "mainForm";
            this.Text = "GPS Application - Map Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.tracksGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView tracksGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gpsTracksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private GMap.NET.WindowsForms.GMapControl gMap;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Display;
        private System.Windows.Forms.DataGridViewButtonColumn displayFormat;
        private System.Windows.Forms.DataGridViewTextBoxColumn startTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn endTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn minLatitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxLatitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn minLongitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxLongitude;
        private System.Windows.Forms.ToolStripMenuItem timeDistanceFilterToolStripMenuItem;
    }
}

