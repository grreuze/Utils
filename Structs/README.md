# Structs

## > Bool3
Like a Vector3 but with Bools instead of floats.  
Includes a Property Drawer to have all 3 bools in one line in the inspector.

##  > IntVectors
Like Vectors but with Integers instead of floats.  
Contains IntVector2 and IntVector3 with the corresponding property drawers.

##  > MinMax
A simple Vector2 with "min" and "max" values instead of "x" and "y".  
Includes a few handy functions such as clamping or lerping between Min and Max, as well as a property drawer.

##  > nVector
Easily converts floats, vectors, colours and arrays into one another. This allows implicit operations between these classes as well as specific declarations such as Vector3 myVector3 = new nVector(myVector2, 2).  
Does NOT include a property drawer, this struct is not meant for serialization, use only for easier exchanges between different vectors.
