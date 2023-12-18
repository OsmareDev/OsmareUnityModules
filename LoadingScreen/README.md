# Loading Screen
![a2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/944dd90a-c61e-4a05-aab9-d3e19ae7c484)

# English

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>In Unity, scene switching functionality is provided, but performing this transition does not hide the object loading of the next scene, resulting in a direct display of the object loading , similar to a theatrical performance without a curtain.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>To solve this problem we will activate a canvas and by updating a loading bar we will give the player information about the progress.</td></tr>
</table>

To keep the module present in the scene consistently, it will be implemented using the **Singleton** design pattern. This module will ensure that it is unique to the scene and will be accessible from anywhere in the code.

The module will receive as a parameter the canvas that you want to use as the loading screen, as well as the canvases that should be deactivated if necessary. The module will then start loading asynchronously and update the loading bar, which has been implemented using the **BarController** module.

Once the scene has fully loaded, the loading screen is disabled and the corresponding scene will be displayed.

# Español

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>En Unity, se proporciona la funcionalidad de cambio de escenas, pero al realizar esta transición, no se oculta la carga de los objetos de la siguiente escena, lo que resulta en una visualización directa de la carga de los objetos, similar a una representación teatral sin telón.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Para solventar este problema activaremos un canvas y actualizando una barra de carga daremos información al jugador del progreso.</td></tr>
</table>

Para mantener el módulo presente en la escena de forma constante, se implementará utilizando el patrón de diseño **Singleton**. Este módulo se asegurará de ser único en la escena y estará accesible desde cualquier parte del código.

El módulo recibirá como parámetro el canvas que se desea usar como pantalla de carga, así como los canvas que deben desactivarse si es necesario. A continuación, el módulo iniciará la carga de forma asíncrona y actualizará la barra de carga, la cual se ha implementado utilizando el módulo **BarController**.

Una vez que la escena se haya cargado por completo, se desactiva la pantalla de carga y se mostrará la escena correspondiente.
