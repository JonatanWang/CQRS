# syntax=docker/dockerfile:1

##############################
# BUILD STAGE
##############################
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /src
COPY . .

ARG TARGETARCH

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish \
        -a ${TARGETARCH/amd64/x64} \
        -c Release \
        --use-current-runtime \
        --self-contained false \
        -o /app

##############################
# RUNTIME STAGE
##############################
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final

WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:5021

COPY --from=build /app .

# Copy your SQLite DB files
COPY ReadDb.db WriteDb.db NoCQRS.db /app/

# Fix permissions so the non-root user can access them
RUN chown -R $APP_UID:$APP_UID /app
RUN chmod -R 775 /app

USER $APP_UID

EXPOSE 5021

ENTRYPOINT ["dotnet", "OrdersAPI.dll"]
