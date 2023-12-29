# Shooting System 2D
![a6_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/5413d407-863d-4051-b79b-07409c2b5d2d)
![a6_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/aa2b61f5-99c3-44c9-a105-432ffebeaba3)

# English

To create a 2-dimensional shooting system we need to take into account 2 processes:

## Aim
**Pointing direction is received through the SetAimDirection function**

When deciding how to aim we are going to create different options:
1. **Free pointing** You point in the direction you want
2. **Assisted Aim** The gameObject provided to the script is automatically pointed to, if none is provided a search will be performed using [OverlapCircleAll](https://docs.unity3d.com/ScriptReference/ Physics2D.OverlapCircleAll.html) to detect the targets of the layer marked in the editor.
3. **4 directions** The direction to point to is converted to 4 possibilities
4. **8 directions** The direction to point to is converted to 8 possibilities
5. **N directions** The direction to point to is converted to as many possibilities as those indicated from the editor

Finally we apply the rotation, if the rotation speed is 0 we determine that an instantaneous transition is desired, otherwise the chosen speed will be used.

## Shoot:

When the shooting action is performed, the “weapon” statistics are used (which for greater ease are created through a scriptable object.) The weapon's firing speed, the firing angle, how many bullets per bullet are taken into account. shot are going to be launched and the distribution that these are going to follow. Finally, the bullet pool is asked for an iteration of the type of bullet the weapon uses.

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>We need to handle different types of ammunition, for which Unity's <a href="https://docs.unity3d.com/ScriptReference/Pool.ObjectPool_1.html">ObjectPool</a> does not work </td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>I will create my own object structure with times, to pool the ammunition</td></tr>
</table>

From here, if we want to add different types of ammunition, we only have to create them from the base class

# Español

Para crear un sistema de disparos en 2 dimensiones necesitamos tener en cuenta 2 procesos:

## Apuntar
**Se recibe la dirección de apuntado a través de la función SetAimDirection**

A la hora de decidir cómo apuntar vamos a crear diferentes opciones:
1. **Apuntado libre** Se apunta en la dirección que se desea
2. **Assisted Aim** Se apunta automáticamente al gameObject que se le proporcione al script, en caso de que no se le proporcione ninguno se realizará una búsqueda usando [OverlapCircleAll](https://docs.unity3d.com/ScriptReference/Physics2D.OverlapCircleAll.html) para detectar los objetivos de la capa marcada en el editor.
3. **4 direcciones** La dirección a la que se debe apuntar se convierte a 4 posibilidades 
4. **8 direcciones** La dirección a la que se debe apuntar se convierte a 8 posibilidades 
5. **N direcciones** La dirección a la que se debe apuntar se convierte a tantas posibilidades como las indicadas desde el editor

Por último aplicamos la rotación, en caso de ser 0 la velocidad de rotación determinamos que se desea una transición instantánea, en caso contrario se usará la velocidad escogida.

## Disparar: 

Cuando se realiza la acción de disparar se usan las estadísticas del “arma” (las cuales para mayor facilidad se crean a través de un scriptable object.) Se tiene en cuenta la velocidad de disparo del arma, el ángulo de disparo, cuantas balas por disparo se van a lanzar y la distribución que van a seguir estas. Por último se le pide a la pool de balas una iteración del tipo de bala que usa el arma.

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>Necesitamos manejar distintos tipos de munición, Para lo cual el <a href="https://docs.unity3d.com/ScriptReference/Pool.ObjectPool_1.html">ObjectPool</a> de unity no sirve</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Creare mi propia estructura de objetos con tiempos, para hacer un pool de la munición</td></tr>
</table>

A partir de aquí si queremos añadir distintos tipos de munición solo debemos crearlos a partir de la clase base
