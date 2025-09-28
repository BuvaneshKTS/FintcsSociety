# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY FintcsApi/FintcsApi.csproj ./FintcsApi/
COPY fintcss.sln ./
RUN dotnet restore ./FintcsApi/FintcsApi.csproj

# Copy everything else
COPY . .

# Build and publish
WORKDIR /src/FintcsApi
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "FintcsApi.dll"]
