cmake_minimum_required(VERSION 3.20)
project(RoboTwinNativeCore VERSION 1.0)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# The core C++ library providing zero-copy video frame bridging and basic FFI
add_library(robotwin_native_core SHARED
    # Add source files here... (e.g., src/video_bridge.cpp)
)

# Target dependencies (e.g., SDL2, FFmpeg wrappers)
# find_package(SDL2 REQUIRED)
# target_link_libraries(robotwin_native_core PRIVATE SDL2::SDL2)
