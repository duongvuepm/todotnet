FROM ubuntu:latest
WORKDIR /app
COPY ./publish .
COPY ./publish/TodoApp /app/myapp
RUN pwd && ls -al
ENTRYPOINT ["/app/myapp"]