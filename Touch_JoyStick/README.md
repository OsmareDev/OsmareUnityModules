# AutoScroll Background
![a5_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/2e39a75b-c64a-48ff-a963-5a3feceebb09)


# English

On mobile devices, players do not have traditional controls and must use screen-tap based controls. One way commonly used by developers is to create virtual buttons or joysticks, which are placed on the screen and detect when that specific area is pressed.

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>Unity provides several tools to detect screen taps. However, the interpretation and management of these keystrokes is largely up to the developer.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>A module called <b>JoyStickController</b> is implemented to detect and interpret the player's actions when clicking and dragging on a specific area of the screen.</td></tr>
</table>

We will manage the control through 3 phases:

- ### Start of pressure:
We will use the function [OnPointerDown](https://docs.unity3d.com/es/530/ScriptReference/UI.Selectable.OnPointerDown.html) to detect the player's pressure point on the screen. By making use of this feature, we ensure that it is only activated within the designated canvas.

We record the position of the pressure made by the player and we will place the images corresponding to the joystick in that same position.

- ### Drag:
We will implement the function [OnDrag](https://docs.unity3d.com/es/530/ScriptReference/EventSystems.IDragHandler.OnDrag.html) to detect the movement of the player's finger on the screen. Here we will store a second position that will correspond to the current location of the finger. By subtracting this new position from the initial position of the pressure and setting the magnitude of the maximum vector to 1, we can obtain the direction of movement.

Finally, we will update the internal image of the joystick to generate the sensation of movement. The position of this image will be adjusted within the limits set by the developer for maximum joystick travel.

- ### End of pressure:
We will implement the function [OnPointerUp](https://docs.unity3d.com/es/530/ScriptReference/EventSystems.IPointerUpHandler.html) to detect when the player lifts his finger from the screen. Here we will override the direction of movement and remove the images from the screen.

To get the direction information, the GetValue function of the script is used.

# Español

En dispositivos móviles, los jugadores no cuentan con controles tradicionales y deben utilizar controles basados en pulsaciones de pantalla. Una forma comúnmente utilizada por los desarrolladores es la creación de botones o joysticks virtuales, que se colocan en la pantalla y detectan cuando se pulsa esa área específica.

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>Unity proporciona diversas herramientas para detectar las pulsaciones en pantalla. Sin embargo, la interpretación y gestión de estas pulsaciones depende en gran medida del desarrollador.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Se implementa un módulo denominado <b>JoyStickController</b> para detectar e interpretar las acciones del jugador al pulsar y arrastrar sobre una zona específica de la pantalla.</td></tr>
</table>

Gestionaremos el control a través de 3 fases:

- ### Comienzo de la presión:
Utilizaremos la función [OnPointerDown](https://docs.unity3d.com/es/530/ScriptReference/UI.Selectable.OnPointerDown.html) para detectar el punto de presión del jugador en la pantalla. Al hacer uso de esta función, nos aseguramos de que solo se active dentro del canvas designado.

Registramos la posición de la presión realizada por el jugador y colocaremos las imágenes correspondientes al joystick en esa misma posición.

- ### Arrastrar:
Implementaremos la función [OnDrag](https://docs.unity3d.com/es/530/ScriptReference/EventSystems.IDragHandler.OnDrag.html) para detectar el desplazamiento del dedo del jugador en la pantalla. Aquí almacenaremos una segunda posición que corresponderá a la ubicación actual del dedo. Restando esta nueva posición con la posición inicial de la presión y poniendo como magnitud del vector máxima 1, podremos obtener la dirección del movimiento.

Por último, actualizaremos la imagen interna del joystick para generar la sensación de movimiento. La posición de esta imagen se ajustará dentro de los límites puestos por el desarrollador para el máximo desplazamiento del joystick.

- ### Fin de la presión:
Implementaremos la función [OnPointerUp](https://docs.unity3d.com/es/530/ScriptReference/EventSystems.IPointerUpHandler.html) para detectar cuando el jugador levante el dedo de la pantalla. Aquí anularemos la dirección de movimiento y quitaremos las imágenes de la pantalla.

Para sacar la información de la dirección se usa la funcion GetValue del script.
