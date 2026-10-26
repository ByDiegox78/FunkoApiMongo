# Etapa 1: Compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar el archivo del proyecto y restaurar dependencias
COPY ["*.csproj", "./"]
RUN dotnet restore

# Copiar el resto del código y compilar la aplicación
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Copiar los binarios compilados de la etapa anterior
COPY --from=build /app/publish .

# Comando de inicio
ENTRYPOINT ["dotnet", "FunkoApiMongo.dll"]