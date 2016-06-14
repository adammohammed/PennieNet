using System;
using System.Collections.Generic;
using Microsoft.Kinect;

namespace PennieNet
{
    public static class BodyExtensions
    {
        public static float stayFarRange = 1300.0f;
        public static float stayCloseRange = 1000.0f;
        public static float minTurnAngle = (float) Math.Abs(15.0 * 3.14 / 180.0);
        public static string cmd = "";

        public static void Follow(this Body b, float cameraWidth, float cameraHeight)
        {
            IReadOnlyDictionary<JointType, Joint> joints = b.Joints;

            Joint Head = joints[JointType.Head];
            Joint Chest = joints[JointType.SpineShoulder];

            if (Head.TrackingState != TrackingState.Tracked || Chest.TrackingState != TrackingState.Tracked)
            {
                return;
            }

            // Nodes Z Cant be smaller than 0 
            if(Head.Position.Z < 0 || Chest.Position.Z < 0)
            {
                return; 
            }


            float headPos = Head.Position.Z * 1000.0f;
            float chestPos = Chest.Position.Z * 1000.0f;
            float headX = Head.Position.X * 1000.0f;

            float angle = Math.Abs(headX / headPos);
            float avgZ = (headPos + chestPos) / 2.0f;

            if(angle > minTurnAngle)
            {
                if(headX-(cameraWidth)/2.0 > 0)
                {
                    cmd = "left";
                }else
                {
                    cmd = "right";
                }

            }
            else if (avgZ >= stayCloseRange && avgZ <= stayFarRange)
            {
                cmd = "stay";
            }
            else if (avgZ > stayFarRange)
            {
                cmd = "fwd";
            }
            else if (avgZ < stayCloseRange)
            {
                cmd = "rev";
            }
            else
            {
                cmd = "stay";
            }
            
        }
    }
}
