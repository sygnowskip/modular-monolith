docker-compose up --scale modular-monolith-events-consumer=5 --force-recreate

dotnet run --urls "http://[::1]:0;https://[::1]:0"