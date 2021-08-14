# API : Documentation Technique

## Introduction

L'API REST du projet GameAdsStudio a été réalisée en C# avec le framework .NET 5.0, l'intéraction avec les données est réalisée grâce a l'Entity Framework.

### Pré-requis
#### Software
1. [Docker](https://www.docker.com/)
2. [Dotnet sdk 5.0+](https://dotnet.microsoft.com/download)
3. [Entity Framework CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

Recommandé:
1. [Postman](https://www.postman.com/)

#### Environnement
A la racine du repository se trouve un fichier `.env.exemple`, il faut copier son contenu dans un nouveau fichier .env, normalement des valeurs par défaut sont déjà présentes afin de faciliter le lancement mais il n'est pas une mauvaise idée de modifier les identifiants ainsi que les secrets lorsque vous développez sur votre machine.
Si jamais il manque une valeur par défaut pour une des variables, alors complétez par une valeur adéquate dans votre fichier `.env`.

### Lancer l'API en local
Afin de lancer l'API en local, nous avons deux docker-compose à disposition :

```console
docker-compose up --build
```
Qui lance:
- API
 - Base de données postgresql
 - Environnement de production
   - Données persistantes grâce à un volume

```console
docker-compose -f docker-compose.dev.yml up --build
```
Qui lance:
 - API
 - Base de données postgresql
 - pgAdmin
 - Environnement de dev
   - Live-reload de l'api à chaque changement grâce à un volume entre l'host et le service
   - Reset de la base de données à chaque lancement/reload de l'API
   - Visualisation des données en base grâce au pgAdmin

Avec les valeurs par défaut contenues dans le fichier `.env.exemple`, vous devriez avoir accès à l'API à l'adresse `127.0.0.1:5000`, nous vous conseillons vivement d'utiliser [Postman](https://www.postman.com/) pour intéragir avec l'API.

### Organisation du code
Actuellement nous suivons un pattern standard pour le design d'API REST, avec les abstraction suivantes :
- API layer : Entrée et sortie des données, ici les controllers dans le dossier `/api/Controllers`
- Service layer : Traitement et logique, ici les business logic dans le dossier `/api/Business`
- Repository layer : Intéractions avec la base de données, ici les repositories dans le dossier `/api/Repositories`

Explication des dossiers de l'API :
- `/api/Business` : Contient toute la logique de notre API, comme par exemple la vérification des droits des utilisateurs lors de la modification d'une ressource ou le traitement d'une nouvelle ressource avant sa sauvegarde.
- `/api/Configuration` : Contient la configuration pour notre API, peu/pas utilisée pour le moment.
- `/api/Context` : Contient le "contexte", la configuration pour l'intéraction avec la base de données pour l'Entity Framework.
- `/api/Controllers` : Contient les controllers de notre API, qui gèrent l'entrée et la sortie des données.
- `/api/DataAnnotations` : Contient des annotations de data qui nous permettent de valider par exemple des champs avec de la logique custom.
- `/api/Enums` : Contient les enums utilisées à travers l'API.
- `/api/Errors` : Contient la classe des erreurs custom que l'on utilise à travers l'API.
- `/api/Helpers` : Contient des classes générales pratiques comme par exemple un encrypteur/vérificateur de mot de passe ou une classe pour gérer la pagination.
- `/api/Mappings` : Contient la configuration du package "AutoMapper" qui nous permet de mapper des classes à partir d'autre classes, ici on cela nous est utile pour passer des entités aux dtos et vice-versa.
- `/api/Middlewares` : Contient les middlewares de l'API comme par exemple le middleware qui va catch les erreurs qui sont throw afin d'afficher un message pour l'utilisateur de l'API.
- `/api/Migrations` : Contient les migrations pour la base de données, pas besoin d'y toucher les fichiers sont modifiés automatiquement lors de l'utilisation du CLI de l'Entity Framework.
- `/api/Models` : Contient les entités et les dtos de ressources, les entités correspondant aux données stockées en base, et les dto correspondant aux données qui transitent vers et depuis l'API.
- `/api/Properties` : Contient les paramètres de lancement de l'API.
- `/api/Repositories` : Contient les repositories de l'API qui intéragissent via l'Entity Framework avec la base de données.

### Documentation
Une documentation est auto-générée par l'API grâce à Swagger, elle est disponible à l'adresse `127.0.0.1:5000/documentation`. Étant donné que la documentation est auto-générée, il est important d'utiliser les bons types de retours notamment au niveau des controllers afin d'éviter d'avoir une documentation incomplète ou erronée.

### Guidelines
#### Controllers
Lors du rajout d'un controller, toujours préfixer la route du controller d'un numéro de version. eg:
```C#
[Route("/v1/ads")]
[ApiController]
public class AdvertisementController : ControllerBase {}
```
En cas de modification d'un controller existant avec des modifications cassantes, il vaut mieux rajouter un controller avec une version différente afin d'éviter d'imposer des breaking changes pour le front-end.   

Afin d'uniformiser le format du retour des données par les controllers, nous retournons une des deux classes suivantes.
En cas de retour d'une ressource unique: GetDto<Dto>
```C#
return Ok(new GetDto<AdvertisementPublicDto>(_business.GetAdvertisementById(id, currentUser)));
```
En cas de retour de multiples ressources : GetAllDto<Dto>
```C#
return Ok(new GetAllDto<AdvertisementPublicDto>(_business.GetAdvertisements(paging, filters, currentUser)));
```   

Pour la pagination, il existe déjà un DTO de pagination que vous pouvez utiliser :
```C#
public IActionResult GetAll([FromQuery] PagingDto paging, [FromQuery] AdvertisementFiltersDto filters)
```   
Vous pouvez également voir sur le snippet de code précédent que nous utilisons également des Dtos pour la gestion des filtres sur les Get de plusieurs ressources, car ça nous permet d'éviter de rajouter tous les filtres un à un dans la déclaration de fonction.

Lors de l'implémentation d'un nouveau controller nous rajoutons systématiquement les routes d'un CRUD standard, soit :
- GET /ressources/:id
- GET /ressources
- POST /ressources
- PATCH /ressources/:id
- DELETE /ressources/:id

#### Business Logic
Lors de l'implémentation d'une nouvelle classe de Business Logic, il ne faut pas oublier de la déclarer en tant que service scoped dans le fichier `/api/Startup.cs` afin de pouvoir l'injecter automatiquement dans les services en ayant besoin. eg:
```C#
services.AddScoped<IUserBusinessLogic, UserBusinessLogic>();
services.AddScoped<ITagBusinessLogic, TagBusinessLogic>();
```

Il est important de ne jamais renvoyer les entités des ressources vers les controllers mais uniquement les DTOs correspondants, nous utilisons le package AutoMapper pour mapper automatiquement les propriétés des entités vers les DTOs. eg:
```C#
return _mapper.Map(campaign, new CampaignPublicDto());
```
On peut également mapper une liste d'entités vers une list de DTOs. eg:
```C#
return (paging.Page, paging.PageSize, (maxPage / paging.PageSize + 1), _mapper.Map(campaigns, new List<CampaignPublicDto>()));
```
Afin de permettre le mapping il faut cependant préciser les correspondances entre les DTOs et les entités dans le fichier `/api/Mappings/MappingProfile.cs`. eg:
```C#
// Tag
CreateMap<TagModel, TagCreationDto>().ReverseMap();
CreateMap<TagModel, TagPublicDto>().ReverseMap();
CreateMap<TagModel, TagUpdateDto>().ReverseMap().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
```
La méthode `.ReverseMap()` signifie que la correspondance fonctionne dans les deux sens, et la méthode `.ForAllmembers(...)...` nous permet de préciser qu'il est inutile de mapper les valeurs non existantes sur le DTO de patch sur l'entité, afin d'éviter de modifier les champs non voulus par l'utilisteur.

On recommande également de créer une méthode qui renvoie une entité afin d'éviter de la duplication de code inutile, et celà permet également de réutiliser cette méthode depuis d'autres BusinessLogic qui en auraient besoin. eg:
```C#
public CampaignModel GetCampaignModelById(Guid id)
{
  var campaign = _repository.GetCampaignById(id);
  if (campaign == null)
  {
    throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find campaign with Id: {id}");
  }
  return campaign;
}
```

#### Repositories
Pas de guidelines particulières si ce n'est qu'il faut renvoyer des `List<Model>` lors d'un get qui renvoie plusieurs ressources.

Lors de l'implémentation de filtres sur les requêtes, vous pouvez construire une requête petit à petit en fonction des filtres envoyés par l'utilisateur. eg:
```C#
public (List<AdvertisementModel>, int) GetAdvertisements(int offset, int limit, AdvertisementFiltersDto filters)
{
  IQueryable<AdvertisementModel> query = _context.Advertisement.OrderBy(a => a.DateCreation);
  
  if (filters.OrganizationId != Guid.Empty)
  {
    query = query.Where(o => o.Campaign.Organization.Id == filters.OrganizationId);
  }

  if (filters.CampaignId != Guid.Empty)
  {
    query = query.Where(o => o.Campaign.Id == filters.CampaignId);
  }

  return (query.Skip(offset).Take(limit).ToList(), query.Count());
}
```

Afin de pouvoir utiliser le contexte, il ne faut pas oublier de rajouter les informations de votre nouveau modèle dans le fichier `/api/Context/ApiContext.cs`, voir plus bas.

#### Models
Lors de la création d'un nouveau model, il faut toujours au moins 3 champs, Id, DateCreation et DateUpdate. L'Id sera l'identifiant unique de votre ressource créé automatiquement, et les deux autres sont les timestamps également créés et modifiés automatiquement. eg:
```C#
[Table("advertisement")]
public class AdvertisementModel
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public Guid Id { get; set; }
        
  public string Name { get; set; }
        
  public CampaignModel Campaign { get; set; }
        
  public IList<TagModel> Tags { get; set; }
        
  public int AgeMin { get; set; }
        
  public int AgeMax { get; set; }
        
  public DateTimeOffset DateCreation { get; set; }
        
  public DateTimeOffset DateUpdate { get; set; }
}
```
Lors des références aux autres ressources, il faut utiliser impérativement les models et non les DTOs.

#### DTOs
Lors des références aux autres ressources, il faut utiliser impérativement les DTOs et non les models. eg:
```C#
public class AdvertisementPublicDto
{
  public Guid Id { get; set; }
        
  public string Name { get; set; }

  public int AgeMin { get; set; }
        
  public int AgeMax { get; set; }

  public IList<TagPublicDto> Tags { get; set; }

  public DateTimeOffset DateCreation { get; set; }
        
  public DateTimeOffset DateUpdate { get; set; }
}
```

#### Context
Lors du rajout d'un modèle / d'une entité, il faut rajouter la configuration correspondant dans le fichier `/api/Context/ApiContext.cs`. 
```C#
public DbSet<UserModel> User { get; set; }
```
et
```C#
modelBuilder.Entity<UserModel>(entity =>
{
  entity.Property(e => e.Id).ValueGeneratedOnAdd();
  entity.Property(e => e.Role).HasConversion(v => v.ToString(), v => (UserRole) Enum.Parse(typeof(UserRole), v));
});
```
Il est indispensable de préciser le `.ValueGeneratedOnAdd()` sur l'Id pour que le framework génère automatiquement l'Id lors de la création de la ressource.
Le `.HasConversion(...)...` est nécessaire sur les énumérations des entités afin de stocker les énumérations sous forme textuelle en base de données.

#### Enumerations
Lors du rajout d'une énumération, il est obligatoire de préciser la valeur de la première énumération afin de pouvoir forcer les utilisateurs à donner la valeur d'une énum, sans quoi elle se set à 0, soit la valeur de base. eg:
```C#
public enum UserType
{
  Developer = 1,
  Advertiser
}
```
