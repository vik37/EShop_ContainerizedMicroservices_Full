FROM node:18-alpine as build

WORKDIR /app
COPY WebApp/EShop.SPA/EShop_Administrator/package*.json ./
RUN npm install
RUN npm install -g @angular/cli@15.2.9

COPY WebApp/EShop.SPA/EShop_Administrator/dist/eshop-administrator ./

FROM nginx:alpine3.18-slim

RUN rm /etc/nginx/conf.d/default.*
EXPOSE 80

COPY WebApp/EShop.SPA/EShop_Administrator/nginx.conf /etc/nginx/conf.d/
COPY --from=build /app/ /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]
