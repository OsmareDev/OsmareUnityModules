# Collisions 2D
![a7_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/2ac45408-a174-4e0a-ae23-a3dabc50f3ef)
![a7_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/d657dc3d-f98f-4151-b276-b2cdf2134fe7)


# English
# In progress... (Done: TopDown Collision System)

Collisions in a video game are the basis of any movement system and therefore constitute a fundamental part of their development.

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>Unity brings with it a physics-based movement which is a problem when it comes to implementing dynamic and unrealistic movements. On the other hand, it also brings a character controller that, despite being decent in 3D, has quite a few problems. </td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>We will make our own collision detection system</td></tr>
</table>
 
To do this, each frame we will detect the initial and final position of the movement to be made. In case of detecting a collision against the layer indicated in the editor, we will calculate the new movement vector.

<br>

As an extra, to make the collisions better and less clumsy we will add a collision system with an angle, if the collision falls within the angle indicated from the editor the character will slide against the collision. This way we have a collision that is less frustrating for the player.

…

# Español
# En progreso... (Hechos: Sistema de Colisiones TopDown)

Las colisiones en un videojuego son la base de cualquier sistema de movimiento y por ende constituyen una parte fundamental en el desarrollo de los mismos.

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>Unity trae consigo un movimiento por físicas el cual es un problema a la hora de implementar movimientos dinámicos y poco realistas, por el otro lado tambien trae un character controller que a pesar de ser decente en 3D tiene bastantes problemas.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Realizaremos nuestro propio sistema de detección de colisiones</td></tr>
</table>
 
Para ello detectaremos cada frame la posición inicial y final del movimiento a realizar, en caso de detectar una colisión contra la capa señalada en el editor calcularemos el nuevo vector de movimiento

<br>

Como extra, para que las colisiones sean mejores y menos torpes añadiremos un sistema de colisión con ángulo, si la colisión entra dentro del ángulo señalado desde el editor el personaje resbalará contra la colisión. De este modo tenemos una colisión que es menos frustrante para el jugador.

…
