# PetPass-API
Manual Técnico 

 

Introducción:  

Bienvenido al Manual Técnico de PetPass, una exhaustiva guía diseñada para proporcionar a los usuarios una comprensión detallada de la configuración y el funcionamiento interno de este innovador proyecto.  

Este manual está diseñado para facilitar a los usuarios, desarrolladores y administradores la comprensión de las complejidades técnicas involucradas en la implementación y configuración de PetPass. A lo largo de estas páginas, exploraremos los aspectos clave de la arquitectura del sistema, las herramientas utilizadas en el desarrollo, los requisitos de hardware y software, así como los procedimientos detallados para la configuración efectiva de la aplicación. 

 

Descripción del proyecto:  

PetPass se trata de una aplicación móvil desarrollada e idealizada para cumplir la tarea de carnetizar y registrar a todas las máscotas radicadas en el área metropoliatana de Cochabamba mediante campañas de carnetización.   

 

Roles / integrantes 

      	Jhon Arnol Pari Siles - Developer 

      	Fernando Santos Davalos Lovera – Git Master/Developer 

       Elias Nahuel Gutierrez Vargas – Team Leader/Developer 

       Diego Adrian Ricaldez Aguilar - DB-Architect/Developer 

 

Arquitectura del software:  

Cuando desarrollamos la aplicación encontramos como una opción solida el patrón de diseño MVVM para tener organizado de manera clara y eficiente el código, como estructura manejamos MODELO, VISTA Y SERVICIOS 

Modelo: En esta parte definimos las clases que representan los datos y la lógica de negocio de la aplicación. 

Vista: Es la interfaz del usuario diseñada para reflejar la información proporcionada por los servicios en otras palabras mostrar datos y proporcionar la interacción del usuario en la aplicación. 

Servicio: Juntos con los modelos se establecen servicios que se encargan de gestionar la obtención y manipulación de datos 

 

Requisitos del sistema: 

Requerimientos de Hardware (mínimo): (cliente) 

Cámara 16 MP 

2Gb de RAM 

Qualcomm Snapdragon 801 

 

Requerimientos de Software: (cliente) 

Android 5.0 

 

Requerimientos de Hardware (server/ hosting/BD) 

Mínimo de 3 Gb de RAM 

Procesador con más de 1 GHz de velocidad 

Espacio de más de 2 Gb 

                   

Requerimientos de Software (server/ hosting/BD) 

Visual Studio 2022 con última actualización disponible 

SQL Server 2019 

Para el hosting del servidor (API) se debe tener algún servicio que ofrezca buen hardware debido a la carga que conllevan las tareas principales, uno recomendado puede ser Microsoft Azure. En caso de utilizar contenedores de Docker lo que se recomienda es tener un almacenamiento de más de 2Gb además, se deben realizar configuraciones previas en caso la BDD se ejecute en un contenedor Docker. 

Para el hosting de la BDD se necesita un servicio compatible con la BDD de SQL Server 2019, además que cuente con un espacio mínimo de 30 Mb para asegurar el correcto funcionamiento de la BDD. De otro modo si es que se prefiere utilizar contenedores de Docker, ingrese sus credenciales y utilice el script de la BDD, necesario posteriormente ajustar la cadena de conexión del servidor o REST API. 

Tener acceso al servicio de Buckets o storage multimedia de Firebase para almacenar las imágenes de manera dinámica. 

 

Instalación y configuración:  

Para la instalación en Android es necesario descargar el APK generado que se brindó por un link de Google drive, luego hay que activar la instalación de aplicaciones desconocidas desde ajustes, luego buscar el archivo APK descargado e instalarlo. 

Para usar el api, antes de generar el APK en la solución del proyecto en el archivo Service\BaseService.cs cambiar la URL del api a usar para el acceso a la base de datos. 

 

PROCEDIMIENTO DE HOSTEADO / HOSTING (configuración) 

B.D  

Para con la Base de Datos se prefirió el servicio de www.Somee.com  

API / servicios Web 

para el hosteado de este servicio se ha utilizado el servicio cloud de Microsoft Azure. 

Otros (FireBase, etc.) 

Para guardar las imágenes se optó por utilizar el servicio de Buckets de Firebase, por lo que deberá usarse el miso o similares. 

 

 

Detalle Servicio de cloud Multimedia FIREBASE 

 

