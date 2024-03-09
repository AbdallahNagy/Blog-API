FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./API.Blog/Blog.API.csproj"
RUN dotnet build "./API.Blog/Blog.API.csproj" -c Release -o /app/build

FROM build as development
CMD dotnet run --no-launch-profile --project "./API.Blog/Blog.API.csproj"

FROM build AS publish
RUN dotnet publish "./API.Blog/Blog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "./API.Blog/Blog.API.dll"]
