tag=$(date '+%Y-%m-%d')-prod
docker tag teamgleason/deepdataservice:test teamgleason/deepdataservice:$tag
docker stop deep-data-service
docker rm deep-data-service
docker system prune -f
docker run -d --name deep-data-service --restart always -v /data/prod:/data -e "CONTAINER_NAME=deep-data-service" -l traefik.frontend.rule=Host:tg-queue.37services.org -l traefik.port=52156 -l traefik.enable=true teamgleason/deepdataservice:$tag
