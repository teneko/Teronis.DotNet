#!/usr/bin/env bash
# Pipeline relevent, but not working in cygwin
#set -euo pipefail
dotnet run --project "src\DotNet\Build\Build\src\Teronis.DotNet.Build.csproj" -- "$@"
