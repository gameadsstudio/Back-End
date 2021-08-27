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

# API : documentation utilisateur

# Informations

## Authentification

L’utilisateur doit être connecté pour accéder à certains endpoints. Lors de la connexion un jeton d’authentification est envoyé au client et devra être communiqué dans l’en-tête avec la clé [`Authentification`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Authorization) et une valeur avec un type [`Bearer`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Authorization#directives) puis le jeton.

# AdContainer

## `GET /v1/ad-containers/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description                   |
| --- | -------------- | -------- | ------ | ----------------------------- |
| id  | `URL`          | `string` | `oui`  | Identifiant unique (**GUID**) |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "version": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "game": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string"
        },
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z"
      },
      "name": "string",
      "semVer": "string"
    },
    "tags": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "description": "string"
      }
    ],
    "type": "Type2D",
    "aspectRatio": "Aspect1X1",
    "width": 0,
    "height": 0,
    "depth": 0,
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                          |
| ------ | ------------------------------------------------------- | ----------- | --------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [AdContainerPublicDtoGetDto](#schemaadcontainerpublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `PATCH /v1/ad-containers/{id}`

### Paramètres

| Nom         | À envoyer dans | Type                                                    | Requis  | Description |
| ----------- | -------------- | ------------------------------------------------------- | ------- | ----------- |
| id          | `URL`          | `string`                                                | `true`  | aucune      |
| body        | `corps`        | `object`                                                | `false` | aucune      |
| Name        | `corps`        | `string`                                                | `false` | aucune      |
| VersionId   | `corps`        | `string`                                                | `false` | aucune      |
| TagNames    | `corps`        | `[string]`                                              | `false` | aucune      |
| Type        | `corps`        | [AdContainerType](#schemaadcontainertype)               | `false` | aucune      |
| AspectRatio | `corps`        | [AdContainerAspectRatio](#schemaadcontaineraspectratio) | `false` | aucune      |
| Width       | `corps`        | `integer(int32)`                                        | `false` | aucune      |
| Height      | `corps`        | `integer(int32)`                                        | `false` | aucune      |
| Depth       | `corps`        | `integer(int32)`                                        | `false` | aucune      |

#### Valeurs énumérées

| Parameter   | Value      |
| ----------- | ---------- |
| Type        | Type2D     |
| Type        | Type3D     |
| AspectRatio | Aspect1X1  |
| AspectRatio | Aspect4X3  |
| AspectRatio | Aspect16X9 |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "version": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "game": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string"
        },
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z"
      },
      "name": "string",
      "semVer": "string"
    },
    "tags": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "description": "string"
      }
    ],
    "type": "Type2D",
    "aspectRatio": "Aspect1X1",
    "width": 0,
    "height": 0,
    "depth": 0,
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                          |
| ------ | ------------------------------------------------------- | ----------- | --------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [AdContainerPublicDtoGetDto](#schemaadcontainerpublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `DELETE /v1/ad-containers/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/ad-containers`

### Paramètres

| Nom      | À envoyer dans | Type             | Requis  | Description |
| -------- | -------------- | ---------------- | ------- | ----------- |
| Page     | query          | `integer(int32)` | `false` | aucune      |
| PageSize | query          | `integer(int32)` | `false` | aucune      |
| orgId    | query          | `string`         | `true`  | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "version": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "game": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
            "name": "string",
            "status": "string",
            "organization": {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "publicEmail": "string",
              "localization": "string",
              "logoUrl": "string",
              "websiteUrl": "string"
            },
            "dateLaunch": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          },
          "name": "string",
          "semVer": "string"
        },
        "tags": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "description": "string"
          }
        ],
        "type": "Type2D",
        "aspectRatio": "Aspect1X1",
        "width": 0,
        "height": 0,
        "depth": 0,
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z"
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                                |
| ------ | ------------------------------------------------------- | ----------- | --------------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [AdContainerPublicDtoGetAllDto](#schemaadcontainerpublicdtogetalldto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `POST /v1/ad-containers`

### Paramètres

| Nom         | À envoyer dans | Type                                                    | Requis  | Description |
| ----------- | -------------- | ------------------------------------------------------- | ------- | ----------- |
| body        | `corps`        | `object`                                                | `false` | aucune      |
| Name        | `corps`        | `string`                                                | `true`  | aucune      |
| VersionId   | `corps`        | `string(uuid)`                                          | `true`  | aucune      |
| TagNames    | `corps`        | `[string]`                                              | `true`  | aucune      |
| Type        | `corps`        | [AdContainerType](#schemaadcontainertype)               | `false` | aucune      |
| AspectRatio | `corps`        | [AdContainerAspectRatio](#schemaadcontaineraspectratio) | `false` | aucune      |
| Width       | `corps`        | `integer(int32)`                                        | `false` | aucune      |
| Height      | `corps`        | `integer(int32)`                                        | `false` | aucune      |
| Depth       | `corps`        | `integer(int32)`                                        | `false` | aucune      |

#### Valeurs énumérées

| Parameter   | Value      |
| ----------- | ---------- |
| Type        | Type2D     |
| Type        | Type3D     |
| AspectRatio | Aspect1X1  |
| AspectRatio | Aspect4X3  |
| AspectRatio | Aspect16X9 |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "version": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "game": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string"
        },
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z"
      },
      "name": "string",
      "semVer": "string"
    },
    "tags": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "description": "string"
      }
    ],
    "type": "Type2D",
    "aspectRatio": "Aspect1X1",
    "width": 0,
    "height": 0,
    "depth": 0,
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                          |
| ------ | ------------------------------------------------------- | ----------- | --------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [AdContainerPublicDtoGetDto](#schemaadcontainerpublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

# Advertisement

## `GET /v1/ads/{id}`

### Paramètres

| Nom | À envoyer dans | Type           | Requis | Description |
| --- | -------------- | -------------- | ------ | ----------- |
| id  | `URL`          | `string(uuid)` | `true` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `PATCH /v1/ads/{id}`

### Paramètres

| Nom      | À envoyer dans | Type             | Requis  | Description |
| -------- | -------------- | ---------------- | ------- | ----------- |
| id       | `URL`          | `string(uuid)`   | `true`  | aucune      |
| body     | `corps`        | `object`         | `false` | aucune      |
| Name     | `corps`        | `string`         | `false` | aucune      |
| TagNames | `corps`        | `[string]`       | `false` | aucune      |
| AgeMin   | `corps`        | `integer(int32)` | `false` | aucune      |
| AgeMax   | `corps`        | `integer(int32)` | `false` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `DELETE /v1/ads/{id}`

### Paramètres

| Nom | À envoyer dans | Type           | Requis | Description |
| --- | -------------- | -------------- | ------ | ----------- |
| id  | `URL`          | `string(uuid)` | `true` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/ads`

### Paramètres

| Nom            | À envoyer dans | Type             | Requis  | Description |
| -------------- | -------------- | ---------------- | ------- | ----------- |
| Page           | query          | `integer(int32)` | `false` | aucune      |
| PageSize       | query          | `integer(int32)` | `false` | aucune      |
| OrganizationId | query          | `string(uuid)`   | `true`  | aucune      |
| CampaignId     | query          | `string(uuid)`   | `false` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `POST /v1/ads`

### Paramètres

| Nom        | À envoyer dans | Type             | Requis  | Description |
| ---------- | -------------- | ---------------- | ------- | ----------- |
| body       | `corps`        | `object`         | `false` | aucune      |
| CampaignId | `corps`        | `string(uuid)`   | `true`  | aucune      |
| Name       | `corps`        | `string`         | `true`  | aucune      |
| TagNames   | `corps`        | `[string]`       | `false` | aucune      |
| AgeMin     | `corps`        | `integer(int32)` | `true`  | aucune      |
| AgeMax     | `corps`        | `integer(int32)` | `true`  | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

# Campaign

## `GET /v1/campaigns/{id}`

### Paramètres

| Nom | À envoyer dans | Type           | Requis | Description |
| --- | -------------- | -------------- | ------ | ----------- |
| id  | `URL`          | `string(uuid)` | `true` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `PATCH /v1/campaigns/{id}`

### Paramètres

| Nom       | À envoyer dans | Type                                    | Requis  | Description |
| --------- | -------------- | --------------------------------------- | ------- | ----------- |
| id        | `URL`          | `string(uuid)`                          | `true`  | aucune      |
| body      | `corps`        | `object`                                | `false` | aucune      |
| Name      | `corps`        | `string`                                | `false` | aucune      |
| AgeMin    | `corps`        | `string`                                | `false` | aucune      |
| AgeMax    | `corps`        | `string`                                | `false` | aucune      |
| DateBegin | `corps`        | `string(date-time)`                     | `false` | aucune      |
| DateEnd   | `corps`        | `string(date-time)`                     | `false` | aucune      |
| Status    | `corps`        | [CampaignStatus](#schemacampaignstatus) | `false` | aucune      |

#### Valeurs énumérées

| Parameter | Value      |
| --------- | ---------- |
| Status    | Created    |
| Status    | InProgress |
| Status    | Terminated |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `DELETE /v1/campaigns/{id}`

### Paramètres

| Nom | À envoyer dans | Type           | Requis | Description |
| --- | -------------- | -------------- | ------ | ----------- |
| id  | `URL`          | `string(uuid)` | `true` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/campaigns`

### Paramètres

| Nom            | À envoyer dans | Type             | Requis  | Description |
| -------------- | -------------- | ---------------- | ------- | ----------- |
| Page           | query          | `integer(int32)` | `false` | aucune      |
| PageSize       | query          | `integer(int32)` | `false` | aucune      |
| OrganizationId | query          | `string(uuid)`   | `true`  | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `POST /v1/campaigns`

### Paramètres

| Nom            | À envoyer dans | Type                                    | Requis  | Description |
| -------------- | -------------- | --------------------------------------- | ------- | ----------- |
| body           | `corps`        | `object`                                | `false` | aucune      |
| OrganizationId | `corps`        | `string(uuid)`                          | `true`  | aucune      |
| Name           | `corps`        | `string`                                | `true`  | aucune      |
| AgeMin         | `corps`        | `string`                                | `false` | aucune      |
| AgeMax         | `corps`        | `string`                                | `false` | aucune      |
| Type           | `corps`        | `string`                                | `false` | aucune      |
| Status         | `corps`        | [CampaignStatus](#schemacampaignstatus) | `false` | aucune      |
| DateBegin      | `corps`        | `string(date-time)`                     | `false` | aucune      |
| DateEnd        | `corps`        | `string(date-time)`                     | `false` | aucune      |

#### Valeurs énumérées

| Parameter | Value      |
| --------- | ---------- |
| Status    | Created    |
| Status    | InProgress |
| Status    | Terminated |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

# Game

## `POST /v1/games`

### Paramètres

| Nom        | À envoyer dans | Type                | Requis  | Description |
| ---------- | -------------- | ------------------- | ------- | ----------- |
| body       | `corps`        | `object`            | `false` | aucune      |
| OrgId      | `corps`        | `string(uuid)`      | `true`  | aucune      |
| MediaId    | `corps`        | `string(uuid)`      | `true`  | aucune      |
| Name       | `corps`        | `string`            | `true`  | aucune      |
| Status     | `corps`        | `string`            | `true`  | aucune      |
| DateLaunch | `corps`        | `string(date-time)` | `true`  | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
    "name": "string",
    "status": "string",
    "organization": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "publicEmail": "string",
      "localization": "string",
      "logoUrl": "string",
      "websiteUrl": "string"
    },
    "dateLaunch": "2019-08-24T14:15:22Z",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                            |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [GamePublicDtoGetDto](#schemagamepublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/games`

### Paramètres

| Nom      | À envoyer dans | Type             | Requis  | Description |
| -------- | -------------- | ---------------- | ------- | ----------- |
| Page     | query          | `integer(int32)` | `false` | aucune      |
| PageSize | query          | `integer(int32)` | `false` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string"
        },
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z"
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                  |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [GamePublicDtoGetAllDto](#schemagamepublicdtogetalldto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/games/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
    "name": "string",
    "status": "string",
    "organization": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "publicEmail": "string",
      "localization": "string",
      "logoUrl": "string",
      "websiteUrl": "string"
    },
    "dateLaunch": "2019-08-24T14:15:22Z",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                            |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [GamePublicDtoGetDto](#schemagamepublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `PATCH /v1/games/{id}`

### Paramètres

| Nom        | À envoyer dans | Type                | Requis  | Description |
| ---------- | -------------- | ------------------- | ------- | ----------- |
| id         | `URL`          | `string`            | `true`  | aucune      |
| body       | `corps`        | `object`            | `false` | aucune      |
| Name       | `corps`        | `string`            | `false` | aucune      |
| Status     | `corps`        | `string`            | `false` | aucune      |
| MediaId    | `corps`        | `string`            | `false` | aucune      |
| DateLaunch | `corps`        | `string(date-time)` | `false` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `DELETE /v1/games/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

# Organization

## `GET /v1/organizations/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": null
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                              |
| ------ | ------------------------------------------------------- | ----------- | ----------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [ObjectGetDto](#schemaobjectgetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `PATCH /v1/organizations/{id}`

### Paramètres

| Nom                  | À envoyer dans | Type            | Requis  | Description |
| -------------------- | -------------- | --------------- | ------- | ----------- |
| id                   | `URL`          | `string`        | `true`  | aucune      |
| body                 | `corps`        | `object`        | `false` | aucune      |
| Name                 | `corps`        | `string`        | `false` | aucune      |
| PublicEmail          | `corps`        | `string(email)` | `false` | aucune      |
| PrivateEmail         | `corps`        | `string(email)` | `false` | aucune      |
| Localization         | `corps`        | `string`        | `false` | aucune      |
| LogoUrl              | `corps`        | `string`        | `false` | aucune      |
| WebsiteUrl           | `corps`        | `string`        | `false` | aucune      |
| State                | `corps`        | `string`        | `false` | aucune      |
| DefaultAuthorization | `corps`        | `string`        | `false` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "publicEmail": "string",
    "privateEmail": "string",
    "localization": "string",
    "logoUrl": "string",
    "websiteUrl": "string",
    "type": "Developers",
    "state": "Created",
    "defaultAuthorization": "User",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z",
    "campaigns": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "ageMin": "string",
        "ageMax": "string",
        "dateBegin": "2019-08-24T14:15:22Z",
        "dateEnd": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {}
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
              "name": "string",
              "status": "string",
              "dateLaunch": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "versions": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "game": {},
                  "name": "string",
                  "semVer": "string"
                }
              ]
            }
          ]
        },
        "advertisements": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "campaign": {},
            "tags": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "description": "string",
                "adContainers": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "version": {},
                    "organization": {},
                    "tags": [],
                    "type": "Type2D",
                    "aspectRatio": "Aspect1X1",
                    "width": 0,
                    "height": 0,
                    "depth": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "advertisements": [
                  {}
                ],
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ],
            "ageMin": 0,
            "ageMax": 0,
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          }
        ]
      }
    ],
    "users": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "username": "string",
        "email": "string",
        "type": "Developer"
      }
    ],
    "games": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "ageMin": "string",
              "ageMax": "string",
              "dateBegin": "2019-08-24T14:15:22Z",
              "dateEnd": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "advertisements": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "campaign": {},
                  "tags": [
                    {}
                  ],
                  "ageMin": 0,
                  "ageMax": 0,
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ]
            }
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {}
          ]
        },
        "versions": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "game": {},
            "name": "string",
            "semVer": "string"
          }
        ]
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                              |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [OrganizationPrivateDtoGetDto](#schemaorganizationprivatedtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `DELETE /v1/organizations/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/organizations`

### Paramètres

| Nom      | À envoyer dans | Type             | Requis  | Description |
| -------- | -------------- | ---------------- | ------- | ----------- |
| Page     | query          | `integer(int32)` | `false` | aucune      |
| PageSize | query          | `integer(int32)` | `false` | aucune      |
| UserId   | query          | `string(uuid)`   | `false` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string"
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                                  |
| ------ | ------------------------------------------------------- | ----------- | ----------------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [OrganizationPublicDtoGetAllDto](#schemaorganizationpublicdtogetalldto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `POST /v1/organizations`

### Paramètres

| Nom          | À envoyer dans | Type                                        | Requis  | Description |
| ------------ | -------------- | ------------------------------------------- | ------- | ----------- |
| body         | `corps`        | `object`                                    | `false` | aucune      |
| Name         | `corps`        | `string`                                    | `true`  | aucune      |
| PrivateEmail | `corps`        | `string(email)`                             | `true`  | aucune      |
| Type         | `corps`        | [OrganizationType](#schemaorganizationtype) | `false` | aucune      |

#### Valeurs énumérées

| Parameter | Value       |
| --------- | ----------- |
| Type      | Developers  |
| Type      | Advertisers |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "publicEmail": "string",
    "privateEmail": "string",
    "localization": "string",
    "logoUrl": "string",
    "websiteUrl": "string",
    "type": "Developers",
    "state": "Created",
    "defaultAuthorization": "User",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z",
    "campaigns": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "ageMin": "string",
        "ageMax": "string",
        "dateBegin": "2019-08-24T14:15:22Z",
        "dateEnd": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {}
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
              "name": "string",
              "status": "string",
              "dateLaunch": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "versions": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "game": {},
                  "name": "string",
                  "semVer": "string"
                }
              ]
            }
          ]
        },
        "advertisements": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "campaign": {},
            "tags": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "description": "string",
                "adContainers": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "version": {},
                    "organization": {},
                    "tags": [],
                    "type": "Type2D",
                    "aspectRatio": "Aspect1X1",
                    "width": 0,
                    "height": 0,
                    "depth": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "advertisements": [
                  {}
                ],
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ],
            "ageMin": 0,
            "ageMax": 0,
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          }
        ]
      }
    ],
    "users": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "username": "string",
        "email": "string",
        "type": "Developer"
      }
    ],
    "games": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "ageMin": "string",
              "ageMax": "string",
              "dateBegin": "2019-08-24T14:15:22Z",
              "dateEnd": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "advertisements": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "campaign": {},
                  "tags": [
                    {}
                  ],
                  "ageMin": 0,
                  "ageMax": 0,
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ]
            }
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {}
          ]
        },
        "versions": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "game": {},
            "name": "string",
            "semVer": "string"
          }
        ]
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                              |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [OrganizationPrivateDtoGetDto](#schemaorganizationprivatedtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `POST /v1/organizations/{id}/users/{userId}`

### Paramètres

| Nom    | À envoyer dans | Type     | Requis | Description |
| ------ | -------------- | -------- | ------ | ----------- |
| id     | `URL`          | `string` | `true` | aucune      |
| userId | `URL`          | `string` | `true` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "publicEmail": "string",
    "privateEmail": "string",
    "localization": "string",
    "logoUrl": "string",
    "websiteUrl": "string",
    "type": "Developers",
    "state": "Created",
    "defaultAuthorization": "User",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z",
    "campaigns": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "ageMin": "string",
        "ageMax": "string",
        "dateBegin": "2019-08-24T14:15:22Z",
        "dateEnd": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {}
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
              "name": "string",
              "status": "string",
              "dateLaunch": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "versions": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "game": {},
                  "name": "string",
                  "semVer": "string"
                }
              ]
            }
          ]
        },
        "advertisements": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "campaign": {},
            "tags": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "description": "string",
                "adContainers": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "version": {},
                    "organization": {},
                    "tags": [],
                    "type": "Type2D",
                    "aspectRatio": "Aspect1X1",
                    "width": 0,
                    "height": 0,
                    "depth": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "advertisements": [
                  {}
                ],
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ],
            "ageMin": 0,
            "ageMax": 0,
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          }
        ]
      }
    ],
    "users": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "username": "string",
        "email": "string",
        "type": "Developer"
      }
    ],
    "games": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "ageMin": "string",
              "ageMax": "string",
              "dateBegin": "2019-08-24T14:15:22Z",
              "dateEnd": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "advertisements": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "campaign": {},
                  "tags": [
                    {}
                  ],
                  "ageMin": 0,
                  "ageMax": 0,
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ]
            }
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {}
          ]
        },
        "versions": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "game": {},
            "name": "string",
            "semVer": "string"
          }
        ]
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                              |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [OrganizationPrivateDtoGetDto](#schemaorganizationprivatedtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `DELETE /v1/organizations/{id}/users/{userId}`

### Paramètres

| Nom    | À envoyer dans | Type     | Requis | Description |
| ------ | -------------- | -------- | ------ | ----------- |
| id     | `URL`          | `string` | `true` | aucune      |
| userId | `URL`          | `string` | `true` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "publicEmail": "string",
    "privateEmail": "string",
    "localization": "string",
    "logoUrl": "string",
    "websiteUrl": "string",
    "type": "Developers",
    "state": "Created",
    "defaultAuthorization": "User",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z",
    "campaigns": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "ageMin": "string",
        "ageMax": "string",
        "dateBegin": "2019-08-24T14:15:22Z",
        "dateEnd": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {}
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
              "name": "string",
              "status": "string",
              "dateLaunch": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "versions": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "game": {},
                  "name": "string",
                  "semVer": "string"
                }
              ]
            }
          ]
        },
        "advertisements": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "campaign": {},
            "tags": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "description": "string",
                "adContainers": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "version": {},
                    "organization": {},
                    "tags": [],
                    "type": "Type2D",
                    "aspectRatio": "Aspect1X1",
                    "width": 0,
                    "height": 0,
                    "depth": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "advertisements": [
                  {}
                ],
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ],
            "ageMin": 0,
            "ageMax": 0,
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          }
        ]
      }
    ],
    "users": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "username": "string",
        "email": "string",
        "type": "Developer"
      }
    ],
    "games": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "ageMin": "string",
              "ageMax": "string",
              "dateBegin": "2019-08-24T14:15:22Z",
              "dateEnd": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "advertisements": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "campaign": {},
                  "tags": [
                    {}
                  ],
                  "ageMin": 0,
                  "ageMax": 0,
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ]
            }
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {}
          ]
        },
        "versions": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "game": {},
            "name": "string",
            "semVer": "string"
          }
        ]
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                              |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [OrganizationPrivateDtoGetDto](#schemaorganizationprivatedtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/organizations/{id}/users`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "username": "string",
        "email": "string",
        "type": "Developer"
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                  |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [UserPublicDtoGetAllDto](#schemauserpublicdtogetalldto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

# Tag

## `GET /v1/tags/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "description": "string"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                          |
| ------ | ------------------------------------------------------- | ----------- | ----------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [TagPublicDtoGetDto](#schematagpublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `PATCH /v1/tags/{id}`

### Paramètres

| Nom         | À envoyer dans | Type     | Requis  | Description |
| ----------- | -------------- | -------- | ------- | ----------- |
| id          | `URL`          | `string` | `true`  | aucune      |
| body        | `corps`        | `object` | `false` | aucune      |
| Name        | `corps`        | `string` | `false` | aucune      |
| Description | `corps`        | `string` | `false` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "description": "string"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                          |
| ------ | ------------------------------------------------------- | ----------- | ----------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [TagPublicDtoGetDto](#schematagpublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `DELETE /v1/tags/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "name": "string",
  "description": "string"
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                              |
| ------ | ------------------------------------------------------- | ----------- | ----------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [TagPublicDto](#schematagpublicdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/tags`

### Paramètres

| Nom         | À envoyer dans | Type             | Requis  | Description |
| ----------- | -------------- | ---------------- | ------- | ----------- |
| Page        | query          | `integer(int32)` | `false` | aucune      |
| PageSize    | query          | `integer(int32)` | `false` | aucune      |
| noPaging    | query          | boolean          | `false` | aucune      |
| Name        | query          | `string`         | `false` | aucune      |
| Description | query          | `string`         | `false` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "description": "string"
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                |
| ------ | ------------------------------------------------------- | ----------- | ----------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [TagPublicDtoGetAllDto](#schematagpublicdtogetalldto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `POST /v1/tags`

### Paramètres

| Nom         | À envoyer dans | Type     | Requis  | Description |
| ----------- | -------------- | -------- | ------- | ----------- |
| body        | `corps`        | `object` | `false` | aucune      |
| Name        | `corps`        | `string` | `true`  | aucune      |
| Description | `corps`        | `string` | `false` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "description": "string"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                          |
| ------ | ------------------------------------------------------- | ----------- | ----------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [TagPublicDtoGetDto](#schematagpublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

# User

## `GET /v1/users/self`

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "role": "User",
    "username": "string",
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "type": "Developer",
    "alias": "string",
    "phone": "string",
    "level": "string",
    "status": "string",
    "dateStatus": "string",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                              |
| ------ | ------------------------------------------------------- | ----------- | --------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [UserPrivateDtoGetDto](#schemauserprivatedtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/users/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": null
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                              |
| ------ | ------------------------------------------------------- | ----------- | ----------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [ObjectGetDto](#schemaobjectgetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `PATCH /v1/users/{id}`

### Paramètres

| Nom       | À envoyer dans | Type                        | Requis  | Description |
| --------- | -------------- | --------------------------- | ------- | ----------- |
| id        | `URL`          | `string`                    | `true`  | aucune      |
| body      | `corps`        | `object`                    | `false` | aucune      |
| Role      | `corps`        | [UserRole](#schemauserrole) | `false` | aucune      |
| Email     | `corps`        | `string(email)`             | `false` | aucune      |
| Username  | `corps`        | `string`                    | `false` | aucune      |
| FirstName | `corps`        | `string`                    | `false` | aucune      |
| LastName  | `corps`        | `string`                    | `false` | aucune      |
| Password  | `corps`        | `string`                    | `false` | aucune      |
| Alias     | `corps`        | `string`                    | `false` | aucune      |
| Phone     | `corps`        | `string`                    | `false` | aucune      |
| Level     | `corps`        | `string`                    | `false` | aucune      |
| Status    | `corps`        | `string`                    | `false` | aucune      |

#### Valeurs énumérées

| Parameter | Value |
| --------- | ----- |
| Role      | User  |
| Role      | Admin |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "role": "User",
    "username": "string",
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "type": "Developer",
    "alias": "string",
    "phone": "string",
    "level": "string",
    "status": "string",
    "dateStatus": "string",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                              |
| ------ | ------------------------------------------------------- | ----------- | --------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [UserPrivateDtoGetDto](#schemauserprivatedtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `DELETE /v1/users/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/users`

### Paramètres

| Nom      | À envoyer dans | Type             | Requis  | Description |
| -------- | -------------- | ---------------- | ------- | ----------- |
| Page     | query          | `integer(int32)` | `false` | aucune      |
| PageSize | query          | `integer(int32)` | `false` | aucune      |
| Username | query          | `string`         | `false` | aucune      |
| Email    | query          | `string`         | `false` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "username": "string",
        "email": "string",
        "type": "Developer"
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                  |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [UserPublicDtoGetAllDto](#schemauserpublicdtogetalldto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `POST /v1/users`

### Paramètres

| Nom       | À envoyer dans | Type                        | Requis  | Description |
| --------- | -------------- | --------------------------- | ------- | ----------- |
| body      | `corps`        | `object`                    | `false` | aucune      |
| Role      | `corps`        | [UserRole](#schemauserrole) | `false` | aucune      |
| Email     | `corps`        | `string(email)`             | `true`  | aucune      |
| Username  | `corps`        | `string`                    | `true`  | aucune      |
| FirstName | `corps`        | `string`                    | `true`  | aucune      |
| LastName  | `corps`        | `string`                    | `true`  | aucune      |
| Password  | `corps`        | `string`                    | `true`  | aucune      |
| Type      | `corps`        | [UserType](#schemausertype) | `false` | aucune      |

#### Valeurs énumérées

| Parameter | Value      |
| --------- | ---------- |
| Role      | User       |
| Role      | Admin      |
| Type      | Developer  |
| Type      | Advertiser |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "role": "User",
    "username": "string",
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "type": "Developer",
    "alias": "string",
    "phone": "string",
    "level": "string",
    "status": "string",
    "dateStatus": "string",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                              |
| ------ | ------------------------------------------------------- | ----------- | --------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [UserPrivateDtoGetDto](#schemauserprivatedtogetdto) |

---

## `POST /v1/users/login`

### Paramètres

| Nom        | À envoyer dans | Type     | Requis  | Description |
| ---------- | -------------- | -------- | ------- | ----------- |
| body       | `corps`        | `object` | `false` | aucune      |
| Identifier | `corps`        | `string` | `true`  | aucune      |
| Password   | `corps`        | `string` | `true`  | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "token": "string"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                          |
| ------ | ------------------------------------------------------- | ----------- | --------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [UserLoginResponseDtoGetDto](#schemauserloginresponsedtogetdto) |

---

## `GET /v1/users/search/{search}`

### Paramètres

| Nom      | À envoyer dans | Type             | Requis  | Description |
| -------- | -------------- | ---------------- | ------- | ----------- |
| search   | `URL`          | `string`         | `true`  | aucune      |
| Page     | query          | `integer(int32)` | `false` | aucune      |
| PageSize | query          | `integer(int32)` | `false` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "username": "string",
        "email": "string",
        "type": "Developer"
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                  |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [UserPublicDtoGetAllDto](#schemauserpublicdtogetalldto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

# Version

## `GET /v1/versions/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "game": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
      "name": "string",
      "status": "string",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string"
      },
      "dateLaunch": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    },
    "name": "string",
    "semVer": "string"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                  |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [VersionPublicDtoGetDto](#schemaversionpublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `PATCH /v1/versions/{id}`

### Paramètres

| Nom    | À envoyer dans | Type     | Requis  | Description |
| ------ | -------------- | -------- | ------- | ----------- |
| id     | `URL`          | `string` | `true`  | aucune      |
| body   | `corps`        | `object` | `false` | aucune      |
| Name   | `corps`        | `string` | `false` | aucune      |
| SemVer | `corps`        | `string` | `false` | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "game": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
      "name": "string",
      "status": "string",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string"
      },
      "dateLaunch": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    },
    "name": "string",
    "semVer": "string"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                  |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [VersionPublicDtoGetDto](#schemaversionpublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `DELETE /v1/versions/{id}`

### Paramètres

| Nom | À envoyer dans | Type     | Requis | Description |
| --- | -------------- | -------- | ------ | ----------- |
| id  | `URL`          | `string` | `true` | aucune      |

### Réponse(s)

| Statut | Type                                                    | Description | Schéma |
| ------ | ------------------------------------------------------- | ----------- | ------ |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | aucune |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `GET /v1/versions`

### Paramètres

| Nom      | À envoyer dans | Type             | Requis  | Description |
| -------- | -------------- | ---------------- | ------- | ----------- |
| Page     | query          | `integer(int32)` | `false` | aucune      |
| PageSize | query          | `integer(int32)` | `false` | aucune      |
| Name     | query          | `string`         | `false` | aucune      |
| SemVer   | query          | `string`         | `false` | aucune      |
| GameId   | query          | `string(uuid)`   | `true`  | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "game": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
          "name": "string",
          "status": "string",
          "organization": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "publicEmail": "string",
            "localization": "string",
            "logoUrl": "string",
            "websiteUrl": "string"
          },
          "dateLaunch": "2019-08-24T14:15:22Z",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        },
        "name": "string",
        "semVer": "string"
      }
    ]
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                        |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [VersionPublicDtoGetAllDto](#schemaversionpublicdtogetalldto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

## `POST /v1/versions`

### Paramètres

| Nom    | À envoyer dans | Type           | Requis  | Description |
| ------ | -------------- | -------------- | ------- | ----------- |
| body   | `corps`        | `object`       | `false` | aucune      |
| GameId | `corps`        | `string(uuid)` | `true`  | aucune      |
| Name   | `corps`        | `string`       | `true`  | aucune      |
| SemVer | `corps`        | `string`       | `true`  | aucune      |

### Example de réponse(s)

> Réponse `200`

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "game": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
      "name": "string",
      "status": "string",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string"
      },
      "dateLaunch": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    },
    "name": "string",
    "semVer": "string"
  }
}
```

### Réponse(s)

| Statut | Type                                                    | Description | Schéma                                                  |
| ------ | ------------------------------------------------------- | ----------- | ------------------------------------------------------- |
| 200    | [OK](https://tools.ietf.org/html/rfc7231#section-6.3.1) | Succès      | [VersionPublicDtoGetDto](#schemaversionpublicdtogetdto) |

> L’utilisateur doit être authentifité pour pouvoir envoyer cette requête.

---

# Schémas

## AdContainerAspectRatio

### Propriétés

```json
"Aspect1X1"
```

| Nom         | Type     | Requis  | Restriction | Description |
| ----------- | -------- | ------- | ----------- | ----------- |
| *anonymous* | `string` | `false` | aucune      | aucune      |

#### Valeurs énumérées

| Property    | Value      |
| ----------- | ---------- |
| *anonymous* | Aspect1X1  |
| *anonymous* | Aspect4X3  |
| *anonymous* | Aspect16X9 |

## AdContainerModel

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "name": "string",
  "version": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "game": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
      "name": "string",
      "status": "string",
      "dateLaunch": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "privateEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string",
        "type": "Developers",
        "state": "Created",
        "defaultAuthorization": "User",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "campaigns": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "ageMin": "string",
            "ageMax": "string",
            "dateBegin": "2019-08-24T14:15:22Z",
            "dateEnd": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organization": {},
            "advertisements": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "campaign": {},
                "tags": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "description": "string",
                    "adContainers": [],
                    "advertisements": [],
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "ageMin": 0,
                "ageMax": 0,
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ]
          }
        ],
        "users": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "role": "User",
            "username": "string",
            "firstName": "string",
            "lastName": "string",
            "password": "string",
            "email": "string",
            "type": "Developer",
            "alias": "string",
            "phone": "string",
            "level": "string",
            "status": "string",
            "dateStatus": "string",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organizations": [
              {}
            ]
          }
        ],
        "games": [
          {}
        ]
      },
      "versions": [
        {}
      ]
    },
    "name": "string",
    "semVer": "string"
  },
  "organization": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "publicEmail": "string",
    "privateEmail": "string",
    "localization": "string",
    "logoUrl": "string",
    "websiteUrl": "string",
    "type": "Developers",
    "state": "Created",
    "defaultAuthorization": "User",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z",
    "campaigns": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "ageMin": "string",
        "ageMax": "string",
        "dateBegin": "2019-08-24T14:15:22Z",
        "dateEnd": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {},
        "advertisements": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "campaign": {},
            "tags": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "description": "string",
                "adContainers": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "version": {},
                    "organization": {},
                    "tags": [],
                    "type": "Type2D",
                    "aspectRatio": "Aspect1X1",
                    "width": 0,
                    "height": 0,
                    "depth": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "advertisements": [
                  {}
                ],
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ],
            "ageMin": 0,
            "ageMax": 0,
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          }
        ]
      }
    ],
    "users": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "role": "User",
        "username": "string",
        "firstName": "string",
        "lastName": "string",
        "password": "string",
        "email": "string",
        "type": "Developer",
        "alias": "string",
        "phone": "string",
        "level": "string",
        "status": "string",
        "dateStatus": "string",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organizations": [
          {}
        ]
      }
    ],
    "games": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {},
        "versions": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "game": {},
            "name": "string",
            "semVer": "string"
          }
        ]
      }
    ]
  },
  "tags": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "description": "string",
      "adContainers": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "version": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "game": {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
              "name": "string",
              "status": "string",
              "dateLaunch": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "publicEmail": "string",
                "privateEmail": "string",
                "localization": "string",
                "logoUrl": "string",
                "websiteUrl": "string",
                "type": "Developers",
                "state": "Created",
                "defaultAuthorization": "User",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "campaigns": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "ageMin": "string",
                    "ageMax": "string",
                    "dateBegin": "2019-08-24T14:15:22Z",
                    "dateEnd": "2019-08-24T14:15:22Z",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organization": {},
                    "advertisements": []
                  }
                ],
                "users": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "role": "User",
                    "username": "string",
                    "firstName": "string",
                    "lastName": "string",
                    "password": "string",
                    "email": "string",
                    "type": "Developer",
                    "alias": "string",
                    "phone": "string",
                    "level": "string",
                    "status": "string",
                    "dateStatus": "string",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organizations": []
                  }
                ],
                "games": [
                  {}
                ]
              },
              "versions": [
                {}
              ]
            },
            "name": "string",
            "semVer": "string"
          },
          "organization": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "publicEmail": "string",
            "privateEmail": "string",
            "localization": "string",
            "logoUrl": "string",
            "websiteUrl": "string",
            "type": "Developers",
            "state": "Created",
            "defaultAuthorization": "User",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "campaigns": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "ageMin": "string",
                "ageMax": "string",
                "dateBegin": "2019-08-24T14:15:22Z",
                "dateEnd": "2019-08-24T14:15:22Z",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "organization": {},
                "advertisements": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "campaign": {},
                    "tags": [],
                    "ageMin": 0,
                    "ageMax": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ]
              }
            ],
            "users": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "role": "User",
                "username": "string",
                "firstName": "string",
                "lastName": "string",
                "password": "string",
                "email": "string",
                "type": "Developer",
                "alias": "string",
                "phone": "string",
                "level": "string",
                "status": "string",
                "dateStatus": "string",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "organizations": [
                  {}
                ]
              }
            ],
            "games": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
                "name": "string",
                "status": "string",
                "dateLaunch": "2019-08-24T14:15:22Z",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "organization": {},
                "versions": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "game": {},
                    "name": "string",
                    "semVer": "string"
                  }
                ]
              }
            ]
          },
          "tags": [],
          "type": "Type2D",
          "aspectRatio": "Aspect1X1",
          "width": 0,
          "height": 0,
          "depth": 0,
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        }
      ],
      "advertisements": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "campaign": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "ageMin": "string",
            "ageMax": "string",
            "dateBegin": "2019-08-24T14:15:22Z",
            "dateEnd": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organization": {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "publicEmail": "string",
              "privateEmail": "string",
              "localization": "string",
              "logoUrl": "string",
              "websiteUrl": "string",
              "type": "Developers",
              "state": "Created",
              "defaultAuthorization": "User",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "campaigns": [
                {}
              ],
              "users": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "role": "User",
                  "username": "string",
                  "firstName": "string",
                  "lastName": "string",
                  "password": "string",
                  "email": "string",
                  "type": "Developer",
                  "alias": "string",
                  "phone": "string",
                  "level": "string",
                  "status": "string",
                  "dateStatus": "string",
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z",
                  "organizations": [
                    {}
                  ]
                }
              ],
              "games": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
                  "name": "string",
                  "status": "string",
                  "dateLaunch": "2019-08-24T14:15:22Z",
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z",
                  "organization": {},
                  "versions": [
                    {}
                  ]
                }
              ]
            },
            "advertisements": [
              {}
            ]
          },
          "tags": [
            {}
          ],
          "ageMin": 0,
          "ageMax": 0,
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        }
      ],
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    }
  ],
  "type": "Type2D",
  "aspectRatio": "Aspect1X1",
  "width": 0,
  "height": 0,
  "depth": 0,
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z"
}
```

| Nom          | Type                                                    | Requis  | Restriction | Description |
| ------------ | ------------------------------------------------------- | ------- | ----------- | ----------- |
| id           | `string(uuid)`                                          | `false` | aucune      | aucune      |
| name         | `string`                                                | `true`  | aucune      | aucune      |
| version      | [VersionModel](#schemaversionmodel)                     | `false` | aucune      | aucune      |
| organization | [OrganizationModel](#schemaorganizationmodel)           | `false` | aucune      | aucune      |
| tags         | [[TagModel](#schematagmodel)] ou `null`                 | `false` | aucune      | aucune      |
| type         | [AdContainerType](#schemaadcontainertype)               | `true`  | aucune      | aucune      |
| aspectRatio  | [AdContainerAspectRatio](#schemaadcontaineraspectratio) | `false` | aucune      | aucune      |
| width        | `integer(int32)`                                        | `false` | aucune      | aucune      |
| height       | `integer(int32)`                                        | `false` | aucune      | aucune      |
| depth        | `integer(int32)`                                        | `false` | aucune      | aucune      |
| dateCreation | `string(date-time)`                                     | `false` | aucune      | aucune      |
| dateUpdate   | `string(date-time)`                                     | `false` | aucune      | aucune      |

## AdContainerPublicDto

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "name": "string",
  "version": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "game": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
      "name": "string",
      "status": "string",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string"
      },
      "dateLaunch": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    },
    "name": "string",
    "semVer": "string"
  },
  "tags": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "description": "string"
    }
  ],
  "type": "Type2D",
  "aspectRatio": "Aspect1X1",
  "width": 0,
  "height": 0,
  "depth": 0,
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z"
}
```

| Nom          | Type                                                    | Requis  | Restriction | Description |
| ------------ | ------------------------------------------------------- | ------- | ----------- | ----------- |
| id           | `string(uuid)`                                          | `false` | aucune      | aucune      |
| name         | `string` ou `null`                                      | `false` | aucune      | aucune      |
| version      | [VersionPublicDto](#schemaversionpublicdto)             | `false` | aucune      | aucune      |
| tags         | [[TagPublicDto](#schematagpublicdto)] ou `null`         | `false` | aucune      | aucune      |
| type         | [AdContainerType](#schemaadcontainertype)               | `false` | aucune      | aucune      |
| aspectRatio  | [AdContainerAspectRatio](#schemaadcontaineraspectratio) | `false` | aucune      | aucune      |
| width        | `integer(int32)`                                        | `false` | aucune      | aucune      |
| height       | `integer(int32)`                                        | `false` | aucune      | aucune      |
| depth        | `integer(int32)`                                        | `false` | aucune      | aucune      |
| dateCreation | `string(date-time)`                                     | `false` | aucune      | aucune      |
| dateUpdate   | `string(date-time)`                                     | `false` | aucune      | aucune      |

## AdContainerPublicDtoDataAllDto

### Propriétés

```json
{
  "pageIndex": 0,
  "itemsPerPage": 0,
  "totalPages": 0,
  "currentItemCount": 0,
  "items": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "version": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "game": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
          "name": "string",
          "status": "string",
          "organization": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "publicEmail": "string",
            "localization": "string",
            "logoUrl": "string",
            "websiteUrl": "string"
          },
          "dateLaunch": "2019-08-24T14:15:22Z",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        },
        "name": "string",
        "semVer": "string"
      },
      "tags": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "description": "string"
        }
      ],
      "type": "Type2D",
      "aspectRatio": "Aspect1X1",
      "width": 0,
      "height": 0,
      "depth": 0,
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    }
  ]
}
```

| Nom              | Type                                                            | Requis  | Restriction | Description |
| ---------------- | --------------------------------------------------------------- | ------- | ----------- | ----------- |
| pageIndex        | `integer(int32)`                                                | `false` | aucune      | aucune      |
| itemsPerPage     | `integer(int32)`                                                | `false` | aucune      | aucune      |
| totalPages       | `integer(int32)`                                                | `false` | aucune      | aucune      |
| currentItemCount | `integer(int32)`                                                | `false` | aucune      | aucune      |
| items            | [[AdContainerPublicDto](#schemaadcontainerpublicdto)] ou `null` | `false` | aucune      | aucune      |

## AdContainerPublicDtoGetAllDto

### Propriétés

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "version": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "game": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
            "name": "string",
            "status": "string",
            "organization": {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "publicEmail": "string",
              "localization": "string",
              "logoUrl": "string",
              "websiteUrl": "string"
            },
            "dateLaunch": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          },
          "name": "string",
          "semVer": "string"
        },
        "tags": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "description": "string"
          }
        ],
        "type": "Type2D",
        "aspectRatio": "Aspect1X1",
        "width": 0,
        "height": 0,
        "depth": 0,
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z"
      }
    ]
  }
}
```

| Nom  | Type                                                                    | Requis  | Restriction | Description |
| ---- | ----------------------------------------------------------------------- | ------- | ----------- | ----------- |
| data | [AdContainerPublicDtoDataAllDto](#schemaadcontainerpublicdtodataalldto) | `false` | aucune      | aucune      |

## AdContainerPublicDtoGetDto

### Propriétés

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "version": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "game": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string"
        },
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z"
      },
      "name": "string",
      "semVer": "string"
    },
    "tags": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "description": "string"
      }
    ],
    "type": "Type2D",
    "aspectRatio": "Aspect1X1",
    "width": 0,
    "height": 0,
    "depth": 0,
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

