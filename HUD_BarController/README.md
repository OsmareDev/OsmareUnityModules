# AutoScroll Background
![a3_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/0fc8db23-7f0c-45ee-b39b-a68c3b604fec)

# English

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>The most similar option that Unity offers for an interface bar is the use of a <a href="https://docs.unity3d.com/es/2019.4/Manual/script-Scrollbar.html" > ScrollBar.</a> Although it is possible to animate it through code, this component does not allow you to create a circular loading bar or a bar that contains different parts functioning as one.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>We can make use of <a href="https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image.html">Image</a> and its property <a href= "https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html">fillAmount.</a> Calculating the animation and functionality in the BarController module.</td></tr>
</table>

Within the module, we can set the values of [fillAmount](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html) to a custom range, even if the range is 0 to 1 To do this, the developer can specify a maximum value to adapt the bar to any range of values.

When implementing the module, we will consider two cases:

- ### If you want to animate the transition:
In each frame, the position of [fillAmount](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html) necessary to go from the current position to the desired one will be calculated.

- ### If animation is not desired:
The value of [fillAmount](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html) will be calculated and updated directly.

In the case of a bar composed of several images, they must be considered as one:

1. The number of images will be taken into account to determine the value of each one.
2. It will calculate how many images need to be filled completely.
3. The next image will adjust to the remaining value
4. The rest will be emptied.

# Español

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>La opción más similar que Unity ofrece para una barra de interfaz es el uso de una <a href="https://docs.unity3d.com/es/2019.4/Manual/script-Scrollbar.html"> ScrollBar.</a> Aunque es posible animarla mediante código, este componente no permite crear una barra de carga circular ni una barra que contenga diferentes partes funcionando como una sola.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Podemos hacer uso de <a href="https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image.html">Image</a> y su propiedad <a href="https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html">fillAmount.</a> Calculando en el módulo BarController la animación y funcionalidad.</td></tr>
</table>

Dentro del módulo, podemos ajustar los valores de [fillAmount](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html) en un rango personalizado, aunque el rango sea de 0 a 1. Para ello el desarrollador puede especificar un valor máximo para adaptar la barra a cualquier rango de valores.

Al implementar el módulo, consideraremos dos casos:

- ### Si se desea animar la transición:
Se calculará, en cada fotograma, la posición de [fillAmount](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html) necesaria para pasar de la posición actual a la deseada.

- ### Si no se desea animación:
Se calculará y actualizará directamente el valor de [fillAmount](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html).

En el caso de una barra compuesta por varias imágenes se deban considerar como una sola:

1. Se tendrá en cuenta el número de imágenes para determinar el valor de cada una.
2. Se calculará cuántas imágenes deben llenarse por completo.
3. La siguiente imagen se ajustará al valor restante
4. El resto se vaciaran.
