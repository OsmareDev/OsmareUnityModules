# Pathfinding 2D
![a8_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/b38c0b20-a7cc-4c0e-9f76-3e5d91ed74d4)
![a8_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/307755ed-8652-4a47-9c11-c1917518439b)

# English

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>When performing pathfinding Unity only includes options for 3D</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>We make our own pathfinding system, with visual editing from the editor to make it more accessible</td></tr>
</table>

To do this we will divide the work into 3 parts

- **Grid and nodes**
<br>
We will create a system of nodes to divide the terrain into a grid, which can be manipulated from the editor. We will use this grid system to feed the A* algorithm, which will determine the best path from the current point to the target. The script will return a list of Vector3 points that represent the path between the managed points in the FindPath function of the script.

<br>

To fill the grid, a collision detection system has been implemented for which the list of layers that represent the non-walkable squares must be entered.

---
- **Save system**
<br>
We will use the generic saving system provided in the Utils folder to save the grid information.

---
- **Editing from the editor**
<br>
From the editor you can manage the grid, its size and how many cells it has, the colors for its visual representation, the collision layers for automatic detection, the percentage of the cell that you want to check and the name of the grid. file that will be used to save the grid information.

<br>

Finally, the gizmos will be programmed in such a way that they are only shown when the object that contains the script is selected, this way it will not interfere with the development of the project. When you finish configuring the grid, press the editor button so that the script begins checking the cells.

# Español

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>A la hora de realizar un pathfinding Unity solo incorpora opciones para 3D</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Hacemos nuestro propio sistema de pathfinding, con edición visual desde el editor para hacerlo más accesible</td></tr>
</table>

Para ello dividiremos el trabajo en 3 partes

- **Grid y nodos**
<br>
Crearemos un sistema de nodos para dividir el terreno en una cuadrícula, la cual es manipulable desde el editor. Usaremos este sistema de cuadrícula para alimentar el algoritmo A*, el cual determinará el mejor camino desde el punto actual hasta el objetivo. El script devolverá una lista de puntos Vector3 que suponen el paso entre los puntos administrados en la función FindPath del script.

<br>

Para rellenar la cuadrícula se ha implementado un sistema de detección de colisiones para el cual se ha de poner la lista de las capas que representan las casillas no caminables.

---
- **Sistema de guardado**
<br>
Usaremos el sistema de guardado genérico proporcionado en la carpeta de Utils para guardar la información del grid.

---
- **Edición desde el editor**
<br>
Desde el editor se permite la administración del grid, el tamaño de éste y cuantas celdas posee, los colores para la representación visual del mismo, las capas de colisión para la detección automática, el porcentaje de la celda que se desea comprobar y el nombre del archivo que se usará para guardar la información del grid.

<br>

Por último se programaran los gizmos de tal manera que estos solo se muestran cuando el objeto que contiene el script es seleccionado, de esta manera no molestará en el desarrollo del proyecto. Al finalizar de configurar el grid se pulsa el botón del editor para que el script comience la comprobación de celdas.

