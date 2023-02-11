using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.UrdfImporter.Control;

namespace RosSharp.RosBridgeClients
{
    public class HuskyController_ : MonoBehaviour
    {
        public float w_radius = 0.3f;
        public float w_distance = 1.0f; //0.230f;
        public float motortorque;

        public WheelCollider WheelFrontLeftCollider;
        public WheelCollider WheelFrontRightCollider;
        public WheelCollider WheelRearLeftCollider;
        public WheelCollider WheelRearRightCollider;

        public Transform WheelFrontLeftTransform;
        public Transform WheelFrontRightTransform;
        public Transform WheelRearLeftTransform;
        public Transform WheelRearRightTransform;

        private float vel_left;
        private float vel_right;
        private float roslinear = 0f;
        private float rosangular = 0f;
        private bool isMessagerecieved = false;
        ROSConnection ros;

        // Start is called before the first frame update
        void Start()
        {   
            ros = ROSConnection.GetOrCreateInstance();
            ros.Subscribe<TwistMsg>("cmd_vel", ROSCommand);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isMessagerecieved)
                MoveHusky();
        }

        void ROSCommand(TwistMsg ros_cmd_vel)
        {
            roslinear = (float)ros_cmd_vel.linear.x;
            rosangular = (float)ros_cmd_vel.angular.z;
            vel_right = 180/Mathf.PI*((2*roslinear + rosangular*w_distance)/(2*w_radius));
            vel_left = 180/Mathf.PI*((2*roslinear - rosangular*w_distance)/(2*w_radius));
            isMessagerecieved = true;
        }

        void MoveHusky()
        {
            // updating wheel velocity
            WheelFrontLeftCollider.motorTorque = vel_left/2;
            WheelFrontRightCollider.motorTorque = vel_right/2;
            WheelRearLeftCollider.motorTorque = vel_left/2;
            WheelRearRightCollider.motorTorque = vel_right/2;
            
            // updating wheel transform
            WheelFrontLeftTransform.Rotate(0, WheelFrontLeftCollider.rpm, 0);
            WheelFrontRightTransform.Rotate(0, WheelRearRightCollider.rpm, 0);
            WheelRearLeftTransform.Rotate(0, WheelRearRightCollider.rpm, 0);
            WheelRearRightTransform.Rotate(0, WheelRearRightCollider.rpm, 0);
            
            isMessagerecieved = false;
        }
    }
}