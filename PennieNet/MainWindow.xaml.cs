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
        private CSVLogger csvLogger;
        private Stopwatch stopWatch;
        private long lastTime;
        private string batchName;

        
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
                csvLogger = new CSVLogger();
                stopWatch.Start();
                lastTime = stopWatch.ElapsedMilliseconds;
            }
        }

        private void _reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var refFrame = e.FrameReference.AcquireFrame();
            using(var frame = refFrame.BodyFrameReference.AcquireFrame())
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
                
                if(User != null)
                {
                    if (!csvLogger.IsRecording)
                    {
                        batchName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                        csvLogger.Start(batchName);
                    }else if( csvLogger.IsRecording && (stopWatch.ElapsedMilliseconds - lastTime) % 3000 == 0)
                    {
                        csvLogger.Stop(batchName +".csv");
                        lastTime = stopWatch.ElapsedMilliseconds;
                    }
                    UserId = User.TrackingId;
                    
                    //USER -> FOLLOW;
                    User.Follow(depthWidth, depthHeight);
                } else
                {
                    _hasUser = false;
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
    }
}
