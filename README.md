# MonoGizmo
A simple and esy to use 2D Gizmo library for MonoGame
<img src="https://github.com/CuvinStefanCristian/MonoGizmo/blob/master/Resources/Generic.png" />

## Features
For the moment only basic gizmo functionality is available but more functionality is expected in the future.

Currently implemented:
- 4 included Gizmos: `Origin`, `Translate`, `Rotate`, `Scale`
- 2 coordinate systems: `World`, `Local`
- Customize Gizmo size and color

### How it works
The Gizmos are controlled by a class names `GizmoManager`. Besides Updating and Rendering the Gizmos, this class also contains the Selection functionality of the entities that inherit the `ISelectable` interface.
The entities are checked for mouse clicks and intersections once they are added to the `Selectables` property of the manager. A QuadTree is used for optimizing the selection process.

The Gizmos can be turned On or Off by using the `ActivateType(GizmoType)` function from the GizmoManager.

### Included Gizmos
- `Origin` : Represents the center point of the

  WIP
