# Utils
Useful Unity C# scripts & packages  
/Scripts contains single C# scripts  
/Editor contains Editor scripts that must be put in an "Editor" folder in order to work  
/Packages contains unity packages  

# Scripts

## BitArray
Uses Bitwise Operators to manipulate arrays of 32 booleans with a single Integer.
Useful to manipulate massive amount of booleans with minimum memory, very useful for savefiles.

## SceneField
Allows scene manipulation as objects (drag and drop in inspector, create arrays, etc).

## SerializableTransform
A TransformSerial class containing a Vector3 position, Quaternion rotation and Vector3 localScale properties, all serializable.
Allows for quick serialization of transforms, useful for savefiles among other things.

# Editor

## GroupObjects
Quickly group objects under new empty Parents.  
Select your game objects in the Hierarchy, press Ctrl+G (or Right Click > Group Objects), enter the Parent's name and validate pressing enter.

# Packages

## FirstPersonPlayer
The most basic of First Person Player packages.

## IntVectors
Like Vectors but with Integers instead of floats. 
Contains IntVector2 and IntVector3 with the corresponding property drawers.

## RealTimeClock
A basic Canvas displaying HH:MM real time.

## Vignette
A better looking, more efficient and colourable vignetting effect. Looks good on any resolution since it does not use a texture.

## Radial Blur
A very simple Radial Blur image effect.
