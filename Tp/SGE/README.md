TP 2 Seminario de lenguajes (.NET)

 
Para acceder a la aplicación realizada, luego de ejecutar el programa debe acceder por el siguiente link: http://localhost:5000/SGE o http://localhost:5001/SGE (dependiendo la computadora utilizada).

Una vez dentro de la aplicación, desplazarse hasta la sección de usuarios y seleccionar la opción de /api/Usuarios/login, completando los datos con las siguientes credenciales:

Administrador: 
*email: admin@sge.com
*contraseña:admin123
   *id:11111111-1111-1111-1111-111111111111

   Este usuario posee todos los permisos requeridos para crear, modificar o eliminar expedientes, tramites y usuarios.
Para dar de altas expedientes debemos utilizar el token generado al iniciar sesión, nos dirigimos al botón "Authorize" localizado en la parte superior derecha, al ingresar ponemos el token en la sección "Value" y confirmamos presionando el botón "Authorize" y por ultimo le damos a "Close". Una vez realizado estos pasos podemos dirigirnos a la sección de Expedientes y a la opción /api/Expedientes donde presionaremos Try in out, e ingresamos los datos solicitados y ejecutamos.
