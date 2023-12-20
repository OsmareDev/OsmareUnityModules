# AutoScroll Background
![a3_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/0fc8db23-7f0c-45ee-b39b-a68c3b604fec)

# English

<tabla>
   <tr><td><b>Problema</b></td></tr>
   <tr><td>La opción más similar que Unity ofrece para una barra de interfaz es el uso de una <a href="https://docs.unity3d.com/es/2019.4/Manual/script-Scrollbar.html"> ScrollBar.</a> Aunque es posible animarla mediante código, este componente no permite crear una barra de carga circular ni una barra que contenga diferentes partes funcionando como una sola.</td></tr>
   <tr><td><b>Solución</b></td></tr>
   <tr><td>Podemos hacer uso de <a href="https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image.html">Imagen</a> y su propiedad <a href= "https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html">fillAmount.</a> Calculando en el módulo BarController la animación y funcionalidad.</td></tr>
</tabla>

Dentro del módulo, podemos ajustar los valores de [fillAmount](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html) en un rango personalizado, aunque el rango sea de 0 a 1 Para ello el desarrollador puede especificar un valor máximo para adaptar la barra a cualquier rango de valores.

Al implementar el módulo, consideraremos dos casos:

- ### Si se desea animar la transición:
Se calculará, en cada fotograma, la posición de [fillAmount](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html) necesaria para pasar de la posición actual a la deseada.

- ### Si no se desea animación:
Se calculará y actualizará directamente el valor de [fillAmount](https://docs.unity3d.com/es/2018.4/ScriptReference/UI.Image-fillAmount.html).

En el caso de una barra compuesta por varias imágenes se deben considerar como una sola:

1. Se tendrá en cuenta el número de imágenes para determinar el valor de cada una.
2. Se calculará cuántas imágenes deben llenarse por completo.
3. La siguiente imagen se ajustará al valor restante
4. El resto se vaciarán.

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
