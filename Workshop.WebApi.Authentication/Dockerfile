FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8443
ENV ASPNETCORE_HTTPS_PORTS=8443
ENV ASPNETCORE_HTTP_PORTS=8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Workshop.WebApi.Authentication/Workshop.WebApi.Authentication.csproj", "Workshop.WebApi.Authentication/"]
COPY ["Workshop.Shared/Workshop.Shared.csproj", "Workshop.Shared/"]
RUN dotnet restore "Workshop.WebApi.Authentication/Workshop.WebApi.Authentication.csproj"
COPY . .
WORKDIR "/src/Workshop.WebApi.Authentication"
RUN dotnet build "Workshop.WebApi.Authentication.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Workshop.WebApi.Authentication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
RUN mkdir /app/publish/certs
RUN dotnet dev-certs https -ep /app/publish/certs/aspnetapp.pfx -p "123456"
RUN chmod -R 777 /app/publish/certs

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN dotnet dev-certs https --trust
ENTRYPOINT ["dotnet", "Workshop.WebApi.Authentication.dll"]
