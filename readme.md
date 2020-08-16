## Arquitectura Gestor de inventario

## Dependencias
- Docker (docker-compose)
- Visual Studio 2019

## Instrucciones

1. Clonar la soluci�n desde el [repositorio](http://example.com).
2. Abrir la soluci�n con Visual Studio 2019
3. Seleccionar el proyecto virtual docker-compose como Start up project
4. Ejecutar en modo release (ctrl+F5)
5. La Web API estar� publicada en un contenedor. Desde el browser puede accederse a la interfaz Swagger en http://localhost:3340 o https://localhost:3341 (se redireccionar� directamente al puerto mapeado autom�ticamente por el orquestador) Nota: Si hubiera alg�n problema, revisar el mapping de puertos con el comando:
```cmd
> docker ps
```

## Arquitectura
Dada la naturaleza de las especificaciones, he optado por un grado intermedio de complejidad por motivos de esfuerzo.

### Dise�o
El dise�o de la soluci�n consiste en una arquitectura cl�sica SOA. 
Se pueden distinguir 3 capas horizontales y una vertical:

1. Acceso a datos (usando ORM Entity Framework).
  - Proyecto: Goalsystems.WebApi.Data ( Proyecto .Net Standard 2.0)
  - Models: Contiene las clases POCO que representan las entidades del ORM (**Atajo**: por el momento no se han a�adido constructores para validar el estado de estos objetos. Al usar Automapper tambi�n hay cierto handicap ya que necesita que tenga constructor impl�cito para funcionar.)
  - DatabaseContext: Contexto para acceder al sistema de persistencia
  - Infrastructure: M�todos agnosticos a la capa de acceso y m�todos de extensi�n para registrar el Contexto usando Microsoft Sql Servier como tecnolog�a para la persistencia. Si la tecnolog�a de persistencia cambiara en el futuro se podr�a implementar nuevos m�todos de extensi�n para escoger cual usar. Otra ventaja es que no te obliga a referenciar directamente el DbContext en la capa MVC.
2. Capa de negocio, donde se encuentra t�da la l�gica. 
  - Proyecto: GoalSystems.WebApi.Business (Proyecto .Net Standard 2.0)
  - Models: Contiene las clases de negocio. Si hubiera alguna regla que se pudiera resolver en las mismas no habr�a ning�n problema. Por ejemplo, m�todos o propiedades que autocalculen valores. Con los requisitos actuales no ha sido necesario m�s que controlar que los objetos son correctamente construidos por sus constructores.
  - Services: Todo lo que tiene que ver con los distintos servicios de negocio (en este caso solo existe uno). Aqu� residen la interfaz, implementaci�n, notificaciones y excepciones relacionadas con el servio encargado de gestionar la l�gica sobre elementos del inventario.
  - Models: Modelos de la capa de negocio. Estas clases deben cumplir el concepto de Ubiquitous Language, pues deben representar conceptos existentes en el lenguaje de negocio. Adem�s si por coherencia pudieran realizar operaciones y efectuar eventos, tampoco estar�a mal que se desarrollaran los m�todos en esta clase mediante m�todos y events.
  - Mappers: Se ha usado Automapper como herramienta para mapear modelos de la capa de acceso a datos con la capa de negocio. La capa de negocio jam�s debe exponer clases de una capa inferior a una superior.
  - EventHandlers: Controladores de eventos. Todas  las acciones del servicio terminan publicando una notificaci�n interna usando el patr�n Mediator. Los EventHandlers son los encargados de suscribirse a esos eventos y realizar las acciones oportunas. Por ejemplo: Publicar un evento en un Bus o cola. De esta forma, el servicio realiza las acciones de las cuales tiene contexto, como persistir datos, notifica de lo que ha hecho, y delega hacia componentes externos cualquier otra acci�n. Nota: Hay que tener en cuenta que, si la aplicaci�n se cae justo despu�s de la instrucci�n de persistencia y antes la notificaci�n, el sistema podr�a quedar en un estado inconsistente. Esta situaci�n es com�nmente conocida con 2PC (two phase commit),  es decir, no es una transacci�n at�mica. Para solventar esta situaci�n se necesitar�a implementar un sistema de transaccionabilidad entre eventos. Algunos ejemplos podr�an ser aumentando la resiliencia persistiendo en base de datos el estado de cada operaci�n como parte de la "Unit of Work", de tal forma que posteriormente, tras una ca�da o redespliegue de la aplicaci�n, podr�a consultarse si existen alg�n proceso que no ha notificado correctamente y procesarlo.
  - Infrastructure: Al igual que en la capa de acceso a datos, provee m�todos agn�sticos al negocio y de extensi�n para la inyecci�n de dependencias.
3. Capa Front. En este caso utilizando el patr�n Modelo Vista Controlador.
  Perfectamente se podr�a haber implementado en un proyecto de tipo Host, sin embargo, he preferido dividirlo en dos proyectos, uno con el c�digo y otro "dummy" que referenciar�a al anterior. Si fuera necesario crear un NuGet package y reutilizarlo con distintos proyectos Hosts, el c�digo de los controladores, configuraciones, etc quedar�a privado y se podr�a ofrecer como una soluc�on completa. Ejemplos de otras herramientas que hacen esto son HangFire, IdentityServer, Swagger...
- Proyecto: GoalSystems.WebApi.Mvc. (Proyecto .Net Core 3.1)
- Controllers: Los controladores que representan cada recurso de la API Rest.
- Infrastructure: De nuevo, m�todos de utilidad y extensiones para segregar la responsabilidad de la configuraci�n en distintos m�todos m�s manejables. Tambi�n contiene global filters de mvc, puede contener serializadores, custom binders, etc. Una clase importante es BaseStartup, una clase abstracta donde se registran todos los servicios necesarios para la aplicaci�n, y todos los middlewares. Esta clase se puede heredar en para usarla como Startup en el proyecto Host y en los proyectos de test funcionales. De esta forma al realizar los tests se utilizar� la misma configuraci�n que en la aplicaci�n, y �sta se testear� de forma impl�cita.
- Mappers: Perfiles con los mapeos para convertir objetos de la capa de negocio a modelos de la Api p�blica.
- Models: Los modelos de la capa front se han dise�ado para poder extenderse e implementar est�ndares de comunicaci�n conocidos. En este proyecto se ha usado Json-Api, si bien solo se han tenido en cuenta las propiedades m�s b�sicas y necesarias (la propiedad "Data"), sin entrar en complejidades como la propiedad "Link" para implementar HATEOAS. M�s informaci�n en [json:api](https://jsonapi.org/).
El patr�n que se ha seguido para los modelos de la API es el de Request/Response, que determina que toda acci�n debe recibir un Request y devolver un Response. Este patr�n es bien conocido en APIs como la de Amazon Web Services. Una de las ventajas es que, para apis m�s complejas, el desarrollador no tiene que pensar en nombres para los modelos de esta capa, simplemente seguir el convenio "{ActionName}Request" y "{ActionName}Response". Toda acci�n debe tener su propio Request y Response intentando prevenir la reutilizaci�n, ya que en este caso podr�a haber confusiones e introducir breacking changes por despite. Tambi�n tiene sus inconvenientes, la deserializaci�n y mapeo hacia la capa de negocio suelen ser algo m�s complejas, y en apis basadas en ofrecer verbos CRUD para cada recurso podr�a desconcertar a los usuarios en algunos casos como el verbo Delete, que usualmente devuelve 204, o Create, que en mucho casos no devuelve m�s que un Location header para notificar d�nde se ha creado el nuevo recurso.
Por �ltimo, el proyecto GoalSystems.WebApi.Host simplemente referencia al proyecto Mvc, levanta el servidor web y hereda la clase Startup de BaseStartup.

4. Infrastructure: Servicios y clases de proposito general o elementos necesarios en todas o varias capas. En este proyecto conviven dos servicios: BackgroundWorkerService y MediatorService. Adem�s aqu� puede encontrarse la clase ConfigurationApp concebida para deserializar el archivo de configuraci�n appsettings.json.
- BackgroundWorkerService: La interfaz del servicio solo expone un m�todo, el �nico que es necesario para los requerimientos. En este caso se ha utilizado la herramienta HangFire para trabajar con procesos en background y procesos planificados necesarios para el evento de Fecha de Expiraci�n. Tambi�n expone un m�todo de extensi�n para inyectar todas las dependencias. En el futuro, si hiciera falta utilizar otra herramienta distinta bastar�a con crear una nueva implementaci�n y un nuevo m�todo para inyectarlo.
- MediatorService: Un servicio para utilizar el patr�n Mediator, la implementaci�n se ha realizado utilizando la herramienta MediatR, aunque hab�a otras opciones, como MicroBus.

## Tests
Cada capa deber�a tener un proyecto por cada tipo de tests que se deban aplicar: Unit, Integration, Functional, Performance, etc... Solo se han creado unos cuantos por ejemplo.

GoalSystems.WebApi.Mvc.Tests.Unit solo realiza una comprobaci�n para ver si los mappers est�n correctamente configurados. Aqu� deber�an ir otros tests para comprobar que todas los servicios inyectados son los correctos, el patron IOption para acceder a la configuraci�n, y especialmente m�todos que impliquen una complejidad algor�tmica m�s alta. Otra opci�n es a�adir las pruebas de las acciones de los controladores mockeando con herramientas como Moq.Net sus servicios y comprobando que se devuelven los c�digos y resultados correctos. Sin embargo, personalmente en este caso pienso que los tests funcionales aportan mucho m�s valor para testear una API, y teniendo herramientas para el Continuous Integration que nos permiten levantar contenedores para la persistencia, simuladores de storage, servidores ftp, buses e incluso clusteres spark... Un buen set de test funcionales probando la misma API usando TestServer de .Net nos dar�a una gran covertura, m�s valor y menos c�digo  que mantener (ya que los test unitarios no ser�an necesarios de realizar en muchos casos).

GoalSystems.WebApi.Mvc.Tests.Functional, usando xUnit, FluentAssertions y TestServer, aqu� crear�a varios tests y theories por cada endpoint comprobando que los c�digos de estado HTTP son los correctos, el resultado de la petici�n as� como headers y otros metadatos. Tambi�n es un buen lugar para consultar el estado de la persistencia usando los servicios y comprobar que todo est� como debe estar.

Otro proyecto que no he desarrollado ser�a el de las pruebas de integraci�n de la capa Business, aunque tambi�n podr�an ser test unitarios. De nuevo, teniendo la facilidad de usar contenedores, las pruebas de integraci�n suelen ser m�s r�pidasd de programar ya que solo hay que mockear servicios que no puedas resolver con un contenedores. Las unitarias por el contrario, deben ser totalmente aisladas y portanto implican mockear todas las dependencias. En estos casos es cuesti�n de buscar un consenso, un punto intermedio entre mantenibilidad del c�digo y velocidad... Si se est� testeando un repositorio que hace un CRUD a base de datos, el hecho de no tener que mockear las herramientas encargadas de persistir (ADO.Net, EF, SDK de una NoSQL, etc) nos da un plus en velocidad de desarrollo de tests, y el tiempo que tardan en pasar usando containers con los recursos de la misma m�quina suele ser despreciable.

**Las pruebas que faltar�an a nivel funcional son:**
- Verbo DELETE
  - Devolver 200 con el item borrado
  - Devolver 404 si el par�metro de entrada no es encontrado en la base de datos
  - Devolver un 400 si el par�metro de entrada es inv�lido (null, cadena vac�a o mayor que 100 chars)

- Verbo GET
  - Devolver 200 con el item borrado
  - Devolver 404 si el par�metro de entrada no es encontrado en la base de datos
  - Devolver un 400 si el par�metro de entrada es inv�lido (null, cadena vac�a o mayor que 100 chars)

- Verbo PUT
  - Devolver 400 si los par�metros de entrada no son v�lidos.

**Pruebas unitarias para la capa de infraestructura**
- BackgroundWorkerService
  - Comprobar que el schedule funciona correctamente
  - Comprobar que lanza Argument exceptions si no se llama con los par�metros correctos al m�todo.
  - A tener en cuenta: Mockear la librer�a Hangfire interna para emular el comportamiento con Moq.Net.
- MediatorService => pruebas similares a las de Hangfire

**Pruebas de integraci�n de ItemServce**
Aunque pr�cticamente toda la casu�stica est� probada con las de integraci�n, se podr�a probar las llamadasa los distintos m�todos del servicio y realizar los Asserts correspondientes haciendo la consulta a base de datos directamente con EF para comprobar que la persistencia est� como se supone que debe estar.

## Atajos
### Entity Framework en servicios de Business
**No se han implementado repositorios** como wrappers the EF por dos motivos. Primero, por tiempo. Implementar un repositorio es otra capa m�s para abstraer el ORM. Segundo, si estamos en el contexto de microservicios, una soluci�n con gran complejidad... a�ade costes de mantenimiento. Adem�s, Entity Framework, por definici�n, no es solo un ORM, sino tambi�n una implementaci�n de Repository Pattern m�s Unit of Work. En otras palabras, los DbSets del contexto son los repositorios, y el SaveChanges es el "Unit of Work" (simplificando bastante). Hoy en d�a es muy confuso la palabra ORM porque, por l�gica, se aplica a modelos de datos relacionales. Sin embargo si no pensaramos en EF como un ORM, si no solo como Patr�n repositorio y Unit of Work, nos dar�amos cuenta que podemos implementar a mano cualquier proveedor. Esto significar�a implementar las funciones que eval�an el arbol de expresi�n generado por linq cuando sea requerido (SaveChanges, SingleAsync, ToList, etc). Un caso curioso es que actualmente existen pr�cticamente todos los proveedores imaginables compatibles con EF y Linq, donde se incluyen todas las bases de datos documentales famosas, sistemas de persistencia como Box, GDrive, Apis externas como MailChimp, etc... todo eso usando EF (no como ORM). M�s datos en [CDATA](https://www.cdata.com/drivers/).

### Migraciones
Cada vez que la aplicaci�n se levanta con docker-compose, se regenera la base de datos. Esto simula el comportamiento de localdb, un comportamiento t�pico para desarrollar y partir siempre desde el mismo estado. No he generado la migraci�n inicial porque no es necesaria ya que siempre se recrea la base de datos en el proyecto Data. Para hacerlo ser�a lanzando el comando:

```Cmd
 dotnet ef migrations add Initial <par�metros para definir d�nde quieres los modelos y el contexto>.
```

Esto es necesario cambiarlo para utilizar migraciones. Aunque lo he gestionado desde c�digo, lo suyo es crear la base de datos o aplicar migraciones en el proceso de deploy cuando el resto de recursos se hayan desplegado correctamente. Si algo fuera mal, los metodos Down de las migraciones se ejecutar�an para dejar la persistencia en el estado anterior.

### Handlers del patr�n Mediator
Como no hab�a especificaciones, simplemente he puesto comentarios en lo que har�a el mediador. Seg�n el enunciado se habla de publicar eventos. Si nos refirieramos a eventos de programaci�n distribuida para comunicar distintos artefactos software independientes, lo suyo ser�a utilizar un Bus como Azure ServiceBus o AWS SNS.

### InMemory
Aunque empec� utilizando una base de datos InMemory, finalmente decid� utilizar la soluci�n integrada con Visual Studio para generar una soluci�n multi-container. La base de datos se despliega junto con la WebApi. Sin embargo hay otros servicios que trabajan en memoria pero es importante saber las implicaciones.
- Hangfire: si no se persiste y la aplicaci�n para, los trabajos planificados como los de la fecha de expiraci�n, o cualquier trabajo en background que no haya finalizado, se perder�an. Lo suyo ser�a montar un contenedor m�s con una **Redis** o **MongoDB** para guardar el estado de HangFire.
- Mediator: Lo mismo, una posible soluci�n para prevenir los problemas del 2PC ser�a persistir en la primera transacci�n el estado de la publicaci�n junto con el objeto (�se ha publicado o no?). Tambien cabr�a la posibilidad de guardar este estado si la persistencia se realizara usando EventSourcing, guardando en un par�metro extra de cada estado o cambio del agregado si se ha publicado el evento correspondiente o no.

## Problemas conocidos o por mejorar
Algunos problemillas que he encontrado por el camino, pero que no he resuelto porque no eran cr�ticos.
### Docker-Compose
Idealmente lo suyo ser�a poder llamar al comando docker-compose up -d para probar la aplicaci�n sin necesidad de Visual Studio. Sin embargo, por la forma en la que VS autogenera el DockerFile parece no funcionar ex�ctamente igual y se queja al realizar la build del proyecto Mvc. 

### Dependencia con MediatR
Uno de los objetivos de crear una abstracci�n sobre las herramientas para aplicar el patr�n mediador era no depender de nada sobre la herramienta. Al final no he tenido tiempo de aislarlo correctamente y como workarround he heredado IMessage directamente de MedatR.INotification. Si no, la herramienta provoca un herror en tiempo de ejecucci�n debido a que el mensaje no implementa su interfaz. Esto me hace pensar que hubiera sido mejor opci�n utilizar MicroBus, que no te obliga a implementar ninguna interfaz. �Porqu� he usado MediatR? Porque no lo hab�a utilizado antes y era una ocasi�n perfecta para ver qu� pod�a aportar y que handicaps tiene.

### Pruebas unitarias, de integraci�n, funcionales, performance, etc
No he dedicado demasiado tiempo a crear pruebas dado que con Swagger pueden realizarse pruebas de humo f�cilmente. Especialmente con esta casu�stica tan sencilla. 

La estructura para la pruebas que considero ideal es crear proyectos de cada tipo de pruebas por separado y para cada capa.
El motivo de hacerlo as� es porque puede disminuir bastante los tiempos de CI.
Primero har�amos un dotnet vstest de los proyectos unitarios ya que son los m�s r�pidos y si alguno falla, no hay que perder el tiempo con el resto.
Despu�s los de integraci�n, y finalmente funcionales.
Los tests de performance para mi gusto ir�an al final ya que seg�n lo concienzudos que sean pueden generar bastante overtime.

No obstante, utilizando el paralelismo de las herramientas de CI/CD que existen en el mercado, se pueden lanzar cada test en distintos "Stages".
Por ejemplo, DevOps Pipelines tiene el sistema de multistage pipelines, que te permite paralelizar todo e ir ara�ando minutos al proceso construir los artefactos.

Otro tipo de tests, como los de smoke ya necesitar�an que el artefacto est� desplegado, por lo que no los tendr� en cuenta.

### Tipo de elemento
La propiedad de tipo de elemento se ha dise�ado como tipo byte. Estos tipos suelen ser bastante est�tico as� que en estos casos lo suyo es crear un enumerado. Una opci�n es crear uno solo en la capa de infraestructura para ser reutilizado. Si cada elemento en base de datos tuviera alguna constraint, como ser una foreign key por ejemplo, lo separar�a en dos enumerados para no exponer Ids de persistencia, uno para la entidad de EF y otro para el resto de la aplicaci�n. El cambio de valores habr�a que tratarlo en los mappers de la capa Business.

### Comentarios
No he a�adido la mayor�a de comentarios en las clases y miembros p�blicos. Es especialmente importante documentar los controladores, acciones y sus modelos, ya que herramientas que usan OpenApi specs pueden documentar todo desde el c�digo y as� el desarrollador no necesita estar yendo a distintas fuentes para documentar o codificar..

### Otras arquitecturas
Otra opci�n que podr�a aplicar ser�a aplicar EventSourcing y CQRS, donde cualquier interacci�n con el sistema es o una Query, un Command o un Event. EventSourcing nos permitir�a tambi�n saber un hist�rico completo de los estados por los que ha pasado cada agregado.

### CI/CD
No he creado las definiciones de CI y CD tampoco. No obstante el orden de ambos procesos ser�a el siguiente:
Continuous Integration:
1. Obtener e instalar dependencias para el Agent donde se est� ejecutando el proceso de CI
2. Restaurar paquetes y realizar la build de cada proyecto implicado en el artefacto.
3. Ejecutar las pruebas en el orden definido previamente.
5. Llamar a otras herramientas como: Generar artefacto de documentaci�n html, markdown..., auditorias como SonarCloud, etc.
4. Si hubiera que realizar NuGet packages, se lanar�a la instrucci�n dotnet pack para la versi�n de prerelease (yo prefiero generar de paso la de release tambi�n en este caso, por si se pasara a producci�n posteriormente).
5. Publicar el artefacto y lanzar el Deploy

Continuous Deployment:
1. Obtener e instalar dependencias para el Agent donde se est� ejecutando el proceso de DI
2. Obtener los binarios previamente generados.
3. Publicar todos los recursos en los servicios utilizados
   - WebApi en un AppService / Azure Function / AKS / ACI...
   - Documentaci�n en un BlobStorage + CDN, o en una Wiki como DevOps
   - Publish de los paquetes NuGet seg�n el entorno. Si estamos en Dev publicariamos las versiones prerelease. Si estamos en Prod las versiones Release (Lo mismo con cualquier otro entorno)
4. Publicar resultados del proceso, documentos de auditor�a, etc.



