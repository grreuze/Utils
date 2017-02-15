# Utils
Useful Unity C# scripts & packages  
/Scripts contains single C# scripts  
/Editor contains Editor scripts that must be put in an "Editor" folder in order to work  
/Image Effects contains camera post processing effects compressed in unity packages  
/Packages contains unity packages  

# Editor

##  > GroupObjects
Quickly group objects under new empty Parents.  
Select your game objects in the Hierarchy, press Ctrl+G (or Right Click > Group Objects), enter the Parent's name and validate pressing enter.

# Image Effects

##  > Gradient Fog
A fog applying a fully editable gradient from the camera to the distance.

##  > Radial Blur
A very simple Radial Blur image effect.

##  > Vignette
A better looking, more efficient and colourable vignetting effect. Looks good on any resolution since it does not use a texture.


# Packages

## > Bool3
Like a Vector3 but with Bools instead of floats.
Includes a Property Drawer to have all 3 bools in one line in the inspector.

##  > FirstPersonPlayer
The most basic of First Person Player packages (good for quick prototypes).

##  > IntVectors
Like Vectors but with Integers instead of floats.  
Contains IntVector2 and IntVector3 with the corresponding property drawers.

##  > MinMax
A simple Vector2 with "min" and "max" values instead of "x" and "y".  
Includes a few handy functions such as clamping or lerping between Min and Max, as well as a property drawer.

##  > RealTimeClock
A basic Canvas displaying HH:MM real time.

##  > Title Screen
A default starting screen with "start" and "exit" buttons. Works with mouse, keyboard and gamepad seamlessly.

# Scripts

##  > BitArray
Uses Bitwise Operators to manipulate arrays of 32 booleans with a single Integer.
Useful to manipulate massive amount of booleans with minimum memory, very useful for savefiles.

##  > BlinkingText
A simple script to put on a UI text element for it to blink.

##  > ClampPosition
Clamps the position of a transform between the specified Vector3 coordinates. 

##  > FollowTarget
Super simple script binding an object to another one. Better than parenting in some cases, worse in others.

##  > LocalTrailRenderer
A Trail Renderer that works in local space.

##  > SceneFeild
Serializable unity scene files class, to allow them being drag'n'dropped in the inspector.

##  > SerializableTransform
A TransformSerial class containing a Vector3 position, Quaternion rotation and Vector3 localScale properties, all serializable.
Allows for quick serialization of transforms, useful for savefiles among other things.

##  > StandardShaderRenderMode
Allows changing Unity's Standard Shader's Rendering Mode at Runtime.
Syntax is material.SetRenderingMode(RenderingMode).
