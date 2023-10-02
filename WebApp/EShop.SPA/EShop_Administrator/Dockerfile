FROM node:18-alpine as build

WORKDIR /app
COPY WebApp/EShop.SPA/EShop_Administrator/package*.json ./
RUN npm install

COPY WebApp/EShop.SPA/EShop_Administrator/ ./
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist/eshop-administrator/ /usr/share/nginx/html
EXPOSE 80
