FROM nginx:alpine AS base
WORKDIR /usr/share/nginx/html
EXPOSE 80
 
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS publish
WORKDIR /src
COPY . .
#RUN dotnet build "CloudNotes.Web.csproj" -c Release -o /app/build
#FROM build AS publish
RUN dotnet publish "CloudNotes.Web.csproj" -c Release -o /app/publish
 
FROM base as final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
CMD ["nginx", "-g", "daemon off;"]