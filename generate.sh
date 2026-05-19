#!/bin/bash

set -euo pipefail

export EXE=~/work/teamcity-build-prosector/TeamCityBuildProsector/src/bin/Debug/net10.0/TeamCityBuildProsector
export DIR=~/work/_/generated-configs

"$EXE" -o md  -m short -f ijplatform_master_Net "$DIR" >short-net.md
"$EXE" -o tsv -m short -f ijplatform_master_Net "$DIR" >short-net.tsv

"$EXE" -o md  -m short -f ijplatform_master_Idea "$DIR" >short-idea.md
"$EXE" -o tsv -m short -f ijplatform_master_Idea "$DIR" >short-idea.tsv