| Nom  | Type                                                | Requis  | Restriction | Description |
| ---- | --------------------------------------------------- | ------- | ----------- | ----------- |
| data | [AdContainerPublicDto](#schemaadcontainerpublicdto) | `false` | aucune      | aucune      |

## AdContainerType

### Propriétés

```json
"Type2D"
```

| Nom         | Type     | Requis  | Restriction | Description |
| ----------- | -------- | ------- | ----------- | ----------- |
| *anonymous* | `string` | `false` | aucune      | aucune      |

#### Valeurs énumérées

| Property    | Value  |
| ----------- | ------ |
| *anonymous* | Type2D |
| *anonymous* | Type3D |

## AdvertisementModel

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "name": "string",
  "campaign": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "ageMin": "string",
    "ageMax": "string",
    "dateBegin": "2019-08-24T14:15:22Z",
    "dateEnd": "2019-08-24T14:15:22Z",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z",
    "organization": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "publicEmail": "string",
      "privateEmail": "string",
      "localization": "string",
      "logoUrl": "string",
      "websiteUrl": "string",
      "type": "Developers",
      "state": "Created",
      "defaultAuthorization": "User",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z",
      "campaigns": [
        {}
      ],
      "users": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "role": "User",
          "username": "string",
          "firstName": "string",
          "lastName": "string",
          "password": "string",
          "email": "string",
          "type": "Developer",
          "alias": "string",
          "phone": "string",
          "level": "string",
          "status": "string",
          "dateStatus": "string",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "organizations": [
            {}
          ]
        }
      ],
      "games": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
          "name": "string",
          "status": "string",
          "dateLaunch": "2019-08-24T14:15:22Z",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "organization": {},
          "versions": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "game": {},
              "name": "string",
              "semVer": "string"
            }
          ]
        }
      ]
    },
    "advertisements": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "campaign": {},
        "tags": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "description": "string",
            "adContainers": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "version": {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "game": {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
                    "name": "string",
                    "status": "string",
                    "dateLaunch": "2019-08-24T14:15:22Z",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organization": {},
                    "versions": []
                  },
                  "name": "string",
                  "semVer": "string"
                },
                "organization": {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "publicEmail": "string",
                  "privateEmail": "string",
                  "localization": "string",
                  "logoUrl": "string",
                  "websiteUrl": "string",
                  "type": "Developers",
                  "state": "Created",
                  "defaultAuthorization": "User",
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z",
                  "campaigns": [
                    {}
                  ],
                  "users": [
                    {}
                  ],
                  "games": [
                    {}
                  ]
                },
                "tags": [
                  {}
                ],
                "type": "Type2D",
                "aspectRatio": "Aspect1X1",
                "width": 0,
                "height": 0,
                "depth": 0,
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ],
            "advertisements": [
              {}
            ],
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          }
        ],
        "ageMin": 0,
        "ageMax": 0,
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z"
      }
    ]
  },
  "tags": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "description": "string",
      "adContainers": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "version": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "game": {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
              "name": "string",
              "status": "string",
              "dateLaunch": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "publicEmail": "string",
                "privateEmail": "string",
                "localization": "string",
                "logoUrl": "string",
                "websiteUrl": "string",
                "type": "Developers",
                "state": "Created",
                "defaultAuthorization": "User",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "campaigns": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "ageMin": "string",
                    "ageMax": "string",
                    "dateBegin": "2019-08-24T14:15:22Z",
                    "dateEnd": "2019-08-24T14:15:22Z",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organization": {},
                    "advertisements": []
                  }
                ],
                "users": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "role": "User",
                    "username": "string",
                    "firstName": "string",
                    "lastName": "string",
                    "password": "string",
                    "email": "string",
                    "type": "Developer",
                    "alias": "string",
                    "phone": "string",
                    "level": "string",
                    "status": "string",
                    "dateStatus": "string",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organizations": []
                  }
                ],
                "games": [
                  {}
                ]
              },
              "versions": [
                {}
              ]
            },
            "name": "string",
            "semVer": "string"
          },
          "organization": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "publicEmail": "string",
            "privateEmail": "string",
            "localization": "string",
            "logoUrl": "string",
            "websiteUrl": "string",
            "type": "Developers",
            "state": "Created",
            "defaultAuthorization": "User",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "campaigns": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "ageMin": "string",
                "ageMax": "string",
                "dateBegin": "2019-08-24T14:15:22Z",
                "dateEnd": "2019-08-24T14:15:22Z",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "organization": {},
                "advertisements": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "campaign": {},
                    "tags": [],
                    "ageMin": 0,
                    "ageMax": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ]
              }
            ],
            "users": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "role": "User",
                "username": "string",
                "firstName": "string",
                "lastName": "string",
                "password": "string",
                "email": "string",
                "type": "Developer",
                "alias": "string",
                "phone": "string",
                "level": "string",
                "status": "string",
                "dateStatus": "string",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "organizations": [
                  {}
                ]
              }
            ],
            "games": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
                "name": "string",
                "status": "string",
                "dateLaunch": "2019-08-24T14:15:22Z",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "organization": {},
                "versions": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "game": {},
                    "name": "string",
                    "semVer": "string"
                  }
                ]
              }
            ]
          },
          "tags": [
            {}
          ],
          "type": "Type2D",
          "aspectRatio": "Aspect1X1",
          "width": 0,
          "height": 0,
          "depth": 0,
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        }
      ],
      "advertisements": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "campaign": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "ageMin": "string",
            "ageMax": "string",
            "dateBegin": "2019-08-24T14:15:22Z",
            "dateEnd": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organization": {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "publicEmail": "string",
              "privateEmail": "string",
              "localization": "string",
              "logoUrl": "string",
              "websiteUrl": "string",
              "type": "Developers",
              "state": "Created",
              "defaultAuthorization": "User",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "campaigns": [
                {}
              ],
              "users": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "role": "User",
                  "username": "string",
                  "firstName": "string",
                  "lastName": "string",
                  "password": "string",
                  "email": "string",
                  "type": "Developer",
                  "alias": "string",
                  "phone": "string",
                  "level": "string",
                  "status": "string",
                  "dateStatus": "string",
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z",
                  "organizations": [
                    {}
                  ]
                }
              ],
              "games": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
                  "name": "string",
                  "status": "string",
                  "dateLaunch": "2019-08-24T14:15:22Z",
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z",
                  "organization": {},
                  "versions": [
                    {}
                  ]
                }
              ]
            },
            "advertisements": [
              {}
            ]
          },
          "tags": [],
          "ageMin": 0,
          "ageMax": 0,
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        }
      ],
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    }
  ],
  "ageMin": 0,
  "ageMax": 0,
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z"
}
```

| Nom          | Type                                    | Requis  | Restriction | Description |
| ------------ | --------------------------------------- | ------- | ----------- | ----------- |
| id           | `string(uuid)`                          | `false` | aucune      | aucune      |
| name         | `string` ou `null`                      | `false` | aucune      | aucune      |
| campaign     | [CampaignModel](#schemacampaignmodel)   | `false` | aucune      | aucune      |
| tags         | [[TagModel](#schematagmodel)] ou `null` | `false` | aucune      | aucune      |
| ageMin       | `integer(int32)`                        | `false` | aucune      | aucune      |
| ageMax       | `integer(int32)`                        | `false` | aucune      | aucune      |
| dateCreation | `string(date-time)`                     | `false` | aucune      | aucune      |
| dateUpdate   | `string(date-time)`                     | `false` | aucune      | aucune      |

## CampaignModel

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "name": "string",
  "ageMin": "string",
  "ageMax": "string",
  "dateBegin": "2019-08-24T14:15:22Z",
  "dateEnd": "2019-08-24T14:15:22Z",
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z",
  "organization": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "publicEmail": "string",
    "privateEmail": "string",
    "localization": "string",
    "logoUrl": "string",
    "websiteUrl": "string",
    "type": "Developers",
    "state": "Created",
    "defaultAuthorization": "User",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z",
    "campaigns": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "ageMin": "string",
        "ageMax": "string",
        "dateBegin": "2019-08-24T14:15:22Z",
        "dateEnd": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {},
        "advertisements": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "campaign": {},
            "tags": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "description": "string",
                "adContainers": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "version": {},
                    "organization": {},
                    "tags": [],
                    "type": "Type2D",
                    "aspectRatio": "Aspect1X1",
                    "width": 0,
                    "height": 0,
                    "depth": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "advertisements": [
                  {}
                ],
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ],
            "ageMin": 0,
            "ageMax": 0,
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          }
        ]
      }
    ],
    "users": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "role": "User",
        "username": "string",
        "firstName": "string",
        "lastName": "string",
        "password": "string",
        "email": "string",
        "type": "Developer",
        "alias": "string",
        "phone": "string",
        "level": "string",
        "status": "string",
        "dateStatus": "string",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organizations": [
          {}
        ]
      }
    ],
    "games": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {},
        "versions": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "game": {},
            "name": "string",
            "semVer": "string"
          }
        ]
      }
    ]
  },
  "advertisements": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "campaign": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "ageMin": "string",
        "ageMax": "string",
        "dateBegin": "2019-08-24T14:15:22Z",
        "dateEnd": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {}
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
              "name": "string",
              "status": "string",
              "dateLaunch": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "versions": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "game": {},
                  "name": "string",
                  "semVer": "string"
                }
              ]
            }
          ]
        },
        "advertisements": []
      },
      "tags": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "description": "string",
          "adContainers": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "version": {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "game": {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
                  "name": "string",
                  "status": "string",
                  "dateLaunch": "2019-08-24T14:15:22Z",
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z",
                  "organization": {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "publicEmail": "string",
                    "privateEmail": "string",
                    "localization": "string",
                    "logoUrl": "string",
                    "websiteUrl": "string",
                    "type": "Developers",
                    "state": "Created",
                    "defaultAuthorization": "User",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "campaigns": [],
                    "users": [],
                    "games": []
                  },
                  "versions": [
                    {}
                  ]
                },
                "name": "string",
                "semVer": "string"
              },
              "organization": {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "publicEmail": "string",
                "privateEmail": "string",
                "localization": "string",
                "logoUrl": "string",
                "websiteUrl": "string",
                "type": "Developers",
                "state": "Created",
                "defaultAuthorization": "User",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "campaigns": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "ageMin": "string",
                    "ageMax": "string",
                    "dateBegin": "2019-08-24T14:15:22Z",
                    "dateEnd": "2019-08-24T14:15:22Z",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organization": {},
                    "advertisements": []
                  }
                ],
                "users": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "role": "User",
                    "username": "string",
                    "firstName": "string",
                    "lastName": "string",
                    "password": "string",
                    "email": "string",
                    "type": "Developer",
                    "alias": "string",
                    "phone": "string",
                    "level": "string",
                    "status": "string",
                    "dateStatus": "string",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organizations": []
                  }
                ],
                "games": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
                    "name": "string",
                    "status": "string",
                    "dateLaunch": "2019-08-24T14:15:22Z",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organization": {},
                    "versions": []
                  }
                ]
              },
              "tags": [
                {}
              ],
              "type": "Type2D",
              "aspectRatio": "Aspect1X1",
              "width": 0,
              "height": 0,
              "depth": 0,
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z"
            }
          ],
          "advertisements": [
            {}
          ],
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        }
      ],
      "ageMin": 0,
      "ageMax": 0,
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    }
  ]
}
```

