FROM node:18-alpine as build-stage
WORKDIR /app
COPY frontend/Task-Manager/package*.json ./
RUN npm install
COPY frontend/Task-Manager/ .
RUN npm run build

FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]