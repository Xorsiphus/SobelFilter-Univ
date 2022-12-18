FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SobelFilterApp/SobelFilterApp.fsproj", "SobelFilterApp/"]
RUN dotnet restore "SobelFilterApp/SobelFilterApp.fsproj"
COPY . .
WORKDIR "/src/SobelFilterApp"
RUN dotnet build "SobelFilterApp.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SobelFilterApp.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SobelFilterApp.dll"]
