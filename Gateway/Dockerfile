FROM golang:1.23

WORKDIR /app

COPY . .

RUN go build cmd/web/main.go

CMD [ "./main" ]