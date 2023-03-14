FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY RafaStore.Server.csproj RafaStore.Server.csproj
RUN dotnet restore RafaStore.Server.csproj
COPY . .
RUN dotnet publish -c Release -o /output --no-restore --nologo