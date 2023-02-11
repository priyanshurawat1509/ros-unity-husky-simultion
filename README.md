# ros-unity-husky-simultion
repository for unity off-road simulation integration with ROS

This repository includes codes used in the project of integrating Unity off-road simulation with Husky robot with ROS to visualize data inside Rviz for training RL models. 
- HuskyController.cs -> Publishes velocity commands from ROS to Husky (inside unity) using a no-slip kinematic model similar to the differential drive model of Husky controller in ROS.
- RGBCameraPublisher


All codes are in C#.
