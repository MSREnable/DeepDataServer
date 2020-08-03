eval $(docker-machine env deepeyes-la)
docker build -t teamgleason/deepdataservice:test .
docker stop test-deep-data-service
docker rm test-deep-data-service
docker system prune -f
docker run -d --name test-deep-data-service --restart always -v /data:/data -e "CONTAINER_NAME=test-deep-data-service" -l traefik.frontend.rule=Host:deepeyes-la.teamgleason.org -l traefik.enable=true -p 52156:52156 teamgleason/deepdataservice:test