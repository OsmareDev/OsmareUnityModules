# Portal 2D
![a9_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/d24d3cdc-ff08-46e2-b77b-853a8b149b84)
![a9_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/4d441f15-e713-41d6-bf7c-d0ca38c0ecb3)


# English

To create 2D portals we are going to rely on the use of masks to give the sensation that they disappear, while we will detect the introduction of the object into the portal area using raycast and through another calculation we will ensure that the object is inside the portal . In this way we can get portals with different oval shapes.

<br>

If it is completely inside the portal, a copy will be created in the portal to which the entry portal is connected. We will use a data structure attached to the utils folder, the structure is a temporary dictionary that all portals will share, the use of this structure will serve to store the copies and attach them to the original object, in case another portal requires a copy of the same object, instead of instantiating a new one, we will save resources by using the previous copy. Once some time has passed after the object has left the portal behind, the dictionary itself will clean up all the copies that have not been used for some time.

<br>

When the portal is crossed until the player is further inside than outside, the portal will move the player to the opposite portal. You can include through a list the names of scripts that you do not want to be deleted from the copies; this, for example, can help them reproduce their originals a little better. Finally, you can also decide which layers the portal can ignore so that it does not interact with said layers and they cannot pass through the portal.

# Español

Para la realización de portales en 2D vamos a basarnos en el uso de máscaras para dar la sensación de que desaparecen, mientras detectaremos la introducción del objeto en el área del portal mediante raycast y mediante otro cálculo nos aseguraremos de que el objeto esta dentro del portal. De esta forma podemos conseguir portales con diferentes formas ovaladas.

<br>

En caso de estar completamente dentro del portal se creará una copia en el portal al cual está conectado el portal de entrada. Haremos uso de una estructura de datos adjuntada en la carpeta utils, la estructura es un diccionario temporal que todos los portales compartirán, el uso de esta estructura servirá para almacenar las copias y adjuntarlas al objeto original, en caso de que otro portal requiera una copia del mismo objeto en vez de instanciar una nueva ahorraremos recursos haciendo uso de la copia anterior, una vez transcurrido un tiempo después de que el objeto haya dejado atrás el portal el diccionario por si mismo hará limpieza de todos las copias que lleven tiempo sin utilizarse.

<br>

Cuando se atraviese el portal hasta estar más dentro que fuera el portal moverá al jugador al portal contrario. Se pueden incluir a través de una lista los nombres de scripts que no se desea que se borren de las copias, esto por ejemplo puede servir para que estas emiten un poco mejor a sus originales. Por ultimo tambien se puede decidir que capas puede ignorar el portal para que este no interactuar con dichas capas y estas no puedan atravesar el portal.
