#!/usr/bin/env bash
set -euo pipefail
dotnet run --project "src\DotNet\Build\Build\src\Teronis.DotNet.Build.csproj" -- "$@"