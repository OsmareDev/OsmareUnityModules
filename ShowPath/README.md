# Show Path
![a_12_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/6f84af51-b950-41be-b53d-8a1dce12d817)
![a_12_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/16d5dbde-50ae-4f4c-92c7-072f4c3ef88e)


# English

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>Unity has a tool to find the path between two points, allowing obstacles to be added to this calculation, but it does not have a way to display the result.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>We will create a PathController script that will be responsible for collecting the calculation made by Unity and showing it to the player.</td></tr>
</table>

We will use the TimedTask structure located in the Utils folder. Which is responsible for obtaining a function and executing it according to the selected seconds. Being able to activate or deactivate said process at will.

We will use Unity's [NavMesh](https://docs.unity3d.com/ScriptReference/AI.NavMesh.html). We implement a function that obtains the points that form the corners of the path. Inside the function, we will check that the destination list is not empty. If it is, we will stop the function. Once the closest destination has been calculated, we will ask [NavMesh](https://docs.unity3d.com/ScriptReference/AI.NavMesh.html) to calculate the path ([NavMeshPath](https://docs.unity3d.com /ScriptReference/AI.NavMeshPath.html)) using the [CalculatePath](https://docs.unity3d.com/ScriptReference/AI.NavMesh.CalculatePath.html) function.

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>In the case of 2D, unity pathfinding does not work.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>We will allow the user to select another script that contains the function with a start point and an end point of the path to be displayed. We will allow it to be done for 3D as well.</td></tr>
</table>

We will use the [LineRenderer](https://docs.unity3d.com/Manual/class-LineRenderer.html) component to display the path. Additionally, we will use the [System.Linq](https://learn.microsoft.com/es-es/dotnet/api/system.linq?view=net-7.0) library to reorganize the lists so that we can determine if there are destinations in them and which of them is the closest.

# Español

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>Unity posee una herramienta para averiguar el camino entre dos puntos, permitiendo añadir obstáculos a este cálculo, pero no tiene una manera de enseñar el resultado.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Crearemos un script PathController que se encargará de recoger el cálculo realizado por Unity y mostrarlo al jugador.</td></tr>
</table>

Usaremos la estructura TimedTask localizada en la carpeta de Utils. La cual se encarga de obtener una función y ejecutarla conforme a los segundos seleccionados. Pudiendo activar o desactivar dicho proceso a placer.

Utilizaremos el [NavMesh](https://docs.unity3d.com/ScriptReference/AI.NavMesh.html) de Unity. Implementamos una función que obtiene los puntos que forman las esquinas del camino. Dentro de la función, comprobaremos que la lista de destinos no esté vacía. En caso de que lo esté, pararemos la función. Una vez calculado el destino más cercano, solicitaremos al [NavMesh](https://docs.unity3d.com/ScriptReference/AI.NavMesh.html) que calcule el camino ([NavMeshPath](https://docs.unity3d.com/ScriptReference/AI.NavMeshPath.html)) utilizando la función [CalculatePath](https://docs.unity3d.com/ScriptReference/AI.NavMesh.CalculatePath.html).

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>En el caso de ser 2D el pathfinding de unity no funciona.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Dejaremos que el usuario pueda seleccionar otro script que contenga la función que con un punto de inicio y un punto final de el camino a mostrar. Dejaremos que se pueda hacer también para 3D.</td></tr>
</table>

Utilizaremos el componente [LineRenderer](https://docs.unity3d.com/Manual/class-LineRenderer.html) para visualizar la ruta. Además, utilizaremos la biblioteca [System.Linq](https://learn.microsoft.com/es-es/dotnet/api/system.linq?view=net-7.0) para reorganizar las listas de modo que podamos determinar si hay destinos en ellas y cuál de ellos es el más cercano.
