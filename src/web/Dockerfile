FROM nginx:alpine AS base
WORKDIR /usr/share/nginx/html
EXPOSE 80
 
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS publish
WORKDIR /src
COPY . .
RUN dotnet publish "CloudNotes.Web.csproj" -c Release -o /app/publish
 
FROM base as final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY --from=publish /src/config.sh /docker-entrypoint.d/
RUN chmod +x /docker-entrypoint.d/config.sh
CMD ["nginx", "-g", "daemon off;"]