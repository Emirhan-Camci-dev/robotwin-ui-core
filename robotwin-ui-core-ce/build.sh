#!/bin/bash
set -e

echo "Building RoboTwin-UI Core (Community Edition)..."

# 1. Build C++ Native Core
echo "==> Compiling Native C++ FFI..."
mkdir -p src/RoboTwin.Native/build
cd src/RoboTwin.Native/build
cmake -DCMAKE_BUILD_TYPE=Release ..
cmake --build .
cd ../../../

# 2. Build .NET C# Library
echo "==> Compiling .NET 8 UI Core..."
dotnet build RoboTwin-CE.sln -c Release

echo "Build Complete! Check bin/Release output."
