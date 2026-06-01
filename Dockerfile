FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY BlazorApp21.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish BlazorApp21.csproj -c Release -o /app/publish

FROM nginx:alpine
RUN apk add --no-cache bash
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf

CMD sh -c "envsubst '\$PORT' < /etc/nginx/nginx.conf > /tmp/nginx.conf && nginx -c /tmp/nginx.conf -g 'daemon off;'"