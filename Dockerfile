FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
# FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.2-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["HangFireService.csproj", "./"]
RUN dotnet restore "./HangFireService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "HangFireService.csproj" -c Release -o /app/build
#RUN apt-get update && \
#    apt-get install -y wget && \
#    apt-get install -y gnupg2 && \
#    wget -qO- https://deb.nodesource.com/setup_10.x | bash - && \
#    apt-get install -y build-essential nodejs
 
FROM build AS publish
RUN dotnet publish "HangFireService.csproj" -c Release -o /app/publish
 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HangFireService.dll"]