| Nom            | Type                                                        | Requis  | Restriction | Description |
| -------------- | ----------------------------------------------------------- | ------- | ----------- | ----------- |
| id             | `string(uuid)`                                              | `false` | aucune      | aucune      |
| name           | `string`                                                    | `true`  | aucune      | aucune      |
| ageMin         | `string` ou `null`                                          | `false` | aucune      | aucune      |
| ageMax         | `string` ou `null`                                          | `false` | aucune      | aucune      |
| dateBegin      | `string(date-time)`                                         | `false` | aucune      | aucune      |
| dateEnd        | `string(date-time)`                                         | `false` | aucune      | aucune      |
| dateCreation   | `string(date-time)`                                         | `false` | aucune      | aucune      |
| dateUpdate     | `string(date-time)`                                         | `false` | aucune      | aucune      |
| organization   | [OrganizationModel](#schemaorganizationmodel)               | `false` | aucune      | aucune      |
| advertisements | [[AdvertisementModel](#schemaadvertisementmodel)] ou `null` | `false` | aucune      | aucune      |

## CampaignStatus

### Propriétés

```json
"Created"
```

| Nom         | Type     | Requis  | Restriction | Description |
| ----------- | -------- | ------- | ----------- | ----------- |
| *anonymous* | `string` | `false` | aucune      | aucune      |

#### Valeurs énumérées

| Property    | Value      |
| ----------- | ---------- |
| *anonymous* | Created    |
| *anonymous* | InProgress |
| *anonymous* | Terminated |

## GameModel

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
  "name": "string",
  "status": "string",
  "dateLaunch": "2019-08-24T14:15:22Z",
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z",
  "organization": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "publicEmail": "string",
    "privateEmail": "string",
    "localization": "string",
    "logoUrl": "string",
    "websiteUrl": "string",
    "type": "Developers",
    "state": "Created",
    "defaultAuthorization": "User",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z",
    "campaigns": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "ageMin": "string",
        "ageMax": "string",
        "dateBegin": "2019-08-24T14:15:22Z",
        "dateEnd": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {},
        "advertisements": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "campaign": {},
            "tags": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "description": "string",
                "adContainers": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "version": {},
                    "organization": {},
                    "tags": [],
                    "type": "Type2D",
                    "aspectRatio": "Aspect1X1",
                    "width": 0,
                    "height": 0,
                    "depth": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "advertisements": [
                  {}
                ],
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ],
            "ageMin": 0,
            "ageMax": 0,
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          }
        ]
      }
    ],
    "users": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "role": "User",
        "username": "string",
        "firstName": "string",
        "lastName": "string",
        "password": "string",
        "email": "string",
        "type": "Developer",
        "alias": "string",
        "phone": "string",
        "level": "string",
        "status": "string",
        "dateStatus": "string",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organizations": [
          {}
        ]
      }
    ],
    "games": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {},
        "versions": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "game": {},
            "name": "string",
            "semVer": "string"
          }
        ]
      }
    ]
  },
  "versions": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "game": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "ageMin": "string",
              "ageMax": "string",
              "dateBegin": "2019-08-24T14:15:22Z",
              "dateEnd": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "advertisements": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "campaign": {},
                  "tags": [
                    {}
                  ],
                  "ageMin": 0,
                  "ageMax": 0,
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ]
            }
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {}
          ]
        },
        "versions": []
      },
      "name": "string",
      "semVer": "string"
    }
  ]
}
```

| Nom          | Type                                            | Requis  | Restriction | Description |
| ------------ | ----------------------------------------------- | ------- | ----------- | ----------- |
| id           | `string(uuid)`                                  | `false` | aucune      | aucune      |
| mediaId      | `string(uuid)`                                  | `false` | aucune      | aucune      |
| name         | `string` ou `null`                              | `false` | aucune      | aucune      |
| status       | `string` ou `null`                              | `false` | aucune      | aucune      |
| dateLaunch   | `string(date-time)`                             | `false` | aucune      | aucune      |
| dateCreation | `string(date-time)`                             | `false` | aucune      | aucune      |
| dateUpdate   | `string(date-time)`                             | `false` | aucune      | aucune      |
| organization | [OrganizationModel](#schemaorganizationmodel)   | `false` | aucune      | aucune      |
| versions     | [[VersionModel](#schemaversionmodel)] ou `null` | `false` | aucune      | aucune      |

## GamePublicDto

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
  "name": "string",
  "status": "string",
  "organization": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "publicEmail": "string",
    "localization": "string",
    "logoUrl": "string",
    "websiteUrl": "string"
  },
  "dateLaunch": "2019-08-24T14:15:22Z",
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z"
}
```

