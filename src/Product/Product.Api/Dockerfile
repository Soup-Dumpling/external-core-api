# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
LABEL product.api.name=api
LABEL product.api.intermediate=true
WORKDIR /src
RUN dotnet restore "Product.Api.csproj"
COPY ./src ./
WORKDIR "/src/Product/Product.Api/"
RUN dotnet build "Product.Api.csproj"

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Product.Api.csproj" -c Release -o /app/publish /p:InformationalVersion=$BUILD_CONFIGURATION --no-restore

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
LABEL product.api.name=api
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Product.Api.dll"]
