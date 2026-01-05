FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Set timezone to Asia/Jakarta (GMT+7)
RUN apt-get update && apt-get install -y tzdata \
&& ln -snf /usr/share/zoneinfo/Asia/Jakarta /etc/localtime \
&& echo "Asia/Jakarta" > /etc/timezone

COPY --from=build /app .
# .NET 8 container default ports:
# HTTP = 8080, HTTPS = 8443
EXPOSE 8080
EXPOSE 8443
ENTRYPOINT ["dotnet", "IngetinGwAPI.dll"]