| Nom          | Type                                                  | Requis  | Restriction | Description |
| ------------ | ----------------------------------------------------- | ------- | ----------- | ----------- |
| id           | `string(uuid)`                                        | `false` | aucune      | aucune      |
| mediaId      | `string(uuid)`                                        | `false` | aucune      | aucune      |
| name         | `string` ou `null`                                    | `false` | aucune      | aucune      |
| status       | `string` ou `null`                                    | `false` | aucune      | aucune      |
| organization | [OrganizationPublicDto](#schemaorganizationpublicdto) | `false` | aucune      | aucune      |
| dateLaunch   | `string(date-time)`                                   | `false` | aucune      | aucune      |
| dateCreation | `string(date-time)`                                   | `false` | aucune      | aucune      |
| dateUpdate   | `string(date-time)`                                   | `false` | aucune      | aucune      |

## GamePublicDtoDataAllDto

### Propriétés

```json
{
  "pageIndex": 0,
  "itemsPerPage": 0,
  "totalPages": 0,
  "currentItemCount": 0,
  "items": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
      "name": "string",
      "status": "string",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string"
      },
      "dateLaunch": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    }
  ]
}
```

| Nom              | Type                                              | Requis  | Restriction | Description |
| ---------------- | ------------------------------------------------- | ------- | ----------- | ----------- |
| pageIndex        | `integer(int32)`                                  | `false` | aucune      | aucune      |
| itemsPerPage     | `integer(int32)`                                  | `false` | aucune      | aucune      |
| totalPages       | `integer(int32)`                                  | `false` | aucune      | aucune      |
| currentItemCount | `integer(int32)`                                  | `false` | aucune      | aucune      |
| items            | [[GamePublicDto](#schemagamepublicdto)] ou `null` | `false` | aucune      | aucune      |

## GamePublicDtoGetAllDto

### Propriétés

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string"
        },
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z"
      }
    ]
  }
}
```

| Nom  | Type                                                      | Requis  | Restriction | Description |
| ---- | --------------------------------------------------------- | ------- | ----------- | ----------- |
| data | [GamePublicDtoDataAllDto](#schemagamepublicdtodataalldto) | `false` | aucune      | aucune      |

## GamePublicDtoGetDto

### Propriétés

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
    "name": "string",
    "status": "string",
    "organization": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "publicEmail": "string",
      "localization": "string",
      "logoUrl": "string",
      "websiteUrl": "string"
    },
    "dateLaunch": "2019-08-24T14:15:22Z",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

| Nom  | Type                                  | Requis  | Restriction | Description |
| ---- | ------------------------------------- | ------- | ----------- | ----------- |
| data | [GamePublicDto](#schemagamepublicdto) | `false` | aucune      | aucune      |

## ObjectGetDto

### Propriétés

```json
{
  "data": null
}
```

| Nom  | Type | Requis  | Restriction | Description |
| ---- | ---- | ------- | ----------- | ----------- |
| data | any  | `false` | aucune      | aucune      |

## OrganizationModel

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "name": "string",
  "publicEmail": "string",
  "privateEmail": "string",
  "localization": "string",
  "logoUrl": "string",
  "websiteUrl": "string",
  "type": "Developers",
  "state": "Created",
  "defaultAuthorization": "User",
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z",
  "campaigns": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "ageMin": "string",
      "ageMax": "string",
      "dateBegin": "2019-08-24T14:15:22Z",
      "dateEnd": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "privateEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string",
        "type": "Developers",
        "state": "Created",
        "defaultAuthorization": "User",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "campaigns": [],
        "users": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "role": "User",
            "username": "string",
            "firstName": "string",
            "lastName": "string",
            "password": "string",
            "email": "string",
            "type": "Developer",
            "alias": "string",
            "phone": "string",
            "level": "string",
            "status": "string",
            "dateStatus": "string",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organizations": [
              {}
            ]
          }
        ],
        "games": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
            "name": "string",
            "status": "string",
            "dateLaunch": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organization": {},
            "versions": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "game": {},
                "name": "string",
                "semVer": "string"
              }
            ]
          }
        ]
      },
      "advertisements": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "campaign": {},
          "tags": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "description": "string",
              "adContainers": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "version": {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "game": {},
                    "name": "string",
                    "semVer": "string"
                  },
                  "organization": {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "publicEmail": "string",
                    "privateEmail": "string",
                    "localization": "string",
                    "logoUrl": "string",
                    "websiteUrl": "string",
                    "type": "Developers",
                    "state": "Created",
                    "defaultAuthorization": "User",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "campaigns": [],
                    "users": [],
                    "games": []
                  },
                  "tags": [
                    {}
                  ],
                  "type": "Type2D",
                  "aspectRatio": "Aspect1X1",
                  "width": 0,
                  "height": 0,
                  "depth": 0,
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ],
              "advertisements": [
                {}
              ],
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z"
            }
          ],
          "ageMin": 0,
          "ageMax": 0,
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        }
      ]
    }
  ],
  "users": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "role": "User",
      "username": "string",
      "firstName": "string",
      "lastName": "string",
      "password": "string",
      "email": "string",
      "type": "Developer",
      "alias": "string",
      "phone": "string",
      "level": "string",
      "status": "string",
      "dateStatus": "string",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z",
      "organizations": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "ageMin": "string",
              "ageMax": "string",
              "dateBegin": "2019-08-24T14:15:22Z",
              "dateEnd": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "advertisements": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "campaign": {},
                  "tags": [
                    {}
                  ],
                  "ageMin": 0,
                  "ageMax": 0,
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ]
            }
          ],
          "users": [],
          "games": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
              "name": "string",
              "status": "string",
              "dateLaunch": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "versions": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "game": {},
                  "name": "string",
                  "semVer": "string"
                }
              ]
            }
          ]
        }
      ]
    }
  ],
  "games": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
      "name": "string",
      "status": "string",
      "dateLaunch": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "privateEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string",
        "type": "Developers",
        "state": "Created",
        "defaultAuthorization": "User",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "campaigns": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "ageMin": "string",
            "ageMax": "string",
            "dateBegin": "2019-08-24T14:15:22Z",
            "dateEnd": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organization": {},
            "advertisements": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "campaign": {},
                "tags": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "description": "string",
                    "adContainers": [],
                    "advertisements": [],
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "ageMin": 0,
                "ageMax": 0,
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ]
          }
        ],
        "users": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "role": "User",
            "username": "string",
            "firstName": "string",
            "lastName": "string",
            "password": "string",
            "email": "string",
            "type": "Developer",
            "alias": "string",
            "phone": "string",
            "level": "string",
            "status": "string",
            "dateStatus": "string",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organizations": [
              {}
            ]
          }
        ],
        "games": []
      },
      "versions": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "game": {},
          "name": "string",
          "semVer": "string"
        }
      ]
    }
  ]
}
```

