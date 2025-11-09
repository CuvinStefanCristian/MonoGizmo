# MonoGizmo
A simple and esy to use 2D Gizmo library for MonoGame. The package is build on `netstandard2.1` and the graphics have been created using the amazing library [Apos.Shapes](https://github.com/Apostolique/Apos.Shapes)

Now with [MonoGame.Forms](https://github.com/BlizzCrafter/MonoGame.Forms) support ! (check version 1.0.1)

[Nuget](https://www.nuget.org/packages/MonoGizmo/1.0.1)

![Demo](https://github.com/CuvinStefanCristian/MonoGizmo/blob/master/Resources/MonoGizmo-Demo.gif)

### NEWS
A majour overhaul will take place soon !

## Features
For the moment only basic gizmo functionality is available but more functionality is expected in the future.

Currently implemented:
- 4 included Gizmos: `Origin`, `Translate`, `Rotate`, `Scale`
- 2 coordinate systems: `World`, `Local`
- Customize Gizmo size and color

Future ideeas:
- Grid System (guides / snap to grid)
- Step Transforms (how much things should move! currently the mouse dictates the ammount of movement, not very precises is it..)

### IMPORTANT
You must use **HiDef** as your GraphicsProfile for DirectX projects! 

### How it works
The Gizmos are controlled by a class names `GizmoManager`. Besides Updating and Rendering the Gizmos, this class also contains the Selection functionality of the entities that inherit the `ISelectable` interface.
The entities are checked for mouse clicks and intersections once they are added to the `Selectables` property of the manager. A QuadTree is used for optimizing the selection process.

The Gizmos can be turned On or Off by using the `ActivateType(GizmoType)` function from the GizmoManager. The Selection can also be cleared from here.

Check out the [MonoGizmo.tests](https://github.com/CuvinStefanCristian/MonoGizmo/tree/master/MonoGizmo.Tests) project for an installation and utilization example.

### ISelectable
One of the key features of this library is its flexibility regarding the type of bounds that a selectable object can implement. The library does not impose any specific type for the bounds, allowing users to define and use their own custom bounds types. This design choice ensures that users are not limited by the library's implementation and can integrate their own logic and structures seamlessly.

In the example project, the `Selectable` class implements the `ISelectable` and `IEntity` interfaces, which define properties and methods for handling bounds, scaling, translation, and rotation. The `Center` property allows for easy manipulation of the object's center point, while the `IsSelected` function provides a bounds specific way of determining if the mouse clicked inside a selectable object. However, the actual type of the bounds is not restricted, enabling users to implement their own bounds logic / selection as needed.

This flexibility allows for a wide range of use cases and customizations, making the library adaptable to various project requirements.

Check how this was implemented in the example project.

### Included Gizmos
- `Origin` : Represents the center point of the `ISelectable` entity. Dragging this Gizmo will cause the selected entity to follow
<img src="https://github.com/CuvinStefanCristian/MonoGizmo/blob/master/Resources/Origin.png" />

- `Translate`: Changes the position of the entity on the `X` and `Y` axis. This is achieved by grabbing the Arrows
<img src="https://github.com/CuvinStefanCristian/MonoGizmo/blob/master/Resources/Translate.png" />

- `Roate` : Rotates the selected entity around it's center / origin (the origin doesn't need to be active). This is done by dragging the ring in the desired rotation direction
<img src="https://github.com/CuvinStefanCristian/MonoGizmo/blob/master/Resources/Rotate.png" />

- `Scale` : This Gizmo modifies the `Width` and `Height` of the selected entity
<img src="https://github.com/CuvinStefanCristian/MonoGizmo/blob/master/Resources/Scale.png" />

### World vs Local coordinates
- `World` These are absolute coordinates of where the game object is located (by absolute, I mean with respect to the world frame, which is considered to be absolute in the game)
<img src="https://github.com/CuvinStefanCristian/MonoGizmo/blob/master/Resources/WorldCoordinates.png" />

- `Local` These coordinates take into accdount the orientation (rotation) of the object
<img src="https://github.com/CuvinStefanCristian/MonoGizmo/blob/master/Resources/LocalCoordinates.png" />
Notice how the Gizmos have now changed their orientation to match the rotation of the object

### Need help ?
Contact me on Discord and we'll try to figure things out ! (no promises though) username:  ATLANTISorg