Se optó por utilizar Firebase debido a su fácil configuración y el manejo sencillo para las imágenes que se requieren guardar. Esto es lo primero que debe configurarse ya que posteriormente no se podrá realizar cambios. Actualmente se encuentra un servicio activo con un Bucket. Para configurar una nueva instancia de Firebase y usted tenga pleno acceso deberá de dirigirse a Firebase , luego, si acceso con una cuenta de GOOGLE, arriba a la derecha se encuentra la opción de IR A CONSOLA, deberá dirigirse ahí. Deberá crear un proyecto con el nombre que usted prefiera, preferible evitar servicio de Google Analytics. Una vez creado dirigase a la opción de Todos los productos en la barra lateral izquierda, posteriormente, buscar el producto STORAGE, debe comenzar y le pedirá escoger entre modo de producción o modo de prueba, escoja modo de producción y siguiente. Escoja el servidor desde el cual estará su servicio de cloud storage (Recomendable southamerica-east1). Se creará su BUCKET, del cuál deberá guardar la URL, se encuentra remarcada en el marco donde se encuentran los archivos. 

 

 

 

IMPORTANTE, DEBE CREAR AHI MISMO 2 CARPETAS, A LA IZQUIERDA SE ENCUENTRA LA OPCION MARCADA CON UNA CARPETA CON UN ICONO DE SUMA, una debe llamarse Brigadiers y la otra deberá llamarse PETS. IMPORTANTE QUE SE LLAMEN ASI. 

DE LA URL DEBE CONSERVAR DESPUES DE gs:// 

 

gs://my-first-projectsas-e200c.appspot.com SI ESTA ES LA RUTA ORIGINAL DEBE CONSERVAR LO SIGUIENTE: my-first-projectsas-e200c.appspot.com 

 

 

Una vez configurada la interfaz del storage, diríjase ahí mismo a la opción RULES, debe modificar el código existente, donde se indique ALLOW READ, WRITE: IF FALSE; debe eliminar el :if false. Deberá quedar algo asi: 

 

 

 

Ya configurado el BUCKET debe restringir el acceso para que solo credenciales que usted permita puedan usar el servicio de subida y lectura de archivos. Para eso dirijase nuevamente a Todos los productos y ahora escoja Authentication, ingrese a comenzar y seleccione en proveedores nativos correo electrónico/contraseña para usted estipular las credenciales, de a habilitar y guarde. 

 

 

Ahora en el menú está la Opción Users, debe crear usuarios con credenciales que usted prefiera un ejemplo es correo electrónico --- adminpetpass@admin.com y contraseña --- Petp@ss123!    ----- IMPORTANTE GUARDAR LAS CREDENCIALES PARA LA CONFIGURACIÓN. 

 

Después de configurar la autenticación, dirigase a la barra lateral izquierda arriba estará una tuerca para opciones y/o ajustes. Haga click y la opción Configuración del proyecto. Una vez hecho le saldrá un parametro llamado Clave de API web, debe copiar esa clave ya que es la llave de acceso a su proyecto. 

 

Por último, ya con la URL del BUCKET, las credenciales de acceso y la clave API web, debe acceder al proyecto y dirigirse a la carpeta Services, debe configurar dos clases, PhotoBrigadierService y PhotoPetService, en ambas debe cambiar las credenciales estipuladas, como el Apikey, Bucket, AuthEmail y AuthPassword por sus credenciales nuevas. ES IMPORTANTE QUE EN EL BUCKET de su pertenencia haya nombrado las carpetas tal cual se le pidió. 

  

 

Detalle BDD - Hosting 

 

Primeramente es necesario tener la BDD hosteada para poder utilizar el servidor web correctamente, por lo que debe acceder al panel de su servicio de hosting. 

En caso de escoger  Somee inicie sesión en   Somee/Login   o regístrese en Somee/Register . Si desea usar la BDD actualmente hosteada las credenciales son las siguientes: nombre de usuario --- nahuubj . Contraseña -- Daxhulk2016 

Si desea hostear su BDD debe crear primeramente en un dominio, una vez se registre en su página de hosting, en este caso Somee le indicará que debe crearse un dominio, puede colocar el nombre que quiera, sin embargo, se recomienda utilizar el alias petpass como predeterminado. Una vez concluida la configuración de su dominio, acceder al apartado Databases y crear su base de datos con la versión 2022 de MSSQL, una vez creada debe hacer click sobre su bdd, se desplegará una interfaz que cuenta con una barra de navegación superior, debe ingresar a Run Scripts e ingresar a la opción OPEN T-SQL CONSOLE, esto desplegará una interfaz donde debe copiar el contenido del script de la bdd. Cabe resaltar que deberá crear sus credenciales para el uso de la Aplicación, más adelante se dejarán consultas listas. IMPORTANTE, ya se uso hosteado o dockerizado debe cambiar la cadena de conexión con las credenciales de su servidor en el archivo appsetings.json 

 

Detalle BDD - Dockerizado 

 

