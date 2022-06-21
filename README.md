# rubiks-cube

This is an Android application that emulates a rubik's cube. You can edit the rubik's cube or register as a user to save its state in the database. Otherwise you can try to solve it yourself.

The Rubik's Cube was built as a 3x3x3 array of small cubies, each possessing a position vector representing their position in 3d space and a color vector representing how the different axes of the cubie are colored.
For the rotation of the cube I've multiplied the selected cubies' position vector with a rotation matrix to calculate the new position and color vectors.

Frameworks and languages:
* C#
* Xamarin Android
* ASP.NET
* SQL

Technologies I've used:
* 3d array of objects
* Linear algebra
* OOP
