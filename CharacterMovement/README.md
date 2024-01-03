# Character Movement
![a_10_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/77cd6e92-79d9-4e46-b31c-046196e457d4)


# English

**The script is compatible with any other script that implements the attached interface or any other script that contains that base information, so it will also work with the Unity Character controller!**

Creating a movement system is one of the most complicated things when creating a video game. Many games base their main mechanics on this and that is why it should feel like one of the most fun or special things when playing.

In order to develop functional movement in both 2 dimensions and 3 dimensions, it is necessary to establish a reference system. In many cases, the camera is used to provide greater intuitiveness of the controls. Once we have established the reference system, it is necessary to determine the directions to take into account, which is up to the developer. In both cases, the vertical axis will be considered; However, in 2D environments, only one of the horizontal axes will be taken into account.

In movement systems, methods are necessary to make the player feel like they are doing things right. To do this, mechanics such as coyote time (having a margin of error when jumping from platforms) and jump buffering (saving the jump input) are implemented. so you don't have to do it perfectly when jumping from the ground)

## **Horizontal Movement**
If the player moves, acceleration will be applied to the displacement vector. This acceleration will give the character inertia. The dot product will be used to determine when the player's direction vector opposes the displacement vector. When this is the case, braking acceleration will be used to stop it before resuming normal acceleration in the direction of motion.

The magnitude of the displacement vector will be limited by the developer, although it can be exceeded at the peak of a jump. This is used to provide better jump control to the player.

Finally, if the player is not moving, braking will be applied to the character depending on whether they are in the air or on the ground. Additionally, if the character collides with a wall, he will stop completely.

## **Vertical Movement**
When the player is in the air, a calculation will be made to determine where they are in the jump. This will be accomplished by evaluating how close the vertical speed is to being 0. If the vertical speed is 0, it means that the character is at the highest point of the jump. Based on this, a higher or lower acceleration will be applied.

If the player breaks the jump before reaching the highest point, a force will be applied to try to stop the jump, giving them complete control over the duration of the jump. After performing these calculations, we ensure that the fall speed does not exceed a certain threshold, thus limiting the speed at which the player falls.

When performing a jump, it will be taken into account not only if the jump action has been activated, but also if it has been pressed within a specific time interval. This way, the player does not need to perform perfect timing with ground detection. You are also given the ability to jump a few moments after leaving the ground, as long as you did not do so by jumping. This is known as "[coyote time](https://en.wiktionary.org/wiki/coyote_time)" and is used to avoid the feeling of controls failing when approaching edges.

Finally, it is verified that the player does not collide with any ceiling. If a collision occurs, the vertical speed will become 0.

# Español

**¡El script es compatible con cualquier otro script que implemente la interfaz adjuntada o cualquier otro script que contenga esa información base, por lo que funcionará también con el Character controller de unity!**

La realización de un sistema de movimiento es de las cosas más complicadas a la hora de crear un videojuego, muchos juegos basan su mecánica principal en esto y por ello debe sentirse de las cosas más divertidas o especiales a la hora de jugar. 

Para poder desarrollar un movimiento funcional en tanto 2 dimensiones como 3 dimensiones, es necesario establecer un sistema de referencia. En muchos casos, se emplea la cámara para proporcionar una mayor intuición de los controles. Una vez que hemos establecido el sistema de referencia, es necesario determinar las direcciones a tener en cuenta, lo cual queda a elección del desarrollador. En ambos casos, se considerará el eje vertical; sin embargo, en entornos 2D, sólo se tendrá en cuenta uno de los ejes horizontales.

En los sistemas de movimiento es necesario métodos para hacer que el jugador sienta que hace las cosas bien, para ello se implementan mecánicas como el coyote time (tener un margen de error al saltar de plataformas) y el jump buffering (guardar el input de salto para no tener que hacerlo perfecto a la hora de saltar desde el suelo)

## **Movimiento Horizontal**
Si el jugador se mueve, se aplicará aceleración al vector de desplazamiento. Esta aceleración conferirá al personaje inercia. Se empleará el producto escalar para determinar cuándo el vector dirección del jugador se opone al vector de desplazamiento. Cuando sea así, se utilizará la aceleración de frenado para detenerlo antes de retomar la aceleración normal en la dirección del movimiento.

La magnitud del vector de desplazamiento se verá limitada por el desarrollador, aunque podrá superarse en el punto álgido de un salto. Esto se utiliza para proporcionar un mejor control del salto al jugador.

Por último, si el jugador no está en movimiento, se aplicará un frenado al personaje dependiendo de si este está en el aire o en el suelo. Además, si el personaje colisiona con una pared, se detendrá completamente.

## **Movimiento Vertical** 
Cuando el jugador se encuentre en el aire, se realizará un cálculo para determinar en qué punto del salto se encuentra. Esto se logrará evaluando qué tan cercana está la velocidad vertical de ser 0. Si la velocidad vertical es 0, significa que el personaje se encuentra en el punto más alto del salto. En base a esto, se aplicará una aceleración más alta o más baja.

Si el jugador interrumpe el salto antes de alcanzar el punto más alto, se aplicará una fuerza para intentar detenerlo, otorgándole así un control absoluto sobre la duración del salto. Después de realizar estos cálculos, nos aseguramos de que la velocidad de caída no supere un umbral determinado, limitando de esta manera la velocidad a la que el jugador cae.

Al momento de realizar un salto, se tomará en cuenta no solo si se ha activado la acción de salto, sino también si se ha pulsado en un intervalo de tiempo específico. De esta manera, el jugador no necesita realizar una sincronización perfecta con la detección del suelo. También se le brinda la posibilidad de saltar unos instantes después de haber dejado el suelo, siempre y cuando no lo haya hecho mediante un salto. Esto se conoce como "[tiempo coyote](https://www.devuego.es/gamerdic/termino/coyote-time/)" y se utiliza para evitar la sensación de que los controles fallan al acercarse a los bordes.

Por último, se verifica que el jugador no colisione con ningún techo. En caso de que ocurra una colisión, la velocidad vertical pasará a ser 0.


