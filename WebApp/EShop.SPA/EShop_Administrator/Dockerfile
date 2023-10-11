FROM node:18-alpine as build

WORKDIR /app
COPY WebApp/EShop.SPA/EShop_Administrator/package*.json ./
RUN npm install
RUN npm install -g @angular/cli@15.2.9

COPY WebApp/EShop.SPA/EShop_Administrator/ ./

RUN npm run build


FROM nginx:alpine3.18-slim
COPY --from=build /app/dist/eshop-administrator/ /usr/share/nginx/html
COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf
CMD ["nginx","-g","daemon off;"]
