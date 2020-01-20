# Teronis.DotNet.GetVersionCache

## Development Requirements

### MSBUILDDISABLENODEREUSE

You need to pass **`MSBUILDDISABLENODEREUSE=1`** as environment variable to Visual Studio. The command for Cygwin would be `export MSBUILDDISABLENODEREUSE=1; /cygdrive/c/Program\ Files\ \(x86\)/Microsoft\ Visual\ Studio/2019/Community/Common7/IDE/devenv.exe`. If not setting the environment variable, you are facing locked file exceptions from msbuild.exe, because it is the subprocess from devenv.exe and remains open with the loaded .dll-file due to cache and therefore perfomance purposes. The lock exception would happen after the second build, as the .dll-file for `UsingTask` is then locked after its first creation.