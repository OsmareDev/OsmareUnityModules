# Collectibles
![a_11_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/cf2e7428-dba3-4e1e-b7dc-c5dabccf78f8)
![a_11_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/609955a2-9e25-402c-b843-8aadd59b1ae4)

# English

Collectible elements play a fundamental role in numerous video games, providing various functionalities and benefits to the player. These items are designed to provide new abilities, increase a counter that represents in-game money, or simply unlock additional in-game content.

<table>
    <tr><td><b>Problem</b></td></tr>
    <tr><td>Unity lacks an item collection system, leaving the implementation to developers.</td></tr>
    <tr><td><b>Solution</b></td></tr>
    <tr><td>We will create an ICollectible interface that allows turning any object into a collectible and a CollectibleCatcherController script that interacts with these collectibles.</td></tr>
</table>

The ICollectible interface will be implemented with the GetCollected function, which will allow us to use this function from the collector controller if the interface is detected.

Within the CollectibleCatcherController, we will use two functions provided by Unity to manage collisions and thus determine when we step over an element.

- [OnTriggerEnter](https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html) will allow us to detect these collisions with a [RigidBody](https://docs.unity3d.com/es/2019.4/Manual/ class-Rigidbody.html)
- [OnControllerColliderHit](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnControllerColliderHit.html) will allow us to detect collisions with a [CharacterController](https://docs.unity3d.com/ScriptReference/CharacterController.html)

In both functions, we will use [TryGetComponent](https://docs.unity3d.com/ScriptReference/Component.TryGetComponent.html) instead of [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent. html), since the first uses memory only if the component exists.

We will also do an extra implementation in case it is decided not to use any of the detections provided by Unity. Which will be loaded through a delegate to maintain efficiency and avoid checking each frame.

With this we will provide an example implementation of the ICollectible interface, where once the function is called, we will calculate a bezier curve to transport the collectible to the player and once we reach it, add a point to a counter.

# Español

Los elementos coleccionables desempeñan un papel fundamental en numerosos videojuegos, aportando diversas funcionalidades y beneficios al jugador. Estos elementos están diseñados con el objetivo de brindar nuevas habilidades, incrementar un contador que representa el dinero en el juego o, simplemente, desbloquear contenido adicional del juego.

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>Unity carece de un sistema de recogida de items, dejando a los desarrolladores la implementación.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>Crearemos una interfaz ICollectible que permite convertir cualquier objeto en un coleccionable y un script CollectibleCatcherController que interactúa con estos coleccionables.</td></tr>
</table>

La interfaz ICollectible se implementará con la función GetCollected, la cual servirá para que desde el controlador del recolector podamos hacer uso de esta función si se detecta la interfaz.

Dentro del CollectibleCatcherController, utilizaremos dos funciones proporcionadas por Unity para gestionar las colisiones y de esta manera determinar cuándo pasamos por encima de un elemento.

- [OnTriggerEnter](https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html) nos permitirá detectar estas colisiones con un [RigidBody](https://docs.unity3d.com/es/2019.4/Manual/class-Rigidbody.html)
- [OnControllerColliderHit](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnControllerColliderHit.html) nos permitirá detectar las colisiones con un [CharacterController](https://docs.unity3d.com/ScriptReference/CharacterController.html)

En ambas funciones, emplearemos [TryGetComponent](https://docs.unity3d.com/ScriptReference/Component.TryGetComponent.html) en lugar de [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html), ya que la primera utiliza memoria sólo si el componente existe.

También haremos una implementación extra en caso de que se decida no usar ninguna de las detecciones proporcionadas por Unity. La cual se cargará a través de un delegado para mantener la eficiencia y evitarnos comprobaciones cada frame.

Con esto proporcionaremos un ejemplo de implementación de la interfaz ICollectible, donde una vez llamado a la función, calcularemos un curva de bézier para transportar el coleccionable hasta el jugador y una vez lleguemos a el sumar un punto a un contador.
