# Shooting System
![a4_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/79e58cc7-a864-48d6-937f-0b8a287a8cf3)
![a4_3](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/f544a8dc-5aef-4d59-ad5f-73bad878837d)


# English

When implementing the dialogue system, we must consider that it is not just about displaying text on the screen or a representative image of the character who speaks. A method needs to be established to link the texts together, so that we know which is next in the sequence. Additionally, some dialogues may offer options that branch the conversation.

This leaves us with the following challenges:

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>Need to link one text with the next.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>We can design a structure or class that stores the text, the reference to the image and a pointer that points to the next structure. In this way, when we finish displaying the current text and want to display the next one, we simply have to follow the reference indicated by the pointer.</td></tr>
</table>

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>We must consider that the dialogue may need to wait for a decision, as well as follow one route or another depending on said decision.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>To solve this problem we will create a base class called BasicNode, which will contain only the text and the image. Next, the following classes derived from it will be created:
<br><br>
<b>-DialogNode:</b> It will only have a reference to the next node.
<br>
<b>-DecisionNode:</b> Will have a list of structures consisting of a text with the decision and a pointer to the node that corresponds to that decision.</td></tr>
</table>

To facilitate the creation of dialogs from the application, the nodes have been converted into [ScriptableObjects](https://docs.unity3d.com/Manual/class-ScriptableObject.html).

The functionality is as follows:

1. **Dialogue Node** The initial text nodes are stored in the nodes, from here we can select if we want these nodes to be executed in order. From the outside we start the dialogue and this module is responsible for telling the **Dialogue Box** that it should display the message.
2. **Dialogue Box** From here the necessary checks are made and it is controlled that the message is written in the indicated place, a text effect derived from the interface **ITypeEffect** can be added
3. **Type Effect** These modules are responsible for writing the text as programmed, to maintain the order 3 functions are used,
- Begin: Responsible for starting the writing process
- Check: It is responsible for checking if the writing process is still in progress
- Finish: It is responsible for abruptly ending the writing process

# Español

Al implementar el sistema de diálogos, debemos considerar que no se trata únicamente de mostrar texto en pantalla o una imagen representativa del personaje que habla. Es necesario establecer un método para vincular los textos entre sí, de modo que sepamos cuál es el siguiente en la secuencia. Además, algunos diálogos pueden ofrecer opciones que ramifican la conversación.

Esto nos deja los siguientes retos:

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>Necesidad de vincular un texto con el siguiente.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Podemos diseñar una estructura o clase que almacene el texto, la referencia a la imagen y un puntero que señale a la siguiente estructura. De esta manera, al finalizar la visualización del texto actual y querer mostrar el siguiente, simplemente debemos seguir la referencia señalada por el puntero.</td></tr>
</table>

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>Debemos considerar que el diálogo puede necesitar esperar una decisión, así como seguir una ruta u otra en función de dicha decisión.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Para solucionar dicho problema crearemos una clase base llamada BasicNode, que contendrá solo el texto y la imagen. A continuación, se crearán las siguientes clases derivadas de ella:
<br><br>
<b>-DialogNode:</b> Tendrá únicamente una referencia al siguiente nodo.
<br>
<b>-DecisionNode:</b> Tendrá una lista de estructuras que constan de un texto con la decisión y un puntero al nodo que corresponde a esa decisión.</td></tr>
</table>

Para facilitar la creación de diálogos desde la aplicación se han convertido los nodos en [ScriptableObjects](https://docs.unity3d.com/Manual/class-ScriptableObject.html).

La funcionalidad es la siguiente:

1. **Dialogue Node** En los nodos se almacenan los nodos de texto iniciales, desde aquí podemos seleccionar si queremos que dichos nodos se ejecuten en orden. Desde el exterior comenzamos el diálogo y este módulo se encarga de comunicarle al **Dialogue Box** que debe mostrar el mensaje.
2. **Dialogue Box** Desde aquí se hacen las comprobaciones necesarias y se controla que el mensaje sea escrito en el lugar indicado, se puede añadir un efecto de texto derivado de la interfaz **ITypeEffect**
3. **Type Effect** Estos módulos se encargan de escribir el texto según se programe, para mantener el orden se usan 3 funciones,
- Begin: Se encarga de comenzar el proceso de escritura
- Check: Se encarga de comprobar si el proceso de escritura sigue en curso
- Finish: Se encarga de terminar abruptamente el proceso de escritura
