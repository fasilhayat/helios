SOLUTION_ROOT=..
SOLUTION_FILE=$(SOLUTION_ROOT)/AspirePlayground.sln
APPHOST=$(SOLUTION_ROOT)/src/AspirePlayground.AppHost/AspirePlayground.AppHost.csproj
API=$(SOLUTION_ROOT)/src/Api/Api.csproj
PID_FILE=.aspire.pid

.DEFAULT_GOAL := help

help:
	@echo Available commands:
	@echo   run
	@echo   build
	@echo   clean
	@echo   restore

run:
	dotnet run --project $(APPHOST)

build:
	dotnet build $(SOLUTION_FILE)

restore:
	dotnet restore $(SOLUTION_FILE)

clean:
	dotnet clean $(SOLUTION_FILE)