eval $(docker-machine env deepeyes-la)
docker stop traefik
docker rm traefik
docker system prune -f
docker run -d --name traefik  --restart always --log-opt max-size=10m --log-opt max-file=5 --log-opt compress=true -p 80:80 -p 443:443 -p 8080:8080 -v /var/run/docker.sock:/var/run/docker.sock -v /data/traefik:/etc/traefik traefik:v1.7.16