Si decide utilizar un contenedor de Docker con la imagen de SQL Server, es necesario que cree una BDD dentro de su instancia, las credenciales son las que usted ya ha generado, por lo que solo debe de hacer correr el script, de todas formas igual que de la forma hosteada deberá crearse nuevas credenciales, a tener en cuenta, ya que decide usar un contenedor de Docker debe cambiar la cadena de conexión de la REST API con sus credenciales del servidor en el archivo appsetings.json. 

 

DEBE RESPETAR EL FORMATO CON COMILLAS SIMPLES, DOBLES Y/O SIN COMILLAS, TAL CUAL ESTA ESCRITO AQUI. SOLAMENTE USAR CUANDO SE TRATA DE REGISTRAR ADMINISTRADORES. 

 

CONSULTA DE REGISTRO – PERSONA 

 

INSERT INTO Person(name, firstName, lastName, ci, gender, address, phone, email) 

VALUES(“su nombre”, “su primer apellido”, “segundo apellido opcional”, “su cedula de identidad”, ‘su genero’, “su direccion de domicilio”, su nro de telefono, “su correo electronico obligatorio que sea funcional y sea verdadero”) 

 

CONSULTA DE REGISTRO DE USUARIO 

 

INSERT INTO [User](personID, username, userpassword, rol) 

VALUES(id de la persona registrada anteriormente obligatorio, “nombre de usuario”, “contraseña”, ‘A’) 

 

 

NOTA, SI HA REGISTRADO DE MANERA MANUAL A TRAVES DE SQL SERVER, DEBERA REALIZAR LA RECUPERACION DE CONTRASEÑA, DEBIDO A QUE EL CIFRADO SE HACE DEL LADO DEL CLIENTE, NO CONFUNDIR CON PRIMER INICIO DE SESION, SE LE ENVIARA UN CODIGO DE RECUPERACION A SU CORREO. 

 

 

 

 

 

 

 

 

Detalle Servidor Web (API) - Hosting 

 

Para poder inicializar el servidor web, es decir, la REST API, se encuentra con dos modos de ejecutarla, hosteandola en algún servicio cloud como es el caso de Microsoft Azure, o, desde un contenedor de Docker. 

Hablemos primeramente del hosting de REST API, se utilizó Microsoft Azure por la facilidad con la que se realizó el hosteo, sin embargo, depende del plan escogido, ya que es gratuito tiene un hardware limitado, aunque es eficiente si se quiere utilizar la aplicación de PetPass desde cualquier parte con un dispositivo móvil. Para ello debe tener cuenta y servicios habilitados dentro de Azure, posteriormente desde el proyecto descargado de Github hacer click derecho sobre el primer archivo que cuenta con una esfera del mundo de color azul, buscar y clickar la opción publicar, deberá crear un nuevo perfil de publicación con Microsoft Azure ya que el perfil que se encuentra ahí está en uso actualmente en otra cuenta. Deberá escoger ahí lo que son sus planes si son gratuitos o de pago y finalmente aparecerá la opción finalizar, por último con el perfil publicado debe dar en publicar y empezará a subirse a Microsoft Azure. En la aplicación PetPass desarrollada en .NET MAUI deberá ingresar a la carpeta services y a la clase BaseAddress, cambiar ahí la url por la que le devuelva Azure ya que sin ello no podrá utilizar la REST API. 

 

Detalle Servidor Web (API) - Dockerizado 

 

Teniendo en cuenta que lo que usted desea es usar la REST API dockerizada deberá seguir unos pasos, primeramente debe cambiar la cadena de conexión por aquel servidor que haya escogido para la BDD, ya sea hosting o dockerizado, si no lo hizo aún, remitirse al Detalle BDD. Si ya realizó su configuración continuemos. 

El Dockerfile se encuentra creado, sin embargo, deberá configurar a preferencia los puertos elegidos para funcionar, lo puede encontrar con la palabra clave EXPOSE, esto hará que se expongan los puertos que usted selecciona para la comunicación de Docker con su equipo. Por defecto lo configuramos en 80, en caso de estar usado debe indicar el segundo. 

 

Por consiguiente, debe abrir el terminal dentro de Visual Studio, debe dirigirse a la barra de herramientas superior arriba de toda la ventana y la opción ver, debe buscar la herramienta terminal una vez hecho, debe ejecutar el siguiente comando para crear la imagen de Docker: (Donde dice Dockerfile deberá cambiarlo por como quiere que se llame su imagen de Docker). 

 

docker build -t aspnetapp -f Dockerfile . 

 

Luego, una vez creada su imagen de Docker deberá ejecutar el comando: 

 

docker run -d -p 5000:80 --name ContainerName Dockerfile 

 

