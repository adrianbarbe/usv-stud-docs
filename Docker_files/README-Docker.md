docker build -f Dockerfile_BE -t usvstud_be .
docker build -f Dockerfile_UI -t usvstud_ui .

docker login --username=adryanbarbe --email=adryanbarbe@gmail.com

docker login -u username -p password registry.hub.docker.com/myproject
docker stack deploy --compose-file docker-compose.yml usvstud --with-registry-auth
