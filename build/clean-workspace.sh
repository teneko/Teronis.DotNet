#!/bin/bash
git_root_directory=$(git rev-parse --show-toplevel)
git clean -xdf -e '${git_root_directory}Teronis.DotNet.sln' \
-e '${git_root_directory}.vs'
