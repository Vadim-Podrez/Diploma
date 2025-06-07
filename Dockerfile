# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копіюємо весь репозиторій (винятки у .dockerignore)
COPY . .

RUN dotnet publish DiplomaApi/src/Web/Web.csproj \
    -c Release -o /app/publish /p:UseAppHost=false

# ---------- runtime stage --------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "DiplomaApi.Web.dll"]