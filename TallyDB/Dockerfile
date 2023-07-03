FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a debug release
RUN dotnet publish -c Debug -o out /p:DefineConstants=ENV_DOCKER

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .

EXPOSE 4053
ENTRYPOINT ["dotnet", "TallyDB.dll"]