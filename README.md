# Dual Quaternion Pose Interpolation

A Unity demonstration comparing two methods of interpolating between
6-DoF rigid-body poses:

1. Position Lerp + Rotation Slerp
2. Dual-Quaternion Slerp

## Project Description

The demo visualizes the interpolation between an initial and final pose.
The two approaches are compared based on their resulting trajectories
and the coupling between translation and rotation.

## Key Observation

Position Lerp + Rotation Slerp interpolates translation and rotation
independently, producing a straight-line positional trajectory.

Dual-Quaternion Slerp treats the pose as a coupled rigid-body
transformation and produces a screw-like trajectory.

## Requirements

- Unity
- C#

## Running the Demo

Open the Unity project and load the demo scene from:

`Assets/Scenes/`

Press Play to view the interpolation.

## Author

RANS03
