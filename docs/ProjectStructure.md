# Project Structure

- **/Teronis.DotNet.sln** covers all projects in this project.
- **/Teronis.DotNet~Publish.sln** covers projects that are candidates to be published to NuGet. When calling `./build.(cmd|ps1|sh) pack` you can find packages in **/obj/packages/**.
- **/lib/**
  - **/lib/built/**: [/lib/README.md](/lib/README.md)
- **/nukebuild/** represents the build project. It has been created with [Nuke](https://github.com/nuke-build/nuke). Please visit and stare their awesome project. :heart:
- **/bin/**
  - **/bin/git-clean-everything.sh** is a **destructive** helper script that **erases everything** that is affected by [.gitignore](/.gitignore).
  - **/bin/push-nuget-packages.ps1** is a helper script invoked in Azure pipeline. Does what is says.
- **/src/** contains all source projects.