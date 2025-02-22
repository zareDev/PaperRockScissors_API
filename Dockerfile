FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PaperRockScissors_API/PaperRockScissors_API.csproj", "PaperRockScissors_API/"]
RUN dotnet restore "./PaperRockScissors_API/./PaperRockScissors_API.csproj"
COPY . .
WORKDIR "/src/PaperRockScissors_API"
RUN dotnet build "./PaperRockScissors_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PaperRockScissors_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaperRockScissors_API.dll"]