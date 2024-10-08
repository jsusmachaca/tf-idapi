package main

import (
	"fmt"
	"log"
	"net/http"
	"os"

	"github.com/joho/godotenv"
	"github.com/jsusmachaca/tf-idapi/api/handler"
)

func init() {
	if err := godotenv.Load(); err != nil {
		log.Println("No .env file found")
	}
}

func main() {
	PORT := ":"
	PORT += os.Getenv("PORT")
	fmt.Println(PORT)
	http.HandleFunc("/api/movie", handler.GetAll)
	http.HandleFunc("/api/movie/search/", handler.Filter)
	http.ListenAndServe(PORT, nil)
}
