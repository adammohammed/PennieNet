using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Kinect;
using System.IO;
using System.Linq;

namespace PennieNet
{
    public class CSVLogger
    {
        // Saves Kinects Data to CSV to Desktop 
        int _current = 0;

        bool _hasEnumeratedJoints = false;

        public bool IsRecording { get; protected set; }

        public string Folder { get; protected set; }

        public string Result { get; protected set; }
        public Stopwatch stopwatch;
        public long timestamp = 0;
        public Body bd;
        public int nodes = 0;
        public string CSVWriteDirectory;
        private string DesktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);


        public void Start(string date)
        {
            // This starts recording
            nodes = 0;
            IsRecording = true;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            Folder = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            
            Directory.CreateDirectory(Folder);

            CSVWriteDirectory = Path.Combine(DesktopFolder, "KinectData/user_" + date);
            Directory.CreateDirectory(CSVWriteDirectory);
        }

        public void Update(Body body)
        {
            // Call this each time you receive a new body frame in order to update CSV
            if (!IsRecording) return;
            if (body == null || !body.IsTracked) return;

            string path = Path.Combine(Folder, _current.ToString() + ".line");
            using (StreamWriter writer = new StreamWriter(path))
            {
                StringBuilder line = new StringBuilder();
                StringBuilder notTracked = new StringBuilder();
                StringBuilder labeller = new StringBuilder();


                if (!_hasEnumeratedJoints)
                {
                    bd = body;
                    line.Append("TimeStamp,");
                    foreach (var joint in body.Joints.Values)
                    {
                        line.Append(string.Format("{0}_X,{0}_Y,{0}_Z", joint.JointType.ToString()));
                        labeller.Append(string.Format("{0}", joint.JointType.ToString()));
                        if (joint.JointType.ToString() != JointType.ThumbRight.ToString())
                        {
                            line.Append(',');
                        }
                        nodes++;
                    }
                    line.Append("," + labeller);
                    line.AppendLine();

                    _hasEnumeratedJoints = true;
                }

                foreach (var joint in body.Joints.Values)
                {
                    line.Append(string.Format("{0},{1},{2},{3}", stopwatch.ElapsedMilliseconds, joint.Position.X, joint.Position.Y, joint.Position.Z));
                    if(joint.JointType != JointType.ThumbRight)
                    {
                        line.Append(',');
                    }
                }

                writer.Write(line);
            }
            _current++;
        }

        public void Stop(string outputFile = null)
        {
            // Consolidates teh multiple .line files created into a CSV File 

            IsRecording = false;
            _hasEnumeratedJoints = false;

            stopwatch.Stop();

            if (outputFile == null)
            {
                Result = DateTime.Now.ToString("yyy_MM_dd_HH_mm_ss") + ".csv";
            }
            else
            {
                Result = outputFile + ".csv";
            }

            Result = Path.Combine(CSVWriteDirectory, Result);

            using (StreamWriter writer = new StreamWriter(Result))
            {
                for (int index = 0; index < _current; index++)
                {
                    string path = Path.Combine(Folder, index.ToString() + ".line");

                    if (File.Exists(path))
                    {
                        string line = string.Empty;

                        using (StreamReader reader = new StreamReader(path))
                        {
                            line = reader.ReadToEnd();
                        }
                        writer.WriteLine(line);
                    }
                }
            }

            // Gets rid of the .line folder 
            Directory.Delete(Folder, true);
        } 
    }
}
