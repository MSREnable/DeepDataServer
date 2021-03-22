docker build -t teamgleason/deepdataservice:test .
docker stop test-deep-data-service
docker rm test-deep-data-service
docker system prune -f
docker run -d --name test-deep-data-service --restart always -v /data/test:/data -e "CONTAINER_NAME=test-deep-data-service" -l traefik.frontend.rule=Host:test-tg-queue.37services.org -l traefik.port=52156 -l traefik.enable=true teamgleason/deepdataservice:test
