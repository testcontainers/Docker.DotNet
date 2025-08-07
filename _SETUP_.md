# Post-create setup guide

### Install required packages:

```bash
sudo apt update
sudo apt install --yes iptables uidmap
```

### Start Podman API server:

```bash
podman system service --time=0 unix:///tmp/storage-run-1000/podman/podman.sock
```

### Run the test:

```bash
dotnet test --filter=FullyQualifiedName~GitHub11
```
