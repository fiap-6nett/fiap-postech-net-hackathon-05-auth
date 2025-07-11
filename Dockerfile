FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FastTech.Usuarios/FastTech.Usuarios.csproj", "FastTech.Usuarios/"]
RUN dotnet restore "FastTech.Usuarios/FastTech.Usuarios.csproj"
COPY . .
WORKDIR "/src/FastTech.Usuarios"
RUN dotnet build "./FastTech.Usuarios.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FastTech.Usuarios.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastTech.Usuarios.dll"]