| Nom                  | Type                                                                  | Requis  | Restriction | Description |
| -------------------- | --------------------------------------------------------------------- | ------- | ----------- | ----------- |
| id                   | `string(uuid)`                                                        | `false` | aucune      | aucune      |
| name                 | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| publicEmail          | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| privateEmail         | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| localization         | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| logoUrl              | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| websiteUrl           | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| type                 | [OrganizationType](#schemaorganizationtype)                           | `false` | aucune      | aucune      |
| state                | [OrganizationState](#schemaorganizationstate)                         | `false` | aucune      | aucune      |
| defaultAuthorization | [OrganizationUserAuthorization](#schemaorganizationuserauthorization) | `false` | aucune      | aucune      |
| dateCreation         | `string(date-time)`                                                   | `false` | aucune      | aucune      |
| dateUpdate           | `string(date-time)`                                                   | `false` | aucune      | aucune      |
| campaigns            | [[CampaignModel](#schemacampaignmodel)] ou `null`                     | `false` | aucune      | aucune      |
| users                | [[UserModel](#schemausermodel)] ou `null`                             | `false` | aucune      | aucune      |
| games                | [[GameModel](#schemagamemodel)] ou `null`                             | `false` | aucune      | aucune      |

## OrganizationPrivateDto

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "name": "string",
  "publicEmail": "string",
  "privateEmail": "string",
  "localization": "string",
  "logoUrl": "string",
  "websiteUrl": "string",
  "type": "Developers",
  "state": "Created",
  "defaultAuthorization": "User",
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z",
  "campaigns": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "ageMin": "string",
      "ageMax": "string",
      "dateBegin": "2019-08-24T14:15:22Z",
      "dateEnd": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "privateEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string",
        "type": "Developers",
        "state": "Created",
        "defaultAuthorization": "User",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "campaigns": [
          {}
        ],
        "users": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "role": "User",
            "username": "string",
            "firstName": "string",
            "lastName": "string",
            "password": "string",
            "email": "string",
            "type": "Developer",
            "alias": "string",
            "phone": "string",
            "level": "string",
            "status": "string",
            "dateStatus": "string",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organizations": [
              {}
            ]
          }
        ],
        "games": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
            "name": "string",
            "status": "string",
            "dateLaunch": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organization": {},
            "versions": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "game": {},
                "name": "string",
                "semVer": "string"
              }
            ]
          }
        ]
      },
      "advertisements": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "campaign": {},
          "tags": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "description": "string",
              "adContainers": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "version": {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "game": {},
                    "name": "string",
                    "semVer": "string"
                  },
                  "organization": {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "publicEmail": "string",
                    "privateEmail": "string",
                    "localization": "string",
                    "logoUrl": "string",
                    "websiteUrl": "string",
                    "type": "Developers",
                    "state": "Created",
                    "defaultAuthorization": "User",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "campaigns": [],
                    "users": [],
                    "games": []
                  },
                  "tags": [
                    {}
                  ],
                  "type": "Type2D",
                  "aspectRatio": "Aspect1X1",
                  "width": 0,
                  "height": 0,
                  "depth": 0,
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ],
              "advertisements": [
                {}
              ],
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z"
            }
          ],
          "ageMin": 0,
          "ageMax": 0,
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        }
      ]
    }
  ],
  "users": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "username": "string",
      "email": "string",
      "type": "Developer"
    }
  ],
  "games": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
      "name": "string",
      "status": "string",
      "dateLaunch": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "privateEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string",
        "type": "Developers",
        "state": "Created",
        "defaultAuthorization": "User",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "campaigns": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "ageMin": "string",
            "ageMax": "string",
            "dateBegin": "2019-08-24T14:15:22Z",
            "dateEnd": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organization": {},
            "advertisements": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "campaign": {},
                "tags": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "description": "string",
                    "adContainers": [],
                    "advertisements": [],
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "ageMin": 0,
                "ageMax": 0,
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ]
          }
        ],
        "users": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "role": "User",
            "username": "string",
            "firstName": "string",
            "lastName": "string",
            "password": "string",
            "email": "string",
            "type": "Developer",
            "alias": "string",
            "phone": "string",
            "level": "string",
            "status": "string",
            "dateStatus": "string",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organizations": [
              {}
            ]
          }
        ],
        "games": [
          {}
        ]
      },
      "versions": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "game": {},
          "name": "string",
          "semVer": "string"
        }
      ]
    }
  ]
}
```

| Nom                  | Type                                                                  | Requis  | Restriction | Description |
| -------------------- | --------------------------------------------------------------------- | ------- | ----------- | ----------- |
| id                   | `string(uuid)`                                                        | `false` | aucune      | aucune      |
| name                 | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| publicEmail          | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| privateEmail         | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| localization         | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| logoUrl              | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| websiteUrl           | `string` ou `null`                                                    | `false` | aucune      | aucune      |
| type                 | [OrganizationType](#schemaorganizationtype)                           | `false` | aucune      | aucune      |
| state                | [OrganizationState](#schemaorganizationstate)                         | `false` | aucune      | aucune      |
| defaultAuthorization | [OrganizationUserAuthorization](#schemaorganizationuserauthorization) | `false` | aucune      | aucune      |
| dateCreation         | `string(date-time)`                                                   | `false` | aucune      | aucune      |
| dateUpdate           | `string(date-time)`                                                   | `false` | aucune      | aucune      |
| campaigns            | [[CampaignModel](#schemacampaignmodel)] ou `null`                     | `false` | aucune      | aucune      |
| users                | [[UserPublicDto](#schemauserpublicdto)] ou `null`                     | `false` | aucune      | aucune      |
| games                | [[GameModel](#schemagamemodel)] ou `null`                             | `false` | aucune      | aucune      |

## OrganizationPrivateDtoGetDto

### Propriétés

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "publicEmail": "string",
    "privateEmail": "string",
    "localization": "string",
    "logoUrl": "string",
    "websiteUrl": "string",
    "type": "Developers",
    "state": "Created",
    "defaultAuthorization": "User",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z",
    "campaigns": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "ageMin": "string",
        "ageMax": "string",
        "dateBegin": "2019-08-24T14:15:22Z",
        "dateEnd": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {}
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
              "name": "string",
              "status": "string",
              "dateLaunch": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "versions": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "game": {},
                  "name": "string",
                  "semVer": "string"
                }
              ]
            }
          ]
        },
        "advertisements": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "campaign": {},
            "tags": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "description": "string",
                "adContainers": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "version": {},
                    "organization": {},
                    "tags": [],
                    "type": "Type2D",
                    "aspectRatio": "Aspect1X1",
                    "width": 0,
                    "height": 0,
                    "depth": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "advertisements": [
                  {}
                ],
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ],
            "ageMin": 0,
            "ageMax": 0,
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z"
          }
        ]
      }
    ],
    "users": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "username": "string",
        "email": "string",
        "type": "Developer"
      }
    ],
    "games": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "ageMin": "string",
              "ageMax": "string",
              "dateBegin": "2019-08-24T14:15:22Z",
              "dateEnd": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "advertisements": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "campaign": {},
                  "tags": [
                    {}
                  ],
                  "ageMin": 0,
                  "ageMax": 0,
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ]
            }
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {}
          ]
        },
        "versions": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "game": {},
            "name": "string",
            "semVer": "string"
          }
        ]
      }
    ]
  }
}
```

