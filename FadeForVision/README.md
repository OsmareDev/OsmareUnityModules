# Fade For Vision
![a_14_1](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/a7fe68c5-c23e-4e48-99af-504cdc324e36)
![a_14_2](https://github.com/OsmareDev/OsmareUnityModules/assets/50903643/a461deb6-4ff9-4ad4-95f1-4443e6d286fe)

# English

When addressing the possibility of allowing the player to see the character through walls, we will focus on handling the transparency of said walls.

<table>
   <tr><td><b>Problem</b></td></tr>
   <tr><td>Calculating the player's position with respect to the stage is quite expensive to do from a shader or a script alone.</td></tr>
   <tr><td><b>Solution</b></td></tr>
   <tr><td>Implement an interface in charge of managing the transparency of the object and a controller class that will notify said objects when they should disappear.</td></tr>
</table>

- XRayController
This module will make use of a [RaycastAll](https://docs.unity3d.com/ScriptReference/Physics.RaycastAll.html) that starts from the Near plane of the camera, in order to avoid collisions with non-rendered objects. Additionally, each impacted object will be checked to see if it contains the FadeObject module, in which case it will be prompted to become transparent.

- FadeObject, upon receiving the command to become transparent, will use an asynchronous function to perform the transition and remain translucent until the object is no longer between the player and the camera.

- DitherShader is responsible for determining, based on the value of the transparency property, the number of pixels that must be discarded to achieve the sensation of transparency. This approach saves resources by avoiding unnecessary calculations on pixels, unlike the transparency process that requires additional calculations on each pixel.

# Español

Al abordar la posibilidad de permitir que el jugador vea al personaje a través de las paredes, nos enfocaremos en manejar la transparencia de dichas paredes.

<table>
  <tr><td><b>Problema</b></td></tr>
  <tr><td>Realizar el cálculo de la posición del jugador con respecto al escenario es bastante costoso como para llevarlo a cabo desde un shader o un script en solitario.</td></tr>
  <tr><td><b>Solución</b></td></tr>
  <tr><td>Implementar una interfaz encargada de gestionar la transparencia del objeto y una clase controladora que avisará a dichos objetos cuándo deben desaparecer.</td></tr>
</table>

- XRayController
Este módulo hará uso de un [RaycastAll](https://docs.unity3d.com/ScriptReference/Physics.RaycastAll.html) que parte desde el plano Near de la cámara, con el fin de evitar colisiones con objetos no renderizados. Además, se verificará si cada objeto impactado contiene el módulo FadeObject, en cuyo caso se le solicitará que se vuelva transparente.

- FadeObject, una vez recibida la orden de volverse transparente, utilizará una función asíncrona para llevar a cabo la transición y mantenerse translúcido hasta que el objeto ya no se encuentre entre el jugador y la cámara.

- DitherShader se encarga de determinar, en base al valor de la propiedad de transparencia, la cantidad de píxeles que deben ser descartados para lograr la sensación de transparencia. Este enfoque permite ahorrar recursos al evitar cálculos en los píxeles innecesarios, a diferencia del proceso de transparencia que requiere cálculos adicionales en cada píxel.
