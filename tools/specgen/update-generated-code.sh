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

docker_version="${release_tag#docker-}"
directory_build_props_path="$script_dir/../../Directory.Build.props"

pushd "$script_dir" > /dev/null

cleanup() {
    rm -f "$specgen_bin" "$directory_build_props_path.bak"
    popd > /dev/null || true
}

trap cleanup EXIT

echo "Updating moby api package to tag '$release_tag'"
go get "github.com/moby/moby/api@$release_tag"

echo "Updating moby client package to tag '$release_tag'"
go get "github.com/moby/moby/client@$release_tag"

# Use a backup suffix that in-place editing works with both GNU and BSD sed.
echo "Updating props DockerVersion with '$docker_version'"
sed -i.bak "s|<DockerVersion>.*</DockerVersion>|<DockerVersion>$docker_version</DockerVersion>|" "$directory_build_props_path"

echo "Building specgen"
go build

echo "Deleting existing generated model classes in '$models_dir'"
find "$models_dir" -maxdepth 1 -type f -name '*.Generated.cs' -delete

echo "Regenerating model classes"
"$specgen_bin" "$models_dir"