| Nom  | Type                                                    | Requis  | Restriction | Description |
| ---- | ------------------------------------------------------- | ------- | ----------- | ----------- |
| data | [OrganizationPrivateDto](#schemaorganizationprivatedto) | `false` | aucune      | aucune      |

## OrganizationPublicDto

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "name": "string",
  "publicEmail": "string",
  "localization": "string",
  "logoUrl": "string",
  "websiteUrl": "string"
}
```

| Nom          | Type               | Requis  | Restriction | Description |
| ------------ | ------------------ | ------- | ----------- | ----------- |
| id           | `string(uuid)`     | `false` | aucune      | aucune      |
| name         | `string` ou `null` | `false` | aucune      | aucune      |
| publicEmail  | `string` ou `null` | `false` | aucune      | aucune      |
| localization | `string` ou `null` | `false` | aucune      | aucune      |
| logoUrl      | `string` ou `null` | `false` | aucune      | aucune      |
| websiteUrl   | `string` ou `null` | `false` | aucune      | aucune      |

## OrganizationPublicDtoDataAllDto

### Propriétés

```json
{
  "pageIndex": 0,
  "itemsPerPage": 0,
  "totalPages": 0,
  "currentItemCount": 0,
  "items": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "publicEmail": "string",
      "localization": "string",
      "logoUrl": "string",
      "websiteUrl": "string"
    }
  ]
}
```

| Nom              | Type                                                              | Requis  | Restriction | Description |
| ---------------- | ----------------------------------------------------------------- | ------- | ----------- | ----------- |
| pageIndex        | `integer(int32)`                                                  | `false` | aucune      | aucune      |
| itemsPerPage     | `integer(int32)`                                                  | `false` | aucune      | aucune      |
| totalPages       | `integer(int32)`                                                  | `false` | aucune      | aucune      |
| currentItemCount | `integer(int32)`                                                  | `false` | aucune      | aucune      |
| items            | [[OrganizationPublicDto](#schemaorganizationpublicdto)] ou `null` | `false` | aucune      | aucune      |

## OrganizationPublicDtoGetAllDto

### Propriétés

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string"
      }
    ]
  }
}
```

