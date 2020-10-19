FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /sln

# Copy solution and project files
COPY ./*.sln  ./

COPY ./src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

COPY ./tests/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p tests/${file%.*}/ && mv $file tests/${file%.*}/; done

RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /sln/publish .
ENTRYPOINT ["dotnet", "Board.WebHost.dll"]