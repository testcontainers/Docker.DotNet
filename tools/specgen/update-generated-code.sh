#!/usr/bin/env bash

set -euo pipefail

usage() {
    echo "Usage: $0 <release-tag>"
    echo "Example: $0 docker-v29.1.5"
}

if [[ $# -ne 1 ]]; then
    usage
    exit 1
fi

release_tag="$1"
script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
models_dir="$(cd "$script_dir/../../src/Docker.DotNet/Models" && pwd)"
specgen_bin="$script_dir/specgen"

pushd "$script_dir" > /dev/null

trap 'popd > /dev/null || true' EXIT

echo "Updating moby api package to tag '$release_tag'"
go get -u "github.com/moby/moby/api@$release_tag"

echo "Updating moby client package to tag '$release_tag'"
go get -u "github.com/moby/moby/client@$release_tag"

echo "Building specgen"
go build

echo "Deleting existing generated model classes in '$models_dir'"
find "$models_dir" -maxdepth 1 -type f -name '*.Generated.cs' -delete

echo "Regenerating model classes"
"$specgen_bin" "$models_dir"