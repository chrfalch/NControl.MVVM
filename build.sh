#!/bin/bash
#
set -e # Exit on any failure for any command
set -x # Print all commands before running them - for easy debugging.

## Clean
xbuild /p:Configuration=Release /p:Platform=Release /target:Clean /p:OutputPath=build NControl.MVVM/NControl.Mvvm.csproj
rm -rf NControl.MVVM/build/
xbuild /p:Configuration=Release /p:Platform=Release /target:Clean /p:OutputPath=build NControl.MVVM.iOS/NControl.Mvvm.iOS.csproj
rm -rf NControl.MVVM.iOS/build/
xbuild /p:Configuration=Release /p:Platform=Release /target:Clean /p:OutputPath=build NControl.MVVM.Droid/NControl.Mvvm.Droid.csproj
rm -rf NControl.MVVM.Droid/build/

## Build with parameters
xbuild /p:Configuration=Release /p:Platform=Release /target:build /p:OutputPath=build NControl.MVVM/NControl.Mvvm.csproj
xbuild /p:Configuration=Release /p:Platform=Release /target:build /p:OutputPath=build NControl.MVVM.iOS/NControl.Mvvm.iOS.csproj
xbuild /p:Configuration=Release /p:Platform=Release /target:build /p:OutputPath=build NControl.MVVM.Droid/NControl.Mvvm.Droid.csproj

mkdir -p ../build
nuget pack -OutputDirectory ../build
