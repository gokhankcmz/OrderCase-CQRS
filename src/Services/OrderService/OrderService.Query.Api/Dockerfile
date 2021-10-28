#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Services/OrderService/OrderService.Query.Api/OrderService.Query.Api.csproj", "src/Services/OrderService/OrderService.Query.Api/"]
COPY ["src/Services/OrderService/OrderService.Application/OrderService.Application.csproj", "src/Services/OrderService/OrderService.Application/"]
COPY ["src/Common/CommonLib/CommonLib.csproj", "src/Common/CommonLib/"]
COPY ["src/Common/Repository/Repository.csproj", "src/Common/Repository/"]
COPY ["src/Common/Entities/Entities.csproj", "src/Common/Entities/"]
RUN dotnet restore "src/Services/OrderService/OrderService.Query.Api/OrderService.Query.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/OrderService/OrderService.Query.Api"
RUN dotnet build "OrderService.Query.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderService.Query.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Query.Api.dll"]
