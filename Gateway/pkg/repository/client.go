package repository

import (
	"io"
	"net/http"
	"os"
)

func GetAll() ([]byte, int) {
	API_URL := os.Getenv("API_URL")

	client := &http.Client{}

	res, err := client.Get(API_URL + "/api/movie")
	if err != nil {
		panic(err)
	}

	defer res.Body.Close()

	status := res.StatusCode
	if status != 200 {
		return nil, status
	}

	data, err := io.ReadAll(res.Body)
	if err != nil {
		panic(err)
	}

	return data, status
}

func Filter(text string) ([]byte, int) {
	API_URL := os.Getenv("API_URL")

	client := &http.Client{}

	res, err := client.Get(API_URL + "/api/movie/search/" + text)
	if err != nil {
		panic(err)
	}

	status := res.StatusCode
	if status != 200 {
		return nil, status
	}

	defer res.Body.Close()

	data, err := io.ReadAll(res.Body)
	if err != nil {
		panic(err)
	}

	return data, status
}
