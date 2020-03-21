# build
FROM mcr.microsoft.com/dotnet/core/sdk:3.1.200-buster AS build
ARG ASSEMBLY_NAME
ARG PROJECT_PATH
WORKDIR /src
COPY . .
RUN dotnet restore "${PROJECT_PATH}/${ASSEMBLY_NAME}.csproj"
WORKDIR "/src/${PROJECT_PATH}"
RUN dotnet publish "${ASSEMBLY_NAME}.csproj" -c Release -o /api

# run
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.2-buster-slim AS runtime
ARG ASSEMBLY_NAME
ENV RUN_ASSEMBLY_FILENAME="${ASSEMBLY_NAME}.dll"
EXPOSE 80
EXPOSE 443
WORKDIR /api
COPY --from=build /api .
ENTRYPOINT dotnet ${RUN_ASSEMBLY_FILENAME}
