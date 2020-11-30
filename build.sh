#!/usr/bin/env bash
# Pipeline relevent, but not working in cygwin
#set -euo pipefail
dotnet run --project "src\Build\src\Teronis.Build.csproj" -- "$@"
