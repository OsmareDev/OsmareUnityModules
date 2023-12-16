# AutoScroll Background
![a1_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/e6dc7063-684e-4bbb-87b3-3731492401a5)
---
![a1_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/2b2e2e39-d2da-41f7-b6d5-80494f762960)
---
![a1_3](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/098552b9-071b-4544-8a7e-eb0debddcea9)
---
![a1_4](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/00fa4c28-0461-47e0-9292-5b9d74f245bf)
---

# English

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>When it comes to animating backgrounds and images, a fundamental task is to automatically pan the image to add expression or generate movement in the background. Unity does not provide a direct way to perform such an action.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>To solve this problem, the BackgroundMovement module will be created. This module will use one of Unity's image components to animate it in different ways.</td></tr>
</table>

Unity provides us with two image modules:

1. The component "[Image](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image.html)", used to display a sprite in the user interface, has different variables, in In no case does it allow us to alter the texture.

2. The "[Raw Image](https://docs.unity3d.com/es/2018.4/Manual/script-RawImage.html)" component allows us to view any type of image and gives us access to the UV rectangle. This rectangle represents the coordinates of the texture and we can modify it through code to give the sensation or illusion that the image is moving without having to move it.

Next, we will proceed to calculate the amount of rotation in each frame, based on the developer's decisions. We will then calculate the amount of displacement based on the speed specified by the developer. To perform this calculation in each frame, we will use the function [Time.deltaTime](https://docs.unity3d.com/es/530/ScriptReference/Time-deltaTime.html), which represents "the time in seconds it took to the last frame is completed".

The direction of scrolling will also be determined by the developer. If it is desired that, independently of the rotation, the image moves in a single direction, to do so we counteract the current rotation by rotating the calculated motion vector in the opposite direction.

# Español

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>Cuando se trata de animar fondos e imágenes, una tarea fundamental es realizar un desplazamiento automático de la imagen para agregar expresión o generar movimiento en el fondo. Unity no provee una forma directa de realizar dicha acción.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Para solventar este problema se creará el módulo BackgroundMovement. Este módulo utilizará uno de los componentes de imagen de Unity para animarlo de distintas maneras.</td></tr>
</table>

Unity nos proporciona dos módulos de imagen:

1. El componente "[Image](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image.html)", utilizado para mostrar un sprite en la interfaz de usuario, cuenta con distintas variables, en ningún caso nos permite alterar la textura.

2. El componente "[Raw Image](https://docs.unity3d.com/es/2018.4/Manual/script-RawImage.html)" nos permite visualizar cualquier tipo de imagen y nos brinda acceso a el rectángulo UV. Este rectángulo representa las coordenadas de la textura y podemos modificarlo mediante código para dar la sensación o la ilusión de que la imagen está en movimiento sin tener que moverla.

A continuación, procederemos a calcular la cantidad de rotación en cada frame, basándonos en las decisiones del desarrollador. Después calcularemos la cantidad de desplazamiento en función de la velocidad especificada por el desarrollador. Para realizar este cálculo en cada frame, utilizaremos la función [Time.deltaTime](https://docs.unity3d.com/es/530/ScriptReference/Time-deltaTime.html), que representa "el tiempo en segundos que tardó en completarse el último frame".

La dirección del desplazamiento también será determinada por el desarrollador. En caso de que se desee que, independientemente de la rotación, la imagen se desplace en una dirección única, para ello contrarrestamos la rotación actual girando en dirección opuesta el vector de movimiento calculado.
