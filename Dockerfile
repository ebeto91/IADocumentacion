# Establece la imagen base para la etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo de proyecto y restaura las dependencias
COPY ["RAS823_MC_CiudadMunicipal_FrontEnd.csproj", "./"]
RUN dotnet restore "RAS823_MC_CiudadMunicipal_FrontEnd.csproj"

# Copia el resto de los archivos y construye el proyecto
COPY . .
WORKDIR "/src/."
RUN dotnet build "RAS823_MC_CiudadMunicipal_FrontEnd.csproj" -c Release -o /app/build

# Publica el proyecto
FROM build AS publish
RUN dotnet publish "RAS823_MC_CiudadMunicipal_FrontEnd.csproj" -c Release -o /app/publish

# Establece la imagen base para la etapa final
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .

# Copia el archivo de configuración de Nginx
COPY nginx/default.conf /etc/nginx/conf.d/default.conf

# Exponer el puerto 80
EXPOSE 80

# Comando para ejecutar Nginx
CMD ["nginx", "-g", "daemon off;"]