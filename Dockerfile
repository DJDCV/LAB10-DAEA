# Usa imagen oficial de .NET 8 para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia todos los archivos
COPY . ./

# Restaura dependencias
RUN dotnet restore

# Publica en modo release
RUN dotnet publish ./Lab10.API/Lab10.API.csproj -c Release -o /out

# Imagen de runtime más liviana
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia los archivos publicados
COPY --from=build /out ./

# Expone el puerto por defecto
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Comando de inicio
ENTRYPOINT ["dotnet", "Lab10.API.dll"]
