# Interest ScreenPoint
![a_13_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/8dbf9134-9965-4dac-91b6-f93535719cb1)
![a_13_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/399aa2ed-d84a-4418-a819-e8852019c312)


# English

In order to provide the player with visual information about points of interest, we will implement a manager that will display in the interface the closest point of interest within a radius defined by the developer.

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>Since the player's camera will be constantly moving, the location of the mark on the screen will quickly become unusable.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>We will use an asynchronous function that updates the position of the object on the screen</td></tr>
</table>

First we will implement an asynchronous function instead of using the [Update](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html) function. This will allow us to avoid unnecessary consumption of resources when we do not want to show the marks on the screen.

We will obtain a list of the points of interest present in the scene and, using the library [System.Linq](https://learn.microsoft.com/es-es/dotnet/api/system.linq?view=net-7.0 ), we will determine if there are any targets within the designated radius. If there is at least one, we will select the closest point of interest.

Now, we will check if there is any obstacle between the viewer and the point of interest using a [Raycast](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html). If the [Raycast](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html) collides with an object, we will verify if it is the intended target. If so, we will try to obtain the IInteractable interface to allow the player to interact with said object.

Now that we've confirmed that this is the correct element, we'll use the [WorldToScreenPoint](https://docs.unity3d.com/ScriptReference/Camera.WorldToScreenPoint.html) function to convert the world point to canvas space, and then use the function [RectangleContainsScreenPoint](https://docs.unity3d.com/ScriptReference/RectTransformUtility.RectangleContainsScreenPoint.html) to determine if said point is within the canvas area. If it is inside the canvas, we will set the calculated position for the mark.

Finally, we will implement a function that will call the interaction function if the point of interest is visible on the screen, the asynchronous function is active, and a reference to an object that implements the IInteractable interface has been stored.

# Español

Con el fin de proporcionar al jugador información visual sobre los puntos de interés, implementaremos un gestor que se encargará de mostrar en la interfaz el punto de interés más cercano dentro de un radio definido por el desarrollador.

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>Dado que la cámara del jugador se encontrará en constante movimiento, la ubicación de la marca en la pantalla se verá inutilizada rápidamente.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Haremos uso de una función asíncrona que actualiza la posición del objeto en pantalla</td></tr>
</table>

Primero implementaremos una función asincrónica en vez de usar la función [Update](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html). Esto nos permitirá evitar el consumo innecesario de recursos cuando no queramos mostrar las marcas en la pantalla.

Obtendremos una lista de los puntos de interés presentes en la escena y, utilizando la librería [System.Linq](https://learn.microsoft.com/es-es/dotnet/api/system.linq?view=net-7.0), determinaremos si existe algún objetivo dentro del radio designado. En caso de que exista uno como mínimo, seleccionaremos el punto de interés más cercano.

Ahora, comprobaremos si existe algún obstáculo entre el visualizador y el punto de interés mediante un [Raycast](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html). Si el [Raycast](https://docs.unity3d.com/ScriptReference/Physics.Raycast.html) colisiona con algún objeto, verificaremos si se trata del objetivo deseado. De ser así, intentaremos obtener la interfaz IInteractable para permitir que el jugador interactúe con dicho objeto.

Ahora que hemos confirmado que se trata del elemento correcto, utilizaremos la función [WorldToScreenPoint](https://docs.unity3d.com/ScriptReference/Camera.WorldToScreenPoint.html) para convertir el punto del mundo al espacio del canvas, y luego emplearemos la función [RectangleContainsScreenPoint](https://docs.unity3d.com/ScriptReference/RectTransformUtility.RectangleContainsScreenPoint.html) para determinar si dicho punto se encuentra dentro del área del canvas. Si está dentro del canvas, estableceremos la posición calculada para la marca.

Por último, implementaremos una función que llamará a la función de interacción si el punto de interés es visible en pantalla, la función asincrónica está activa y se ha almacenado una referencia a un objeto que implementa la interfaz IInteractable.
