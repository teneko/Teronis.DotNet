# Folder **built/**

The folder **built/** contains binaries compiled once. They are needed in process of building the projects. You can rebuild them by `/build.(cmd|ps1|sh) cleanbinary compilebinary` where the target `cleanbinary` cleans the built-folder and the target `compilebinary` compiles them.

This approach is better than the approach to implement an incremental build safe process for creating these binaries dynamically before building projects in Visual Studio or aquivalent.