A tener en cuenta, donde ve puerto 5000:80 debe cambiar el primer número es decir el 5000, ese es el puerto que escoge para que su máquina pueda comunicarse con el puerto seleccionado anteriormente, donde dice ContainerName es el nombre que le asignará a su contenedor de Docker y por último donde dice Dockerfile es el nombre que le haya puesto a su imagen de Docker anteriormente. 

 

En caso desea usar la BDD existente en SOMEE mencionada arriba, simplemente deberá ejecutar en un terminal cmd el comando: 

 

docker login     ---------- esto es para revisar si su cuenta docker se encuentra en el 			      sistema 

 

docker pull nahuubj/petpassdockerizado10 -------- esto descargará la imagen desde 							un repositorio creado con la							            imagen docker 

 

 

Posteriormente, debe revisar su docker desktop y verá que ya tiene la imagen, deberá clickar en RUN, y le pedirá crear un contenedor nuevo, debe configurarlo desde la opción OPTIONAL SETTINGS, donde deberá asignarle un nombre al contenedor, y los puertos para la comunicación. Como anteriormente se mencionó priorice utilizar el puerto principal, en este caso 80 o el que usted configure primero en el Dockerfile. El segundo puerto no es necesario, el 443 quedará vacio y se colocará 5555 donde esté :80/tcp con eso ya puede clickear RUN, se ejecutará automaticamente el contenedor de la REST API. Imagen de la descripción recien realizada: 

 

 

 

Dado que ha ejecutado el Contenedor Docker, esta versión se encuentra ligada a una Base de Datos existente, algunas de las credenciales de inicio de sesión se encuentran más abajo. 

 

GIT :  

Versión final entregada del proyecto. 

Entrega compilados ejecutables 

 

Personalización y configuración:  

No hay 

Seguridad:  

Mantener el uso del api a través de Json en su mayoría para mantener la seguridad de los datos. 

Tener cuidado con el uso de la cámara. 

No compartir Usuarios ni contraseñas. 

Se recomienda Crear nuevos usuarios para cada rol ya que estos están ligados a un correo único. 

 

Usuario Admin: diego 

Contraseña: diego1234! 

 

Usuario Brigadier: drdiego1501 

Contraseña: diego1234! 

 

 

Depuración y solución de problemas:  

Registro de errores, Comienza por revisar los mensajes de salida por consola estos registros proporcionan información sobre los errores 

Conflictos de dependencias, Verifica las dependencias del sistema asegúrate que todas las bibliotecas estén actualizadas  

Si sale error de falta de archivos o falta de paquetes apenas abres el proyecto recompila dos o tres veces, si no se arregla revise sus últimos cambios. 

Glosario de términos:  

No hay 

Referencias y recursos adicionales: Enlaces o referencias a otros recursos útiles, como documentación técnica relacionada, tutoriales o foros de soporte. 

            


Herramientas de Implementación: 

Lenguajes de programación: los lenguajes de programación que se utilizan son  C#.NET MAUI 

Frameworks:  

Newtonsoft.Json 

Camera.MAUI(1.4.4) 

Microsoft.Extensions.DependencyInjection(7.0.0) 

System.Data.SqlClient(4.8.5) 

APIs de terceros, etc. 
 

Bibliografía 

https://stackoverflow.com/questions/37866619/cleartext-http-traffic-to-myserver-com-not-permitted-on-android-n-preview 

https://www.geeksforgeeks.org/android-cleartext-http-traffic-not-permitted/ 

https://youtu.be/776e-aERehs?si=C1MDGVzbyh_3i6Ba 

https://youtube.com/playlist?list=PLTVK2lirpnSjv-Ieoxj_F35ECC_J1L9ax 

https://youtube.com/playlist?list=PLA6V-IrRGAMjpeCI8aKxD8zO4d_yubG_j 

https://youtu.be/nlZSLPf22vI?si=0ibJNGEmVpjIJiNh 


ENTREGA FINAL
    


	Figma - https://www.figma.com/file/4OfBEctmK2qXo8pxlUTtBt/CARNETIZACION---Mockups-v1?type=design&node-id=0%3A1&mode=design&t=euxBT8ZpR1hy3VnK-1
	Linear - https://linear.app/prib-carnetizacion/team/PRI/projects/all
	Github API - https://github.com/Nahuubj/PetPass-API.git
	Github APP - https://github.com/davalos31/PRI-CARNETIZACION23.git
	Docker enviado por Elias Nahuel Gutierrez Vargas (Team Leader) - https://hub.docker.com/repository/docker/nahuubj/petpassdockerizado10/general
	Apk se encuentra en Archivos
	Carpeta de QA subida en Archivos
	Video Tecnico YT - https://youtu.be/pQzP5ZMMd9U
	Video Practico YT - https://youtu.be/Kf68ZYY21Rs



 

