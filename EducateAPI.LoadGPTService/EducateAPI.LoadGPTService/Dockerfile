# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY . ./
COPY EducateAPI.LoadGPTService/*.csproj ./EducateAPI.LoadGPTService/

RUN dotnet restore

COPY EducateAPI.LoadGPTService/. .

# Build and publish in Release mode
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copy the build output from the first stage
COPY --from=build-env /app/publish .

ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 80
EXPOSE 443

# Start the application
ENTRYPOINT ["dotnet", "EducateAPI.LoadGPTService.dll"]