| Nom  | Type                                                                      | Requis  | Restriction | Description |
| ---- | ------------------------------------------------------------------------- | ------- | ----------- | ----------- |
| data | [OrganizationPublicDtoDataAllDto](#schemaorganizationpublicdtodataalldto) | `false` | aucune      | aucune      |

## OrganizationState

### Propriétés

```json
"Created"
```

| Nom         | Type     | Requis  | Restriction | Description |
| ----------- | -------- | ------- | ----------- | ----------- |
| *anonymous* | `string` | `false` | aucune      | aucune      |

#### Valeurs énumérées

| Property    | Value    |
| ----------- | -------- |
| *anonymous* | Created  |
| *anonymous* | Active   |
| *anonymous* | Inactive |

## OrganizationType

### Propriétés

```json
"Developers"
```

| Nom         | Type     | Requis  | Restriction | Description |
| ----------- | -------- | ------- | ----------- | ----------- |
| *anonymous* | `string` | `false` | aucune      | aucune      |

#### Valeurs énumérées

| Property    | Value       |
| ----------- | ----------- |
| *anonymous* | Developers  |
| *anonymous* | Advertisers |

## OrganizationUserAuthorization

### Propriétés

```json
"User"
```

| Nom         | Type     | Requis  | Restriction | Description |
| ----------- | -------- | ------- | ----------- | ----------- |
| *anonymous* | `string` | `false` | aucune      | aucune      |

#### Valeurs énumérées

| Property    | Value |
| ----------- | ----- |
| *anonymous* | User  |
| *anonymous* | Admin |

## TagModel

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "name": "string",
  "description": "string",
  "adContainers": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "version": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "game": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
          "name": "string",
          "status": "string",
          "dateLaunch": "2019-08-24T14:15:22Z",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "organization": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "publicEmail": "string",
            "privateEmail": "string",
            "localization": "string",
            "logoUrl": "string",
            "websiteUrl": "string",
            "type": "Developers",
            "state": "Created",
            "defaultAuthorization": "User",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "campaigns": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "ageMin": "string",
                "ageMax": "string",
                "dateBegin": "2019-08-24T14:15:22Z",
                "dateEnd": "2019-08-24T14:15:22Z",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "organization": {},
                "advertisements": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "campaign": {},
                    "tags": [],
                    "ageMin": 0,
                    "ageMax": 0,
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ]
              }
            ],
            "users": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "role": "User",
                "username": "string",
                "firstName": "string",
                "lastName": "string",
                "password": "string",
                "email": "string",
                "type": "Developer",
                "alias": "string",
                "phone": "string",
                "level": "string",
                "status": "string",
                "dateStatus": "string",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "organizations": [
                  {}
                ]
              }
            ],
            "games": [
              {}
            ]
          },
          "versions": [
            {}
          ]
        },
        "name": "string",
        "semVer": "string"
      },
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "privateEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string",
        "type": "Developers",
        "state": "Created",
        "defaultAuthorization": "User",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "campaigns": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "ageMin": "string",
            "ageMax": "string",
            "dateBegin": "2019-08-24T14:15:22Z",
            "dateEnd": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organization": {},
            "advertisements": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "campaign": {},
                "tags": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "description": "string",
                    "adContainers": [],
                    "advertisements": [],
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z"
                  }
                ],
                "ageMin": 0,
                "ageMax": 0,
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z"
              }
            ]
          }
        ],
        "users": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "role": "User",
            "username": "string",
            "firstName": "string",
            "lastName": "string",
            "password": "string",
            "email": "string",
            "type": "Developer",
            "alias": "string",
            "phone": "string",
            "level": "string",
            "status": "string",
            "dateStatus": "string",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organizations": [
              {}
            ]
          }
        ],
        "games": [
          {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
            "name": "string",
            "status": "string",
            "dateLaunch": "2019-08-24T14:15:22Z",
            "dateCreation": "2019-08-24T14:15:22Z",
            "dateUpdate": "2019-08-24T14:15:22Z",
            "organization": {},
            "versions": [
              {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "game": {},
                "name": "string",
                "semVer": "string"
              }
            ]
          }
        ]
      },
      "tags": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "description": "string",
          "adContainers": [],
          "advertisements": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "campaign": {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "ageMin": "string",
                "ageMax": "string",
                "dateBegin": "2019-08-24T14:15:22Z",
                "dateEnd": "2019-08-24T14:15:22Z",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "organization": {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "publicEmail": "string",
                  "privateEmail": "string",
                  "localization": "string",
                  "logoUrl": "string",
                  "websiteUrl": "string",
                  "type": "Developers",
                  "state": "Created",
                  "defaultAuthorization": "User",
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z",
                  "campaigns": [
                    {}
                  ],
                  "users": [
                    {}
                  ],
                  "games": [
                    {}
                  ]
                },
                "advertisements": [
                  {}
                ]
              },
              "tags": [
                {}
              ],
              "ageMin": 0,
              "ageMax": 0,
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z"
            }
          ],
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        }
      ],
      "type": "Type2D",
      "aspectRatio": "Aspect1X1",
      "width": 0,
      "height": 0,
      "depth": 0,
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    }
  ],
  "advertisements": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "campaign": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "ageMin": "string",
        "ageMax": "string",
        "dateBegin": "2019-08-24T14:15:22Z",
        "dateEnd": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "privateEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string",
          "type": "Developers",
          "state": "Created",
          "defaultAuthorization": "User",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "campaigns": [
            {}
          ],
          "users": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "role": "User",
              "username": "string",
              "firstName": "string",
              "lastName": "string",
              "password": "string",
              "email": "string",
              "type": "Developer",
              "alias": "string",
              "phone": "string",
              "level": "string",
              "status": "string",
              "dateStatus": "string",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organizations": [
                {}
              ]
            }
          ],
          "games": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
              "name": "string",
              "status": "string",
              "dateLaunch": "2019-08-24T14:15:22Z",
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z",
              "organization": {},
              "versions": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "game": {},
                  "name": "string",
                  "semVer": "string"
                }
              ]
            }
          ]
        },
        "advertisements": [
          {}
        ]
      },
      "tags": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "description": "string",
          "adContainers": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "version": {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "game": {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
                  "name": "string",
                  "status": "string",
                  "dateLaunch": "2019-08-24T14:15:22Z",
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z",
                  "organization": {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "publicEmail": "string",
                    "privateEmail": "string",
                    "localization": "string",
                    "logoUrl": "string",
                    "websiteUrl": "string",
                    "type": "Developers",
                    "state": "Created",
                    "defaultAuthorization": "User",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "campaigns": [],
                    "users": [],
                    "games": []
                  },
                  "versions": [
                    {}
                  ]
                },
                "name": "string",
                "semVer": "string"
              },
              "organization": {
                "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                "name": "string",
                "publicEmail": "string",
                "privateEmail": "string",
                "localization": "string",
                "logoUrl": "string",
                "websiteUrl": "string",
                "type": "Developers",
                "state": "Created",
                "defaultAuthorization": "User",
                "dateCreation": "2019-08-24T14:15:22Z",
                "dateUpdate": "2019-08-24T14:15:22Z",
                "campaigns": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "name": "string",
                    "ageMin": "string",
                    "ageMax": "string",
                    "dateBegin": "2019-08-24T14:15:22Z",
                    "dateEnd": "2019-08-24T14:15:22Z",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organization": {},
                    "advertisements": []
                  }
                ],
                "users": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "role": "User",
                    "username": "string",
                    "firstName": "string",
                    "lastName": "string",
                    "password": "string",
                    "email": "string",
                    "type": "Developer",
                    "alias": "string",
                    "phone": "string",
                    "level": "string",
                    "status": "string",
                    "dateStatus": "string",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organizations": []
                  }
                ],
                "games": [
                  {
                    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                    "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
                    "name": "string",
                    "status": "string",
                    "dateLaunch": "2019-08-24T14:15:22Z",
                    "dateCreation": "2019-08-24T14:15:22Z",
                    "dateUpdate": "2019-08-24T14:15:22Z",
                    "organization": {},
                    "versions": []
                  }
                ]
              },
              "tags": [
                {}
              ],
              "type": "Type2D",
              "aspectRatio": "Aspect1X1",
              "width": 0,
              "height": 0,
              "depth": 0,
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z"
            }
          ],
          "advertisements": [],
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        }
      ],
      "ageMin": 0,
      "ageMax": 0,
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    }
  ],
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z"
}
```

| Nom            | Type                                                        | Requis  | Restriction | Description |
| -------------- | ----------------------------------------------------------- | ------- | ----------- | ----------- |
| id             | `string(uuid)`                                              | `false` | aucune      | aucune      |
| name           | `string`                                                    | `true`  | aucune      | aucune      |
| description    | `string` ou `null`                                          | `false` | aucune      | aucune      |
| adContainers   | [[AdContainerModel](#schemaadcontainermodel)] ou `null`     | `false` | aucune      | aucune      |
| advertisements | [[AdvertisementModel](#schemaadvertisementmodel)] ou `null` | `false` | aucune      | aucune      |
| dateCreation   | `string(date-time)`                                         | `false` | aucune      | aucune      |
| dateUpdate     | `string(date-time)`                                         | `false` | aucune      | aucune      |

## TagPublicDto

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "name": "string",
  "description": "string"
}
```

| Nom         | Type               | Requis  | Restriction | Description |
| ----------- | ------------------ | ------- | ----------- | ----------- |
| id          | `string(uuid)`     | `false` | aucune      | aucune      |
| name        | `string` ou `null` | `false` | aucune      | aucune      |
| description | `string` ou `null` | `false` | aucune      | aucune      |

## TagPublicDtoDataAllDto

### Propriétés

```json
{
  "pageIndex": 0,
  "itemsPerPage": 0,
  "totalPages": 0,
  "currentItemCount": 0,
  "items": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "description": "string"
    }
  ]
}
```

| Nom              | Type                                            | Requis  | Restriction | Description |
| ---------------- | ----------------------------------------------- | ------- | ----------- | ----------- |
| pageIndex        | `integer(int32)`                                | `false` | aucune      | aucune      |
| itemsPerPage     | `integer(int32)`                                | `false` | aucune      | aucune      |
| totalPages       | `integer(int32)`                                | `false` | aucune      | aucune      |
| currentItemCount | `integer(int32)`                                | `false` | aucune      | aucune      |
| items            | [[TagPublicDto](#schematagpublicdto)] ou `null` | `false` | aucune      | aucune      |

## TagPublicDtoGetAllDto

### Propriétés

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "description": "string"
      }
    ]
  }
}
```

| Nom  | Type                                                    | Requis  | Restriction | Description |
| ---- | ------------------------------------------------------- | ------- | ----------- | ----------- |
| data | [TagPublicDtoDataAllDto](#schematagpublicdtodataalldto) | `false` | aucune      | aucune      |

## TagPublicDtoGetDto

### Propriétés

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "name": "string",
    "description": "string"
  }
}
```

