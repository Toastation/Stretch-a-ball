# Stretch-a-ball

* Unity version used : 2019.2.10f1
* Check the dev branch for the latest update
* Scripts are located in Assets/Scripts

# Presentation

Welcome to the project Stretch-A-Ball !

Data representation is increasingly interested in new technologies AR and VR.
New peripherals are created to interact between the man and the machine to help navigate and manipulate better a world in three dimensions.
In this project, the technology used is a Leap Motion Controller, a small device that uses cameras to detect hands and their motions.

This project follows on this trend mixing data visualization and 3d environments.
It has for purpose to visualize data points in a scene in Unity, and to manipulate them.
Their manipulation is made by using some highly complex Meshes.
Points can be selected by these Meshes whose shape can be changed for maximum accuracy and operations can be made between Meshes.

This project was made from October 2019 to April 2020.

# Instructions

These instructions are valid once the scene is played.
If you have not made it there, you can check the Installation part.
You will be welcome with all the points that were present in your csv file.
You can now open you left hand to find a menu, made of six buttons and a screen.

### Menu

The main page is composed of these options :
- Creation : in which you can instantiate a Mesh by pinching your thumb and your index in both hands.
- Selection : in which you have several new options, explained further down.
- Statistics & Save : in which statistics are displayed about the number of Meshes you have created and the number of boolean operations you have made so far and your work is saved
- Hide/Show : in which you can hide or display a coordinate system to help you get rour bearings better
- Help/Credits : in which you will find help "on the go" and credits
- Quit : to help you quit the gameview in Unity

The Selection page is composed of the following options :
- Modification : in which you can modify the shape of a Mesh by pinching somewhere above its surface
- Erase : in which you can delete a Mesh
- SetOperation : in which you have several new options to make operations between Meshes
- Return : that makes you return to the Main page in the menu

The SetOPeration page is composed of these options :
- SetIntersection : in which you can realize the operation "AND" between two Meshes
- SetUnion : in which you can realize the operation "OR" between two Meshes
- RelativeComplement : in which you can realize the operation "\" between two Meshes
- Confirm : with which you can confirm an operation
- Return : that makes you return to the Selection page in the menu

Once the scene is played, no further action from a keyboard is required.

### Creation

To create a Mesh, you first need to click on Creation.
You now need to pinch your right thumb with your right index, and do the same with your left hand. 
A new Mesh should appear, you can rescale it by increasing or decreasing the length between your hands.
You can also change the depth at which the Mesh was created, by alternating a left pinch and a right pinch and moving your hands like if you were pulling on a string.


### Boolean Operations

To select a Mesh either to delete it or for boolean operations, you must point your index at it (the use of both hands is valid).

To make a boolean operation, follow the following instructions :
1. First, go into the SetOPeration page.
2. Select a Mesh by pointing at it with one of your indexes
3. Click on the operation you wish to make
4. Select a second Mesh
5. Click on confirm
6. You can now go back to point 2 or point 3 if you wish to use the result of the last operation.
7. You can also return to do something else

### Save 

You can save your work by clicking on the button Statistics & Save. 
This will display statistics on the Meshes that are in the scene and the operations made so far.
This will also export CSV files int the ./Assets/Resources/ folder. 
One CSV file will be created for each Mesh and for each operation.
Be careful, as the data will be overwritten if you start the scene several times. 
You may want to copy your work in another folder before leaving Unity or before replaying the scene.

### Camera Motions

You can pivote the camera right or left by moving your hands to the edges of the screen.
You can also go forward or backward by moving your right hand in the same directions.

# Installation
1. Download the Leap Motion V4 SDK here : https://developer.leapmotion.com/setup/desktop
2. Download the StretchABall package or clone the project.
3. In Unity Hub, select "import a project" if you have cloned the project
3. In Unity Hub, start a new project and use Assets->Import Package->Custom Package... if you have the StretchABall package
4. Choose your CSV file in the gameObject ScatterPlot
5. Play the scene

# Credits

This project was made for AKKA RESEARCH and delivered in April 2020.

Special thanks to the developping team :
- Aubin Claire
- Chapal Victor
- Even Melvin
- Hertay Dylan
- Lanneau Quentin
- Trocherie Lucas

And for those who helped along the way :
- Herrero Victor
- Mazue Julien

# Links 
* [Manual for scripting with Unity](https://docs.unity3d.com/Manual/ScriptingSection.html)
* [Leap Motion Unity SDK ](https://developer.leapmotion.com/unity)
* [How to git with Unity](https://thoughtbot.com/blog/how-to-git-with-unity)
* [Packages to handle the Leap Motion in Unity](https://developer.leapmotion.com/unity#5436356)
