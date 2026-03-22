FROM node:20 as build

WORKDIR /app

COPY src/EShop.StoreFront/package.json package.json
COPY src/EShop.StoreFront/package-lock.json package-lock.json

RUN npm install

COPY src/EShop.StoreFront/ .

RUN npm run build

FROM nginx:alpine

COPY src/EShop.StoreFront/default.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html

# Expose the default nginx port
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]