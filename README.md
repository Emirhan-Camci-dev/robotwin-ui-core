# RoboTwin-UI Core (Community Edition)

![License: AGPLv3](https://img.shields.io/badge/License-AGPLv3-blue.svg)

RoboTwin-UI Core is a high-performance Modular Desktop Robotics HMI and Teleoperation SDK for C# (.NET 8 Avalonia/WPF) and C++20. 
Designed for AMR manufacturers, robotic arm integrators, and AGV operators, it eliminates the engineering burden of building custom desktop teleoperation cockpits.

This is the **Community Edition (Open-Core)** licensed under AGPLv3.

## Features (Community Edition)
* **Single-Camera Video Viewer**: Low-latency video playback (MJPEG/RTSP).
* **2D Telemetry Dashboards**: ReactiveUI / MVVM controls for Avalonia and WPF.
* **Software Joystick Mapper**: Map generic controller inputs to simple twist vectors.
* **Zero-Allocation Data Streams**: Highly optimized C++ FFI interop using `Span<T>`.

## Enterprise Pro Edition
Looking for 60 FPS 3D Digital Twin URDF/glTF rendering, WebRTC ultra-low-latency video, hardware E-Stop watchdogs, and multi-robot fleet switching?

[![Buy License](https://img.shields.io/badge/Polar.sh-Buy_License-9cf.svg)](https://buy.polar.sh/polar_cl_GuAj2hfp9UW0F9vIBRkudt8dRc80dN4uv1ObQ1jYeHv)


## Quickstart

Add the SDK to your Avalonia `.NET 8` project and initialize the basic telemetry viewer:

```csharp
// 1. Initialize Video Stream & Gamepad
var videoFeed = new VideoStream("rtsp://robot.local/cam0");
var gamepad = GamepadManager.GetPrimary();

// 2. Bind zero-allocation Twist commands
gamepad.OnVelocityChanged += (v, w) => TelemetryBus.PublishTwist(v, w);

// 3. Mount UI in Avalonia Window
this.Content = new RoboDashboard { VideoSource = videoFeed };
```

---
**Author**: Emirhan CAMCI | **Email**: byemir@live.com | **Year**: 2026
