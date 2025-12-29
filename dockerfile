# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# copy everything else
COPY . ./
RUN dotnet publish -c Release -o out

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/out .

# Railway sets PORT automatically
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

EXPOSE 8080

ENTRYPOINT ["dotnet", "ExpenseTracker.Api.dll"]
