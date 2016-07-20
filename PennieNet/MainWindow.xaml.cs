using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Kinect;
using System.Diagnostics;

namespace PennieNet
{
    public partial class MainWindow : Window
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> bodies;
        //CoordinateMapper cm; // For later use if I need it
        private bool setup = false;
        private bool _hasUser = false;
        private Body User;
        private ulong UserId;
        FrameDescription fd;
        private float depthWidth;
        private float depthHeight;
        CSVLogger csvLogger;
        private string batchName;
        //Recorder csvRecorder;

        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();
            if(_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body | FrameSourceTypes.Depth);
                _reader.MultiSourceFrameArrived += _reader_MultiSourceFrameArrived;

                fd = _sensor.DepthFrameSource.FrameDescription;
                depthWidth = fd.Width;
                depthHeight = fd.Height;
                if (!setup)
                { 
                    //this.CreateBones();
                }
                //csvRecorder = new Recorder(3);
                //csvRecorder.Enabled = true;
                csvLogger = new CSVLogger();

                this.KeyDown += MainWindow_KeyDown;
            }
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.S)
            {
                csvLogger.Stop(batchName);
            }
        }

        private void _reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var refFrame = e.FrameReference.AcquireFrame();
            using (var frame = refFrame.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    bodies = new Body[frame.BodyCount];
                    frame.GetAndRefreshBodyData(bodies);

                    if (!_hasUser)
                    {
                        User = (from bd in bodies where bd.IsTracked select bd).FirstOrDefault();
                        _hasUser = true;
                    }
                    else
                    {
                        User = (from bd in bodies where bd.IsTracked && bd.TrackingId == UserId select bd).FirstOrDefault();
                    }

                    if (User != null)
                    {

                        UserId = User.TrackingId;

                        //USER -> FOLLOW;
                        User.Follow(depthWidth, depthHeight);

                        // Record Data
                        if (!csvLogger.IsRecording)
                        {
                            batchName = DateTime.UtcNow.ToString("HH-mm-ss");
                            csvLogger.Start(batchName);
                        }
                        else 
                        {
                            csvLogger.Update(User);
                        }
                        //csvRecorder.Update(User);
                        // Update Text 
                        this.commandStatus.Text = BodyExtensions.cmd;
                    }
                    else
                    {
                        _hasUser = false;
                        if(BodyExtensions.commander != null) {
                            BodyExtensions.commander.IssueCmd("stop");
                        }
                    }
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if(_sensor != null)
            {
                _sensor.Close();
                _sensor = null;
            }

            if(_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }

            if(BodyExtensions.commander != null)
            {
                BodyExtensions.commander.Dispose();
                BodyExtensions.commander = null;
            }
        }

        private void ipSet_Clicked(object sender, RoutedEventArgs e)
        {
            BodyExtensions.commander = null;
            //BodyExtensions.commander = new RobotApi(this.hostaddr.ToString(), int.Parse(this.hostport.Text));

        }
    }
}