| Nom  | Type                                | Requis  | Restriction | Description |
| ---- | ----------------------------------- | ------- | ----------- | ----------- |
| data | [TagPublicDto](#schematagpublicdto) | `false` | aucune      | aucune      |

## UserLoginResponseDto

### Propriétés

```json
{
  "token": "string"
}
```

| Nom   | Type               | Requis  | Restriction | Description |
| ----- | ------------------ | ------- | ----------- | ----------- |
| token | `string` ou `null` | `false` | aucune      | aucune      |

## UserLoginResponseDtoGetDto

### Propriétés

```json
{
  "data": {
    "token": "string"
  }
}
```

| Nom  | Type                                                | Requis  | Restriction | Description |
| ---- | --------------------------------------------------- | ------- | ----------- | ----------- |
| data | [UserLoginResponseDto](#schemauserloginresponsedto) | `false` | aucune      | aucune      |

## UserModel

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "role": "User",
  "username": "string",
  "firstName": "string",
  "lastName": "string",
  "password": "string",
  "email": "string",
  "type": "Developer",
  "alias": "string",
  "phone": "string",
  "level": "string",
  "status": "string",
  "dateStatus": "string",
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z",
  "organizations": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "publicEmail": "string",
      "privateEmail": "string",
      "localization": "string",
      "logoUrl": "string",
      "websiteUrl": "string",
      "type": "Developers",
      "state": "Created",
      "defaultAuthorization": "User",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z",
      "campaigns": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "ageMin": "string",
          "ageMax": "string",
          "dateBegin": "2019-08-24T14:15:22Z",
          "dateEnd": "2019-08-24T14:15:22Z",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "organization": {},
          "advertisements": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "campaign": {},
              "tags": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "description": "string",
                  "adContainers": [
                    {}
                  ],
                  "advertisements": [
                    {}
                  ],
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ],
              "ageMin": 0,
              "ageMax": 0,
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z"
            }
          ]
        }
      ],
      "users": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "role": "User",
          "username": "string",
          "firstName": "string",
          "lastName": "string",
          "password": "string",
          "email": "string",
          "type": "Developer",
          "alias": "string",
          "phone": "string",
          "level": "string",
          "status": "string",
          "dateStatus": "string",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "organizations": []
        }
      ],
      "games": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
          "name": "string",
          "status": "string",
          "dateLaunch": "2019-08-24T14:15:22Z",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "organization": {},
          "versions": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "game": {},
              "name": "string",
              "semVer": "string"
            }
          ]
        }
      ]
    }
  ]
}
```

| Nom           | Type                                                      | Requis  | Restriction | Description |
| ------------- | --------------------------------------------------------- | ------- | ----------- | ----------- |
| id            | `string(uuid)`                                            | `false` | aucune      | aucune      |
| role          | [UserRole](#schemauserrole)                               | `false` | aucune      | aucune      |
| username      | `string` ou `null`                                        | `false` | aucune      | aucune      |
| firstName     | `string` ou `null`                                        | `false` | aucune      | aucune      |
| lastName      | `string` ou `null`                                        | `false` | aucune      | aucune      |
| password      | `string` ou `null`                                        | `false` | aucune      | aucune      |
| email         | `string` ou `null`                                        | `false` | aucune      | aucune      |
| type          | [UserType](#schemausertype)                               | `false` | aucune      | aucune      |
| alias         | `string` ou `null`                                        | `false` | aucune      | aucune      |
| phone         | `string` ou `null`                                        | `false` | aucune      | aucune      |
| level         | `string` ou `null`                                        | `false` | aucune      | aucune      |
| status        | `string` ou `null`                                        | `false` | aucune      | aucune      |
| dateStatus    | `string` ou `null`                                        | `false` | aucune      | aucune      |
| dateCreation  | `string(date-time)`                                       | `false` | aucune      | aucune      |
| dateUpdate    | `string(date-time)`                                       | `false` | aucune      | aucune      |
| organizations | [[OrganizationModel](#schemaorganizationmodel)] ou `null` | `false` | aucune      | aucune      |

## UserPrivateDto

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "role": "User",
  "username": "string",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "type": "Developer",
  "alias": "string",
  "phone": "string",
  "level": "string",
  "status": "string",
  "dateStatus": "string",
  "dateCreation": "2019-08-24T14:15:22Z",
  "dateUpdate": "2019-08-24T14:15:22Z"
}
```

| Nom          | Type                        | Requis  | Restriction | Description |
| ------------ | --------------------------- | ------- | ----------- | ----------- |
| id           | `string(uuid)`              | `false` | aucune      | aucune      |
| role         | [UserRole](#schemauserrole) | `false` | aucune      | aucune      |
| username     | `string` ou `null`          | `false` | aucune      | aucune      |
| firstName    | `string` ou `null`          | `false` | aucune      | aucune      |
| lastName     | `string` ou `null`          | `false` | aucune      | aucune      |
| email        | `string` ou `null`          | `false` | aucune      | aucune      |
| type         | [UserType](#schemausertype) | `false` | aucune      | aucune      |
| alias        | `string` ou `null`          | `false` | aucune      | aucune      |
| phone        | `string` ou `null`          | `false` | aucune      | aucune      |
| level        | `string` ou `null`          | `false` | aucune      | aucune      |
| status       | `string` ou `null`          | `false` | aucune      | aucune      |
| dateStatus   | `string` ou `null`          | `false` | aucune      | aucune      |
| dateCreation | `string(date-time)`         | `false` | aucune      | aucune      |
| dateUpdate   | `string(date-time)`         | `false` | aucune      | aucune      |

## UserPrivateDtoGetDto

### Propriétés

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "role": "User",
    "username": "string",
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "type": "Developer",
    "alias": "string",
    "phone": "string",
    "level": "string",
    "status": "string",
    "dateStatus": "string",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  }
}
```

| Nom  | Type                                    | Requis  | Restriction | Description |
| ---- | --------------------------------------- | ------- | ----------- | ----------- |
| data | [UserPrivateDto](#schemauserprivatedto) | `false` | aucune      | aucune      |

## UserPublicDto

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "username": "string",
  "email": "string",
  "type": "Developer"
}
```

| Nom      | Type                        | Requis  | Restriction | Description |
| -------- | --------------------------- | ------- | ----------- | ----------- |
| id       | `string(uuid)`              | `false` | aucune      | aucune      |
| username | `string` ou `null`          | `false` | aucune      | aucune      |
| email    | `string` ou `null`          | `false` | aucune      | aucune      |
| type     | [UserType](#schemausertype) | `false` | aucune      | aucune      |

## UserPublicDtoDataAllDto

### Propriétés

```json
{
  "pageIndex": 0,
  "itemsPerPage": 0,
  "totalPages": 0,
  "currentItemCount": 0,
  "items": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "username": "string",
      "email": "string",
      "type": "Developer"
    }
  ]
}
```

| Nom              | Type                                              | Requis  | Restriction | Description |
| ---------------- | ------------------------------------------------- | ------- | ----------- | ----------- |
| pageIndex        | `integer(int32)`                                  | `false` | aucune      | aucune      |
| itemsPerPage     | `integer(int32)`                                  | `false` | aucune      | aucune      |
| totalPages       | `integer(int32)`                                  | `false` | aucune      | aucune      |
| currentItemCount | `integer(int32)`                                  | `false` | aucune      | aucune      |
| items            | [[UserPublicDto](#schemauserpublicdto)] ou `null` | `false` | aucune      | aucune      |

## UserPublicDtoGetAllDto

### Propriétés

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "username": "string",
        "email": "string",
        "type": "Developer"
      }
    ]
  }
}
```

| Nom  | Type                                                      | Requis  | Restriction | Description |
| ---- | --------------------------------------------------------- | ------- | ----------- | ----------- |
| data | [UserPublicDtoDataAllDto](#schemauserpublicdtodataalldto) | `false` | aucune      | aucune      |

## UserRole

### Propriétés

```json
"User"
```

| Nom         | Type     | Requis  | Restriction | Description |
| ----------- | -------- | ------- | ----------- | ----------- |
| *anonymous* | `string` | `false` | aucune      | aucune      |

#### Valeurs énumérées

| Property    | Value |
| ----------- | ----- |
| *anonymous* | User  |
| *anonymous* | Admin |

## UserType

### Propriétés

```json
"Developer"
```

| Nom         | Type     | Requis  | Restriction | Description |
| ----------- | -------- | ------- | ----------- | ----------- |
| *anonymous* | `string` | `false` | aucune      | aucune      |

#### Valeurs énumérées

| Property    | Value      |
| ----------- | ---------- |
| *anonymous* | Developer  |
| *anonymous* | Advertiser |

## VersionModel

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "game": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
    "name": "string",
    "status": "string",
    "dateLaunch": "2019-08-24T14:15:22Z",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z",
    "organization": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "publicEmail": "string",
      "privateEmail": "string",
      "localization": "string",
      "logoUrl": "string",
      "websiteUrl": "string",
      "type": "Developers",
      "state": "Created",
      "defaultAuthorization": "User",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z",
      "campaigns": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "ageMin": "string",
          "ageMax": "string",
          "dateBegin": "2019-08-24T14:15:22Z",
          "dateEnd": "2019-08-24T14:15:22Z",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "organization": {},
          "advertisements": [
            {
              "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
              "name": "string",
              "campaign": {},
              "tags": [
                {
                  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
                  "name": "string",
                  "description": "string",
                  "adContainers": [
                    {}
                  ],
                  "advertisements": [
                    {}
                  ],
                  "dateCreation": "2019-08-24T14:15:22Z",
                  "dateUpdate": "2019-08-24T14:15:22Z"
                }
              ],
              "ageMin": 0,
              "ageMax": 0,
              "dateCreation": "2019-08-24T14:15:22Z",
              "dateUpdate": "2019-08-24T14:15:22Z"
            }
          ]
        }
      ],
      "users": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "role": "User",
          "username": "string",
          "firstName": "string",
          "lastName": "string",
          "password": "string",
          "email": "string",
          "type": "Developer",
          "alias": "string",
          "phone": "string",
          "level": "string",
          "status": "string",
          "dateStatus": "string",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z",
          "organizations": [
            {}
          ]
        }
      ],
      "games": [
        {}
      ]
    },
    "versions": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "game": {},
        "name": "string",
        "semVer": "string"
      }
    ]
  },
  "name": "string",
  "semVer": "string"
}
```

| Nom    | Type                          | Requis  | Restriction | Description |
| ------ | ----------------------------- | ------- | ----------- | ----------- |
| id     | `string(uuid)`                | `false` | aucune      | aucune      |
| game   | [GameModel](#schemagamemodel) | `false` | aucune      | aucune      |
| name   | `string` ou `null`            | `false` | aucune      | aucune      |
| semVer | `string` ou `null`            | `false` | aucune      | aucune      |

## VersionPublicDto

### Propriétés

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "game": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
    "name": "string",
    "status": "string",
    "organization": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "name": "string",
      "publicEmail": "string",
      "localization": "string",
      "logoUrl": "string",
      "websiteUrl": "string"
    },
    "dateLaunch": "2019-08-24T14:15:22Z",
    "dateCreation": "2019-08-24T14:15:22Z",
    "dateUpdate": "2019-08-24T14:15:22Z"
  },
  "name": "string",
  "semVer": "string"
}
```

| Nom    | Type                                  | Requis  | Restriction | Description |
| ------ | ------------------------------------- | ------- | ----------- | ----------- |
| id     | `string(uuid)`                        | `false` | aucune      | aucune      |
| game   | [GamePublicDto](#schemagamepublicdto) | `false` | aucune      | aucune      |
| name   | `string` ou `null`                    | `false` | aucune      | aucune      |
| semVer | `string` ou `null`                    | `false` | aucune      | aucune      |

## VersionPublicDtoDataAllDto

### Propriétés

```json
{
  "pageIndex": 0,
  "itemsPerPage": 0,
  "totalPages": 0,
  "currentItemCount": 0,
  "items": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "game": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
        "name": "string",
        "status": "string",
        "organization": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "name": "string",
          "publicEmail": "string",
          "localization": "string",
          "logoUrl": "string",
          "websiteUrl": "string"
        },
        "dateLaunch": "2019-08-24T14:15:22Z",
        "dateCreation": "2019-08-24T14:15:22Z",
        "dateUpdate": "2019-08-24T14:15:22Z"
      },
      "name": "string",
      "semVer": "string"
    }
  ]
}
```

| Nom              | Type                                                    | Requis  | Restriction | Description |
| ---------------- | ------------------------------------------------------- | ------- | ----------- | ----------- |
| pageIndex        | `integer(int32)`                                        | `false` | aucune      | aucune      |
| itemsPerPage     | `integer(int32)`                                        | `false` | aucune      | aucune      |
| totalPages       | `integer(int32)`                                        | `false` | aucune      | aucune      |
| currentItemCount | `integer(int32)`                                        | `false` | aucune      | aucune      |
| items            | [[VersionPublicDto](#schemaversionpublicdto)] ou `null` | `false` | aucune      | aucune      |

## VersionPublicDtoGetAllDto

### Propriétés

```json
{
  "data": {
    "pageIndex": 0,
    "itemsPerPage": 0,
    "totalPages": 0,
    "currentItemCount": 0,
    "items": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "game": {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
          "name": "string",
          "status": "string",
          "organization": {
            "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
            "name": "string",
            "publicEmail": "string",
            "localization": "string",
            "logoUrl": "string",
            "websiteUrl": "string"
          },
          "dateLaunch": "2019-08-24T14:15:22Z",
          "dateCreation": "2019-08-24T14:15:22Z",
          "dateUpdate": "2019-08-24T14:15:22Z"
        },
        "name": "string",
        "semVer": "string"
      }
    ]
  }
}
```

| Nom  | Type                                                            | Requis  | Restriction | Description |
| ---- | --------------------------------------------------------------- | ------- | ----------- | ----------- |
| data | [VersionPublicDtoDataAllDto](#schemaversionpublicdtodataalldto) | `false` | aucune      | aucune      |

## VersionPublicDtoGetDto

### Propriétés

```json
{
  "data": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "game": {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "mediaId": "5a8ffac5-2288-485d-b463-90c3cd9941ad",
      "name": "string",
      "status": "string",
      "organization": {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "name": "string",
        "publicEmail": "string",
        "localization": "string",
        "logoUrl": "string",
        "websiteUrl": "string"
      },
      "dateLaunch": "2019-08-24T14:15:22Z",
      "dateCreation": "2019-08-24T14:15:22Z",
      "dateUpdate": "2019-08-24T14:15:22Z"
    },
    "name": "string",
    "semVer": "string"
  }
}
```

| Nom  | Type                                        | Requis  | Restriction | Description |
| ---- | ------------------------------------------- | ------- | ----------- | ----------- |
| data | [VersionPublicDto](#schemaversionpublicdto) | `false` | aucune      | aucune      |
