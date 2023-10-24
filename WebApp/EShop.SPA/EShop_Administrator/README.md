# EShopAdministrator

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 15.0.0.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

## Production NGINX Web Server inside Docker

Navigate to `http://localhost:7077/`. 

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/eshop-administrator` directory.

Building angular (ng-build --configuration production) because the NGINX docker image is configured only to get the production files in the dist folder. For the development environment, it uses the outside links:

`export const environment = {
  production: false,
  CATALOGAPI_URL: "http://localhost:4040/api/v1/",
  ORDERAPI_URL: "http://localhost:5015/api/v1/adminorders/"
};

`
In the production environment Angular works inside NGINX Web Server as a reverse proxy and use a load balancer using the docker host IP internal address.
`
export const environment = {
  production: true,
  CATALOGAPI_URL: "http://host.docker.internal:4040/api/v1/",
  ORDERAPI_URL: "http://host.docker.internal:5015/api/v1/adminorders/"
};
`
