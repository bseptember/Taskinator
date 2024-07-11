# Taskinator 🐊

A `To-Do list` app that performs CRUD operations. Made with 
`.Net MAUI`
`Realm`
`Microsoft.Recognizers.Text`
`SkiaSharp.Extended.UI.Maui`
`Plugin.Maui.Calendar`

# CI/CD Status & Release

[![CI Build]( )

# Maintenance Status 🔹<a href="https://github.com/bseptember/Taskinator/issues">Report Bug</a> &nbsp; &nbsp;

![maintenance-status](https://img.shields.io/badge/maintenance-actively--developed-brightgreen.svg)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)

# Supported Platforms

<table>
  <tr>
    <th>Platform</th>
    <th>Version</th>
    <th>Target</th>
  </tr>
  <tr>
    <td>Android</td>
    <td>API 21+</td>
    <td>API 34 / Android 14</td>
  </tr>
  <tr>
    <td>iOS</td>
    <td>iOS 11+</td>
    <td>iOS 16.7 & 17.4.1</td>
  </tr>
    <tr>
    <td>Windows</td>
    <td>Windows 11+</td>
    <td>10.0.17763.0</td>
  </tr>
</table>

# Required SDKs

- .Net 8.0 from <a href="https://dotnet.microsoft.com/download/dotnet/8.0" target="_blank">`here`</a>
- XCode 15 from <a href="https://developer.apple.com/xcode/" target="_blank">`here`</a>

# Screens

## Android
[Android](https://github.com/bseptember/Taskinator/assets/68727050/01ab1363-c4b4-48a2-a549-93f5fb103c3a)

## Windows
[Windows](https://github.com/bseptember/Taskinator/assets/68727050/66ab3cd1-9aee-4670-826b-61483efd05a3)

## Upcoming

- [ ] Swipe gestures
- [ ] Overlapping events

## Getting Started

- Install `.NET 8` SDK from <a href="https://dotnet.microsoft.com/download/dotnet/8.0" target="_blank">`here`</a> on your machine.
- Install <a href="https://visualstudio.microsoft.com/downloads/" target="_blank">`Visual Studio`</a> on your machine and while choosing components you must check the <a href="https://dotnet.microsoft.com/en-us/learn/maui/first-app-tutorial/install" target="_blank">`.NetMaui`</a> box to install .NetMaui.
- Install <a href="https://developer.android.com/studio?gclid=Cj0KCQiAnNacBhDvARIsABnDa6-EYNc5MIjFoAruujioi9l-gjeu8JVsJd_aqCGGhImxOZkFyoo_woYaAoOCEALw_wcB&gclsrc=aw.ds" target="_blank">`Android Studio`</a> on your machine.
- Create a virtual device with andoid API 31 || 32 || 33 ||34.
- Clone, download or fork this repository.
- Open the solution file, build then run with selected device.
- If build failed with Dependency errors, please unload the project and reload with dependencies.
- Or `cd` to the project directory and run `dotnet restore {name}.sln` to restore dependencies.

## Getting Started iOS

- Install `.NET 8` SDK from <a href="https://dotnet.microsoft.com/download/dotnet/8.0" target="_blank">`here`</a> on your machine.
- Install <a href="https://visualstudio.microsoft.com/vs/mac/" target="_blank">`Visual Studio for mac`</a> on your machine and while choosing components you must check the `.NetMaui` box.
- Install <a href="https://developer.apple.com/xcode/" target="_blank">`XCode`</a> on your machine.
- Clone, download or fork this repository.
- Open the solution file, build then run with selected device iOS 15+.
- If build failed with Dependency errors, please unload the project and reload with dependencies.
- Or `cd` to the project directory and run `dotnet restore {name}.sln` to restore dependencies.

## Permissions
- Android: `Internet`, `Read & Write Internal Storage`, `Biometric fingerprint`, `Haptic feedback`.
- iOS: `Read & Write Internal Storage`, `Haptic feedback`.
