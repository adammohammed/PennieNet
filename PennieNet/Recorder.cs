using System;
using Microsoft.Kinect;
using System.Diagnostics;

namespace PennieNet
{
    public class Recorder 
    {
        CSVLogger csvlog;
        Stopwatch watch;
        string batchName;
        int batchSize;
        public bool Enabled = true;

        public Recorder(int batch_interval)
        {
            csvlog = new CSVLogger();
            watch = new Stopwatch();
            batchSize = batch_interval; 
        }

        public void Update(Body b)
        {
            if (!Enabled) return;

            if (!csvlog.IsRecording)
            {
                batchName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                csvlog.Start(batchName);
                watch.Restart();
            }
            else if (csvlog.IsRecording && watch.Elapsed >= TimeSpan.FromSeconds(batchSize))
            {
                csvlog.Stop(batchName);
                watch.Stop(); 
            }
            else
            {
                csvlog.Update(b);
            }
        }
    }
}
