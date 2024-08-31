package repository

import (
	"io"
	"net/http"
	"os"
)

func GetAll() []byte {
	API_URL := os.Getenv("API_URL")

	client := &http.Client{}

	res, err := client.Get(API_URL + "/api/pelicula")
	if err != nil {
		panic(err)
	}

	defer res.Body.Close()

	data, err := io.ReadAll(res.Body)
	if err != nil {
		panic(err)
	}

	return data
}

func Filter(text string) []byte {
	API_URL := os.Getenv("API_URL")

	client := &http.Client{}

	res, err := client.Get(API_URL + "/api/pelicula/search/" + text)
	if err != nil {
		panic(err)
	}

	defer res.Body.Close()

	data, err := io.ReadAll(res.Body)
	if err != nil {
		panic(err)
	}

	return data
}
