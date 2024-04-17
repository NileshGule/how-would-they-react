## Start Ollama 2 model using Docker

```bash

docker pull ollama/ollama

docker run -d -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama

docker exec -it ollama ollama run llama2

```
