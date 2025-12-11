

https://roadmap.sh/projects/fitness-workout-tracker


# API Documentation

## Table of Contents
1. [Overview](#overview)
2. [Authentication](#authentication)
3. [Authentication Endpoint](#authentication-controller)
4. [Exercise Data Endpoint](#exercise-data-controller)
5. [Error Codes](#error-codes)
6. [Changelog](#changelog)

---

## Overview

This document provides details about the API endpoints, including request methods, parameters, and example responses.

- **Base URL:** `https://api.example.com/v1`
- **Spotlight:** `https://rizqial-porto-api.stoplight.io/docs/workout-tracker/elz1nohtgf3z2-workout-tracker-api`
- **Format:** JSON
- **Version:** 1.0.0

---

## Authentication

The API uses **Bearer Token** authentication. Include the token in the `Authorization` header:

```http
Authorization: Bearer <your_token_here>
```

---

## Authentication Controller
Using JWT Authentication for Login and Register a new Account

 ### Endpoints

- [`POST` api/auth/register ](#post-apiauthregister)

- [`POST` api/auth/login](#post-apiauthlogin)


---


### `POST` api/auth/register 

Register new account for user

- **URL:** `/api/resource`
- **Method:** `POST`
- **Headers:**
    - `Authorization: Bearer <token>`
    - `Content-Type: application/json`
- **Body Parameters:**
    - `email` (string, required): email register
    - `username` (string, required): username register
    - `password` (string, required): password register

#### Example Request

```http
POST /api/auth/register HTTP/1.1
Host: WorkoutTrackerApi.example.com
Content-Type: application/json

{
  "email": "test123@gmail.com",
  "username" : "test",
  "password" : "Test12@"
}
```
#### Example Response

`200 (OK)`

---

### `POST` api/auth/login 

Login account

- **URL:** `/api/resource`
- **Method:** `POST`
- **Headers:**
    - `Content-Type: application/json`
- **Body Parameters:**
    - `username` (string, required): username account
    - `password` (string, required): password account

#### Example Request

```http
POST /api/auth/login HTTP/1.1
Host: WorkoutTrackerApi.example.com
Content-Type: application/json

{
  "username" : "test",
  "password" : "Test12@"
}
```
#### Example Response

`200 (OK)`

```json
{
    "token" : "BEARER_TOKEN"
}
```

---

## Exercise Data Controller

Exercise Data contains a list of all workout movements input by the admin, which users can later add to their exercise sets when creating a workout plan.

### Endpoints

- [`GET` /api/ExerciseDatas](#get-apiexercisedatas)
- [`GET` /api/ExerciseDatas/{id}](#get-apiexercisedatasid)
- [`POST` /api/ExerciseDatas](#post-apiexercisedatas)
- [`PUT` /api/ExerciseDatas/{id}](#put-apiexercisedatasid)
- [`DELETE` /api/ExerciseDatas/{id}](#delete-apiexercisedatasid)

---

### `GET` api/ExerciseDatas

Displays all data about exercises that have been input by the admin.

- **URL:** `/api/ExerciseDatas`
- **Method:** `GET`
- **Headers:**
    - `Content-Type: application/json`

```http
GET /api/exercisedatas
Host: api.example.com
Authorization: Bearer <your_token_here>
```

#### Example Response

`200 (OK)`

```json
{
  [
    {
      "id": 1,
      "name": "…",
      "categoryWorkout": 1,
      "muscleGroup": 1
    }
  ]
}
```


### `GET` api/ExerciseDatas/{id}

Displays all exercise data based on the input ID.

- **URL:** `/api/ExerciseDatas/{id}`
- **Method:** `GET`
- **Headers:**
  - `Content-Type: application/json`
- **Path Parameters**
  - id : integer

### Example Request

```http
GET /api/exercisedatas/1
Host: api.example.com
Authorization: Bearer <your_token_here>
```

### Example Response

`200 (OK)`

```json
{
  "id": 1,
  "name": "…",
  "categoryWorkout": 1,
  "muscleGroup": 1
}
```

### `POST` api/ExerciseDatas/

Input new exercise data

- **URL:** `/api/exercisedatas`
- **Method:** `POST`
- **Headers:**
    - `Authorization: Bearer <token>`
    - `Content-Type: application/json`
- **Body Parameters:**
    - `exerciseId` (integer, required) 
    - `name` (string, required)
    - `categoryWorkout` (enum, required)
    - `muscleGroup` (enum, required)

### Example Request

```http
POST /api/resource?page=1&limit=10 HTTP/1.1
Host: api.example.com
Authorization: Bearer <your_token_here>
Content-Type: application/json
```

```json
{
  "exerciseId": 1,
  "name": "Bench Press",
  "categoryWorkout": 1,
  "muscleGroup": 1,
}
```

### Example Response

`200 (OK)`

```json
{
  "exerciseId": 1,
  "name": "Bench Press",
  "categoryWorkout": 1,
  "muscleGroup": 1,
}
```

### `PUT` api/ExerciseDatas/{id}

Modifies exercise data based on the input ID.

- **URL:** `/api/exercisedatas/{id}`
- **Method:** `PUT`
- **Headers:**
    - `Authorization: Bearer <token>`
    - `Content-Type: application/json`
- **Path Parameters**
    - `Id` : integer
- **Body Parameters:**
    - `exerciseId` (integer, required) 
    - `name` (string, required)
    - `categoryWorkout` (enum, required)
    - `muscleGroup` (enum, required)

### Example Request

```http
PUT /api/exercisedatas/1 HTTP/1.1
Host: api.example.com
Authorization: Bearer <your_token_here>
Content-Type: application/json
```

```json
{
  "exerciseId": 0,
  "name": "Bench Press",
  "categoryWorkout": 1,
  "muscleGroup": 1,
}
```

### Example Response

`200 (OK)`

```json
{
  "exerciseId": 1,
  "name": "Bench Press",
  "categoryWorkout": 1,
  "muscleGroup": 1,
}
```

### `DELETE` api/ExerciseDatas/{id}

Delete exercise data

- **URL:** `/api/exercisedatas/{id}`
- **Method:** `DELETE`
- **Headers:**
    - `Authorization: Bearer <token>`
    - `Content-Type: application/json`
- **Path Parameters**
    - `Id` : integer

### Example Request

```http
DELETE /api/exercisedatas/1 HTTP/1.1
Host: api.example.com
Authorization: Bearer <your_token_here>
Content-Type: application/json
```



### Example Response

`200 (OK)`



---
## Endpoints

### GET /api/resource

Retrieve a list of resources.

- **URL:** `/api/resource`
- **Method:** `GET`
- **Headers:**
    - `Authorization: Bearer <token>`
- **Query Parameters:**
    - `page` (integer, optional): The page number to retrieve.
    - `limit` (integer, optional): The number of items per page.

#### Example Request

```http
GET /api/resource?page=1&limit=10 HTTP/1.1
Host: api.example.com
Authorization: Bearer <your_token_here>
```



#### Example Response

`200 (OK)`

```json
{
  "id": 1,
  "name": "Bench Press",
  "categoryWorkout": 1,
  "muscleGroup": 1
}
```

---

### POST /api/resource

Create a new resource.

- **URL:** `/api/resource`
- **Method:** `POST`
- **Headers:**
    - `Authorization: Bearer <token>`
    - `Content-Type: application/json`
- **Body Parameters:**
    - `name` (string, required): The name of the resource.

#### Example Request

```http
POST /api/resource HTTP/1.1
Host: api.example.com
Authorization: Bearer <your_token_here>
Content-Type: application/json

{
  "name": "New Resource"
}
```

#### Example Response

```json
{
  "id": 3,
  "name": "New Resource",
  "created_at": "2023-01-03T12:00:00Z"
}
```

---

### PUT /api/resource/{id}

Update an existing resource.

- **URL:** `/api/resource/{id}`
- **Method:** `PUT`
- **Headers:**
    - `Authorization: Bearer <token>`
    - `Content-Type: application/json`
- **Path Parameters:**
    - `id` (integer, required): The ID of the resource to update.
- **Body Parameters:**
    - `name` (string, optional): The updated name of the resource.

#### Example Request

```http
PUT /api/resource/3 HTTP/1.1
Host: api.example.com
Authorization: Bearer <your_token_here>
Content-Type: application/json

{
  "name": "Updated Resource"
}
```

#### Example Response

```json
{
  "id": 3,
  "name": "Updated Resource",
  "updated_at": "2023-01-04T12:00:00Z"
}
```

---

### DELETE /api/resource/{id}

Delete a resource.

- **URL:** `/api/resource/{id}`
- **Method:** `DELETE`
- **Headers:**
    - `Authorization: Bearer <token>`
- **Path Parameters:**
    - `id` (integer, required): The ID of the resource to delete.

#### Example Request

```http
DELETE /api/resource/3 HTTP/1.1
Host: api.example.com
Authorization: Bearer <your_token_here>
```

#### Example Response

```json
{
  "message": "Resource deleted successfully."
}
```

---

## Error Codes

| Code | Message                  | Description                      |
|------|--------------------------|----------------------------------|
| 400  | Bad Request              | Invalid input parameters.       |
| 401  | Unauthorized             | Missing or invalid token.       |
| 403  | Forbidden                | Access denied.                  |
| 404  | Not Found                | Resource not found.             |
| 500  | Internal Server Error    | Server encountered an error.    |

---

## Changelog

- **v1.0.0**
    - Initial release of the API.

---

### Notes
- Always ensure proper authentication.
- Ensure input validation to avoid errors.

---



