TP 2 Seminario de lenguajes (.NET)



Integrantes: Chiappa Del Rio Santaigo(22430/2), Cejas Juan Pablo (20814/6), Macias Ludmila Shulamit(27510/3).



Para acceder a la aplicación realizada, luego de ejecutar el programa debe acceder por el siguiente link: http://localhost:5000/SGE o http://localhost:5001/SGE 

(dependiendo la computadora utilizada).



Una vez dentro de la aplicación, desplazarse hasta la sección de usuarios y seleccionar la opción de /api/Usuarios/login, completando los datos con las siguientes credenciales:



* Administrador:
\*email: admin@sge.com
\*contraseña:admin123



&#x20;  Este usuario posee todos los permisos requeridos para crear, modificar o eliminar expedientes, tramites y usuarios.

===================== EXPEDIENTES =====================


* Para dar de altas expedientes debemos utilizar el token generado al iniciar sesión, nos dirigimos al botón "Authorize" localizado en la parte superior derecha, al ingresar ponemos el token en la sección "Value" y confirmamos presionando el botón "Authorize" y por ultimo le damos a "Close". Una vez realizado estos pasos podemos dirigirnos a la sección de Expedientes y a la opción \[POST]/api/Expedientes donde presionaremos Try in out, e ingresamos los datos solicitados y ejecutamos. De esta forma ya tendremos generado un Expediente con su id relacionada.



* Para ver el listado de los expedientes debemos dirigirnos a la opción justo debajo de la utilizada para agregar, llamada \[GET]/api/Expedientes. Con esta opción podremos ver el listado de todos los expedientes cargados en el sistema, simplemente dándole al botón Try in out veremos todos los resultados.

* Para eliminar Expedientes de nuestro sistema tendremos la opción \[DELETE]/api/Expedientes, aquí debemos presionar nuevamente el botón Try in out, luego ingresamos el id del expediente a eliminar, ejecutamos y este será eliminado, mostrándonos la confirmación abajo.

* Para consultar un expediente debemos ir a la opción \[GET]/api/Expedientes/consultar, este nos solicitara el id del expediente deseado luego de apretar el botón Try in out y al presionar "Execute" nos dará la información buscada del Expediente.

* Para modificar la información almacenada en un Expediente tenemos la opción \[PUT]/api/Expedientes/caratula, donde  nos pedirán el id del Expediente a modificar y su nuevo contenido. Luego de completar estos datos le damos a "Execute" y tendremos nuestro Expediente modificado.



* Por ultimo, para cambiar el estado del Expediente tenemos \[PUT]/api/Expediente/estado, donde ingresaremos el id del expediente y su nuevo estado, siendo:
\*0=Recien iniciado.
\*1=Para resolver
\*2=con Resolución
\*3=En notificación
\*4=Finalizado



===================== TRAMITES =====================

* Para darle de alta a los Tramites procedemos de forma similar a los Expedientes, utilizando un usuario con los permisos requeridos nos vamos a la opción \[POST] /api/Tramites, introducimos el id del Expediente asociado, su contenido y su etiqueta, al darle a "Execute" tendremos nuestro Tramite creado y asociado al Expediente.
Etiquetas:
\*0: Escrito Presentado
\*1: Pase A Estudio
\*2: Despacho
\*3: Resolución
\*4: Notificación
\*5: Pase Al Archivo

* Para modificar el contenido de un Tramite, debemos ir a la sección de \[PUT]/api/Tramites, ingresar el id de el Tramite a modificar junto a su nuevo contenido, luego confirmaremos apretando el botón "Execute".

* Para eliminar un Tramite debemos dirigirnos a la opción \[DELETE]/api/Tramites, este nos pedirá el id del Tramite y luego de darle a "Execute" este quedara eliminado. 

* Para ver todos los Tramites asociados a un Expediente debemos ir a la ante ultima opción mostrada como \[GET]/api/Tramites, este nos solicitara el id del Expediente y nos mostrara todos los datos de todos los Tramites asociados.

* Por ultimo, para ver los datos de un Tramite en especifico debemos ir a la sección de \[GET]/api/Tramites/{id}, donde este nos pedirá el id de nuestro Tramite y nos devolverá todos sus datos.



===================== USUARIOS =====================

* Para ingresar un nuevo usuario debemos dirigirnos a la sección \[POST]/api/Usuarios/registrar, donde nos solicitara un mail, nombre y una contraseña. Luego de rellenar la información y apretar "Execute" tendremos un usuario creado.

* Para iniciar sesión ya hicimos referencia al principio de la explicación.

* Para modificar la información de un Usuario, tenemos que utilizar la opción llamada \[PUT]/api/Usuarios/modificar, donde con el usuario actualmente logueado podremos modificar su nombre, email y clave.

* Para asignarle o quitarle permisos a un Usuario, debemos dirigirnos a la opción \[PUT]/api/permisos, donde nos solicitara el Id del usuario a quien queremos modificar y los permisos a asignar, siendo los siguientes:
\*0: Expediente Alta
\*1: Expediente Baja
\*2: Expediente Modificación
\*3: Tramite Alta
\*4: Tramite Baja
\*5: Tramite Modificación

* Para ver la información de un Usuario debemos ir a la sección \[GET]/api/usuarios/consultar y debemos introducir su Id y esto nos devolverá toda su información.

* Finalmente para eliminar un Usuario debemos dirigirnos a la opción llamada \[DELETE]/api/Usuarios/eliminar y aquí debemos introducir el Id del Usuario a eliminar, luego de darle a "Execute" este quedara eliminado del sistema.



===================== TODOS LOS USUARIOS =====================



* Administrador:
\*email: admin@sge.com
\*contraseña:admin123
\*id: "11111111-1111-1111-1111-111111111111"

* Usuario Solo Lectura:
\*email: prueba1@sge.com
\*contraseña: prueba123
id: "22222222-2222-2222-2222-222222222222"

* Usuario Expediente y Tramite alta:
\*email: prueba2@sge.com
\*contraseña: prueba123
id: "33333333-3333-3333-3333-333333333333"



