#!/usr/bin/env bash
set -euo pipefail
dotnet run --project "src\Build\src\Build\Teronis.DotNet.Build.csproj" -- "$@"