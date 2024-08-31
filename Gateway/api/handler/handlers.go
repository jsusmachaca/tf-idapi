package handler

import (
	"net/http"
	"strings"

	"github.com/jsusmachaca/tf-idapi/internal/util"
	"github.com/jsusmachaca/tf-idapi/pkg/repository"
)

func GetAll(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")

	token := r.Header.Get("authorization")

	if !strings.HasPrefix(token, "Bearer ") {
		w.WriteHeader(http.StatusUnauthorized)
		w.Write([]byte(`{"error": "token not provided"}`))
		return
	}
	token = token[7:]

	if util.JWTValidator(token) != nil {
		w.Write([]byte(`{"error": "token is not valid"}`))
	}

	w.Write(repository.GetAll())
}

func Filter(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")

	token := r.Header.Get("Authorization")

	if !strings.HasPrefix(token, "Bearer ") {
		w.WriteHeader(http.StatusUnauthorized)
		w.Write([]byte(`{"error": "token not provided"}`))
		return
	}
	token = token[7:]

	if util.JWTValidator(token) != nil {
		w.WriteHeader(http.StatusUnauthorized)
		w.Write([]byte(`{"error": "token is not valid"}`))
		return
	}

	url := r.URL.Path

	params := strings.Split(url, "/")

	if len(params) <= 5 && params[4] != "" {
		w.Write(repository.Filter(params[4]))
		return
	}

	w.WriteHeader(http.StatusNotFound)
	w.Write([]byte(`{"error": "movies not found"}`))
}
