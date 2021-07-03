FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DotNet-Dojo-RafaelMarcos.csproj", "./"]
RUN dotnet restore "DotNet-Dojo-RafaelMarcos.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "DotNet-Dojo-RafaelMarcos.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNet-Dojo-RafaelMarcos.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNet-Dojo-RafaelMarcos.dll"]
